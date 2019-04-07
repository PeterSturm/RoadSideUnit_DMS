using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;

namespace SNMPManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerLogController : BaseController
    {
        public ManagerLogController(IContextService contextService, ILogger logger)
            : base(contextService, logger)
        {
        }
        
        // GET api/values
        [HttpGet("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<ManagerLog>> Get(string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetManagerLogs();
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.OrderByDescending(l => l.TimeStamp).ToList();
        }

        [HttpGet("{username}/{token}/{from}/{to}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<ManagerLog>> Get(string username, string token, string from, string to)
        {
            DateTime datefrom;
            DateTime dateto;

            if (!DateTime.TryParse(from, out datefrom))
                return BadRequest(from);
            if (!DateTime.TryParse(to, out dateto))
                return BadRequest(to);

            if (datefrom > dateto)
                return BadRequest();

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetManagerLogs(datefrom, dateto);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.OrderByDescending(l => l.TimeStamp).ToList();
        }

        [HttpGet("{username}/{token}/{logtype}/{from}/{to}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<ManagerLog>> Get(string username, string token, LogType logtype, string from, string to)
        {
            DateTime datefrom;
            DateTime dateto;

            if (!DateTime.TryParse(from, out datefrom))
                return BadRequest(from);
            if (!DateTime.TryParse(to, out dateto))
                return BadRequest(to);

            if (datefrom > dateto)
                return BadRequest();

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetManagerLogs(logtype, datefrom, dateto);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.OrderByDescending(l => l.TimeStamp).ToList();
        }
    }
}