using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMS___Business_Layer;
using GMS___Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

        [HttpGet("{email}")]
        public User Get(string email) => userProcessor.GetUserByEmail(email);

        [HttpPost]
        public User Post([FromBody] User user)
        {
            return userProcessor.InsertNewUser(user.UserName,user.EmailAddress,user.Password);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public User LogIn([FromBody] User user)
        {
            return userProcessor.LogInUser(user.EmailAddress, user.Password);
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public User SignUp([FromBody] User user)
        {
            return userProcessor.InsertNewUser(user.UserName, user.EmailAddress, user.Password);
        }

        [HttpPost("insertapi")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<User> InsertApi([FromBody] User user)
        {
            if (userProcessor.InsertApiKey(user.EmailAddress, user.ApiKey))
                return user;
            return BadRequest("Not valid information");
        }
    }
}
