using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using SNMPManager.WebAPI.Models;

namespace SNMPManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IContextService contextService, ILogger logger)
            : base(contextService, logger)
        {

        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<UserModel>> Get(string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var users = _contextService.GetUser();
            if (users == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return users.Select(u => UserModel.MapFromEntity(u)).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<UserModel> Get(int id, string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var user = _contextService.GetUser(id);
            if (user == null)
                return NotFound(id);

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return UserModel.MapFromEntity(user);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public IActionResult Post([FromBody] UserModel user, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.AddUser(UserModel.MaptoEntity(user)))
                return Conflict();
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(user);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public IActionResult Put([FromBody] UserModel user, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.UpdateUser(UserModel.MaptoEntity(user)))
                return NotFound(user);
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(user);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public IActionResult Delete(int userid, string username, string token)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!_contextService.RemoveUser(userid))
                return NotFound(userid);
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok();
            }
        }
    }
}
