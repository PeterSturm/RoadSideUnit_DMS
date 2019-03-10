using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using SNMPManager.Core.Exceptions;

namespace SNMPManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSUController : ControllerBase
    {
        private readonly ISNMPManagerServices _SNMMPManagerService;
        private readonly ILogger _logger;

        public RSUController(ISNMPManagerServices SNMPManagerService, ILogger logger)
        {
            _SNMMPManagerService = SNMPManagerService;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<RSU>> Get(string username, string token)
        {
            /*
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;
            */

            var rsus = _SNMMPManagerService.GetRSU();
            if (rsus == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerTask.ADMINISTRATION, Operation.SELECT);

            return rsus.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<RSU> Get(int id, string username, string token)
        {
            /*
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;
            */

            var rsu = _SNMMPManagerService.GetRSU(id);
            if (rsu == null)
                return NotFound(id);

            _logger.LogAPICall(username, ManagerTask.ADMINISTRATION, Operation.SELECT);

            return rsu;
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] RSU rsu, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_SNMMPManagerService.AddRSU(rsu))
                return Conflict();
            else
            {
                _logger.LogAPICall(username, ManagerTask.ADMINISTRATION, Operation.INSERT);
                return Ok(rsu);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public IActionResult Put([FromBody] RSU rsu, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_SNMMPManagerService.UpdateRSU(rsu))
                return NotFound(rsu);
            else
            {
                _logger.LogAPICall(username, ManagerTask.ADMINISTRATION, Operation.UPDATE);
                return Ok(rsu);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public IActionResult Delete(int rsuid, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!_SNMMPManagerService.RemoveRSU(rsuid))
                return NotFound(rsuid);
            else
            {
                _logger.LogAPICall(username, ManagerTask.ADMINISTRATION, Operation.DELETE);
                return Ok();
            }
        }

        private IActionResult AuthenticateAuthorize(string userName, string token)
        {
            try
            {
                _SNMMPManagerService.AuthorizeUser(userName, token, ManagerTask.ADMINISTRATION);
            }
            catch (AuthorizationFailed author) { return Unauthorized(author.Message); }
            catch (AuthenticationFailed authen) { return Forbid(authen.Message); }

            return null;
        }
    }
}
