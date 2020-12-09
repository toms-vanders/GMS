using AuthenticationService.Managers;
using AuthenticationService.Models;
using GMS___Business_Layer;
using GMS___Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace GMS___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IOptions<ClientSettings> clientSettings;
        private UserProcessor userProcessor;
        public UserController(IOptions<ClientSettings> clientSettings)
        {
            this.clientSettings = clientSettings;
            userProcessor = new UserProcessor();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> LogIn([FromBody] User user)
        {
            User tempUser = userProcessor.LogInUser(user.UserName, user.Password);
            if (tempUser != null)
            {
                IAuthContainerModel model = GetJWTContainerModel(tempUser.UserName, tempUser.EmailAddress, tempUser.UserRole);
                IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
                return authService.GenerateToken(model);
            }
            return BadRequest("Invalid login information was given");
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public User SignUp([FromBody] User user)
        {
            return userProcessor.InsertNewUser(user.UserName, user.EmailAddress, user.Password);
        }

        [HttpGet]
        public ActionResult<User> Get()
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    List<Claim> claims = authService.GetTokenClaims(token).ToList();
                    return userProcessor.GetUserByEmail(claims.FirstOrDefault(t => t.Type.Equals(ClaimTypes.Email)).Value);
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpGet("{email}")]
        public ActionResult<User> Get(string email)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    List<Claim> claims = authService.GetTokenClaims(token).ToList();
                    if ((claims.FirstOrDefault(t => t.Type.Equals(ClaimTypes.Role)).Value == "Admin"))
                        return userProcessor.GetUserByEmail(email);
                    return BadRequest("Unauthorized Access");
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        [HttpPost]
        public User Post([FromBody] User user)
        {
            return userProcessor.InsertNewUser(user.UserName, user.EmailAddress, user.Password);
        }

        [HttpPost("insertapi")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> InsertApi([FromBody] User user)
        {
            IAuthService authService = new JWTService(clientSettings.Value.SecretKey);
            string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                if (!authService.IsTokenValid(token))
                {
                    return BadRequest("Unauthorized Access");
                } else
                {
                    List<Claim> claims = authService.GetTokenClaims(token).ToList();
                    if ((claims.FirstOrDefault(t => t.Type.Equals(ClaimTypes.Email)).Value == user.EmailAddress) && userProcessor.InsertApiKey(user.EmailAddress, user.ApiKey))
                        return user;
                    return BadRequest("Not valid information");
                }
            } catch
            {
                return BadRequest("Unauthorized Access");
            }
        }

        #region Private Methods
        private static JWTContainerModel GetJWTContainerModel(string username, string email, string role)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }
            };
        }
        #endregion
    }
}
