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
        public ActionResult<object> Get(string username, string token, int rsuid, string OID)
        {
            // TODO solve auth with ActionResult return type

            //var securityProblem = AuthenticateAuthorize(username, token);
            //if (securityProblem != null)
            //    return securityProblem;

            var rsu = _contextService.GetRSU(rsuid);
            if (rsu == null)
                return NotFound();
            // TODO finish funcionality

            // TODO fix return
            return Ok();
        }

        public IActionResult Set(string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            // TODO finish funcionality

            // TODO fix return
            return Ok();
        }
    }
}