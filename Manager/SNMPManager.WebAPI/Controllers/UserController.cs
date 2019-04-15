using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;

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
        [HttpGet("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<UserDto>> Get(string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var users = _contextService.GetUser();
            if (users == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Token = u.Token,
                Role = u.Role.Name,
                SnmPv3Auth = u.SNMPv3Auth,
                SnmPv3Priv = u.SNMPv3Priv
            }).ToList();
        }

        // GET api/values/5
        [HttpGet("{username}/{token}/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<UserDto> Get(string username, string token, int id)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var user = _contextService.GetUser(id);
            if (user == null)
                return NotFound(id);

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = user.Token,
                Role = user.Role.Name,
                SnmPv3Auth = user.SNMPv3Auth,
                SnmPv3Priv = user.SNMPv3Priv
            };
        }

        // POST api/values
        [HttpPost("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public IActionResult Post(string username, string token, [FromBody] UserDto user)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.AddUser(new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = user.Token,
                Role = _contextService.GetRole(user.Role),
                SNMPv3Auth = user.SnmPv3Auth,
                SNMPv3Priv = user.SnmPv3Priv
            }))
                return Conflict();
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(user);
            }
        }

        // PUT api/values/5
        [HttpPut("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public IActionResult Put(string username, string token, [FromBody] UserDto user)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.UpdateUser(new User
            {
                Id = user.Id,
                UserName = user.UserName,
                Token = user.Token,
                Role = _contextService.GetRole(user.Role),
                SNMPv3Auth = user.SnmPv3Auth,
                SNMPv3Priv = user.SnmPv3Priv
            }))
                return NotFound(user);
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(user);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{username}/{token}/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public IActionResult Delete(string username, string token, int userid)
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
