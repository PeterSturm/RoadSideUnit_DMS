using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using SNMPManager.Core.Exceptions;
using Lextm.SharpSnmpLib;
using Common.DTO;

namespace SNMPManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SNMPController : BaseController
    {
        private readonly ISNMPManagerService _snmpManagerService;

        public SNMPController(IContextService contextService, ISNMPManagerService snmpManagerService, ILogger logger)
            :base(contextService, logger)
        {

            _snmpManagerService = snmpManagerService;
            _snmpManagerService.Configure(_contextService.GetManagerSettings());
        }

        [HttpGet("{username}/{token}/{rsuid}/{OID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<MIBObjectDto>> Get(string username, string token, int rsuid, string OID)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            var user = _contextService.GetUser(username);

            var rsu = _contextService.GetRSU(rsuid);
            if (rsu == null)
                return NotFound($"RSU with id {rsuid} not found!");

            List<MIBObject> mibobjects;
            try
            {
                mibobjects = _snmpManagerService.Get(rsu, user, OID);
                if (mibobjects == null)
                    return NotFound();

            }
            // TODO change returns to more proper ones
            catch (ReplyIsReportMessage) { return NotFound(); }
            catch (SnmpGetError) { return NotFound(); }

            return mibobjects.Select(mibo => new MIBObjectDto {
                    Oid = mibo.OID,
                    Type = mibo.Type.ToString(),
                    Value = mibo.Value
                }).ToList();
        }

        [HttpPost("{username}/{token}/{rsuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Set(string username, string token, int rsuid, [FromBody] MIBObjectDto mibObject)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            var user = _contextService.GetUser(username);

            var rsu = _contextService.GetRSU(rsuid);
            if (rsu == null)
                return NotFound($"RSU with id {rsuid} not found!");

            MIBObject mibo = new MIBObject(mibObject.Oid, mibObject.Type, mibObject.Value);

            try
            {
                if (_snmpManagerService.Set(rsu, user, mibo.OID, mibo.Type, mibo.Value))
                    return Ok();
                else
                    return StatusCode(500);
            }
            catch (ReplyIsReportMessage) { return NotFound(); }
            catch (InvalidDataType) { return BadRequest("Invalid data type"); }
            catch (FormatException) { return BadRequest($"Invalid value for type {mibObject.Type.ToString()}"); }
        }
    }
}