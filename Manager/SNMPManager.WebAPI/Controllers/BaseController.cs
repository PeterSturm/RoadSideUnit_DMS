using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SNMPManager.Core.Entities;
using SNMPManager.Core.Enumerations;
using SNMPManager.Core.Exceptions;
using SNMPManager.Core.Interfaces;

namespace SNMPManager.WebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IContextService _contextService;
        protected readonly ILogger _logger;

        public BaseController(IContextService ContextService, ILogger logger)
        {
            _contextService = ContextService;
            _logger = logger;
        }

        protected ActionResult AuthenticateAuthorize(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return Unauthorized();

            try
            {
                _contextService.AuthorizeUser(userName, token);
            }
            catch (AuthorizationFailed author) { return Unauthorized(author.Message); }
            catch (AuthenticationFailed authen) { return Forbid(authen.Message); }

            return null;
        }
    }
}