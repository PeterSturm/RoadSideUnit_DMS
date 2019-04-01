using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Interfaces;
using SNMPManager.Core.Exceptions;
using SNMPManager.WebAPI.Controllers;
using SNMPManager.WebAPI.Models;
using DTO;
using System.Net;

namespace SNMPManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSUController : BaseController
    {
        public RSUController(IContextService contextService, ILogger logger)
            : base(contextService, logger)
        {

        }

        [HttpGet("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<RsuDto>> Get(string username, string token)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var rsus = _contextService.GetRSU();
            if (rsus == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return rsus.Select(r => new RsuDto {
                Id = r.Id,
                IP = r.IP.ToString(),
                Port = r.Port,
                Name = r.Name,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Active = r.Active,
                MIBVersion = r.MIBVersion,
                FirmwareVersion = r.FirmwareVersion,
                LocationDescription = r.LocationDescription,
                Manufacturer = r.Manufacturer,
                NotificationIP = r.NotificationIP.ToString(),
                NotificationPort = r.NotificationPort
            }).ToList();
        }

        [HttpGet("{username}/{token}/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<RsuDto> Get(string username, string token, int id)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var rsu = _contextService.GetRSU(id);
            if (rsu == null)
                return NotFound(id);

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return new RsuDto
            {
                Id = rsu.Id,
                IP = rsu.IP.ToString(),
                Port = rsu.Port,
                Name = rsu.Name,
                Latitude = rsu.Latitude,
                Longitude = rsu.Longitude,
                Active = rsu.Active,
                MIBVersion = rsu.MIBVersion,
                FirmwareVersion = rsu.FirmwareVersion,
                LocationDescription = rsu.LocationDescription,
                Manufacturer = rsu.Manufacturer,
                NotificationIP = rsu.NotificationIP.ToString(),
                NotificationPort = rsu.NotificationPort
            };
        }

        [HttpGet("{username}/{token}/{ip}/{port}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<IEnumerable<RsuDto>> Get(string username, string token, string ip, int port)
        {

            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;


            var rsus = _contextService.GetRSU()
                .Where(r => r.IP.ToString() == ip
                           && r.Port == port);
            if (rsus == null)
                return NotFound();

            _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);

            return rsus.Select(r => new RsuDto
            {
                Id = r.Id,
                IP = r.IP.ToString(),
                Port = r.Port,
                Name = r.Name,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Active = r.Active,
                MIBVersion = r.MIBVersion,
                FirmwareVersion = r.FirmwareVersion,
                LocationDescription = r.LocationDescription,
                Manufacturer = r.Manufacturer,
                NotificationIP = r.NotificationIP.ToString(),
                NotificationPort = r.NotificationPort
            }).ToList();
        }

        [HttpPost("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public IActionResult Post(string username, string token, [FromBody] RsuDto rsu)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.AddRSU(new RSU
            {
                Id = rsu.Id,
                IP = IPAddress.Parse(rsu.IP),
                Port = rsu.Port,
                Name = rsu.Name,
                Latitude = rsu.Latitude,
                Longitude = rsu.Longitude,
                Active = rsu.Active,
                MIBVersion = rsu.MIBVersion,
                FirmwareVersion = rsu.FirmwareVersion,
                LocationDescription = rsu.LocationDescription,
                Manufacturer = rsu.Manufacturer,
                NotificationIP = IPAddress.Parse(rsu.NotificationIP),
                NotificationPort = rsu.NotificationPort
            }))
                return Conflict();
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(rsu);
            }
        }

        [HttpPut("{username}/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public IActionResult Put(string username, string token, [FromBody] RsuDto rsu)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_contextService.UpdateRSU(new RSU
            {
                Id = rsu.Id,
                IP = IPAddress.Parse(rsu.IP),
                Port = rsu.Port,
                Name = rsu.Name,
                Latitude = rsu.Latitude,
                Longitude = rsu.Longitude,
                Active = rsu.Active,
                MIBVersion = rsu.MIBVersion,
                FirmwareVersion = rsu.FirmwareVersion,
                LocationDescription = rsu.LocationDescription,
                Manufacturer = rsu.Manufacturer,
                NotificationIP = IPAddress.Parse(rsu.NotificationIP),
                NotificationPort = rsu.NotificationPort
            }))
                return NotFound(rsu);
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok(rsu);
            }
        }

        [HttpDelete("{username}/{token}/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public IActionResult Delete(string username, string token, int rsuid)
        {
            var securityProblem = AuthenticateAuthorize(username, token);
            if (securityProblem != null)
                return securityProblem;

            if (!_contextService.RemoveRSU(rsuid))
                return NotFound(rsuid);
            else
            {
                _logger.LogAPICall(username, ManagerOperation.ADMINISTRATION);
                return Ok();
            }
        }
    }
}
