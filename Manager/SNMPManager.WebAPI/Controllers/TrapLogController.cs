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
    public class TrapLogController : BaseController
    {
        public TrapLogController(IContextService contextService, ILogger logger)
            : base(contextService, logger)
        {
        }
        
        // GET api/values
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<TrapLog>> Get(string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetTrapLogs();
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.ToList();
        }

        [HttpGet("{from}/{to}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<TrapLog>> Get(string username, string token, string from, string to)
        {
            DateTime datefrom;
            DateTime dateto;

            if (!DateTime.TryParse(from, out datefrom))
                return BadRequest(from);
            if (!DateTime.TryParse(to, out dateto))
                return BadRequest(to);

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetTrapLogs(datefrom, dateto);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.ToList();
        }

        [HttpGet("{type}/{from}/{to}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<TrapLog>> Get(string username, string token, LogType logtype, string from, string to)
        {
            DateTime datefrom;
            DateTime dateto;

            if (!DateTime.TryParse(from, out datefrom))
                return BadRequest(from);
            if (!DateTime.TryParse(to, out dateto))
                return BadRequest(to);

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetTrapLogs(logtype, datefrom, dateto);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.ToList();
        }

        [HttpGet("{rsuid}/{from}/{to}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<TrapLog>> Get(string username, string token, int rsuId, string from, string to)
        {
            DateTime datefrom;
            DateTime dateto;

            if (!DateTime.TryParse(from, out datefrom))
                return BadRequest(from);
            if (!DateTime.TryParse(to, out dateto))
                return BadRequest(to);

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetTrapLogs(rsuId, datefrom, dateto);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.ToList();
        }

        [HttpGet("{rsuid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<TrapLog>> Get(string username, string token, int rsuId)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var logs = _contextService.GetTrapLogs(rsuId);
            if (logs == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return logs.ToList();
        }
    }
}