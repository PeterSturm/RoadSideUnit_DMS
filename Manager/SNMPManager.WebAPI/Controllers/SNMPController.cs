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

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<MIBObject>> Get(string username, string token, int rsuid, string OID)
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

            return mibobjects;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Set(string username, string token, int rsuid, string OID, SnmpType type, string value)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            var user = _contextService.GetUser(username);

            var rsu = _contextService.GetRSU(rsuid);
            if (rsu == null)
                return NotFound($"RSU with id {rsuid} not found!");

            try
            {
                if (_snmpManagerService.Set(rsu, user, OID, type, value))
                    return Ok();
                else
                    return StatusCode(500);
            }
            catch (ReplyIsReportMessage) { return NotFound(); }
            catch (InvalidDataType) { return BadRequest("Invalid data type"); }
            catch (FormatException) { return BadRequest($"Invalid value for type {type.ToString()}"); }
        }
    }
}