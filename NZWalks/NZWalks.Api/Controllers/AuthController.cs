using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]

        public async Task<IActionResult> Loginasync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate incomming request - fluent validator

            //Check if user is authorized
            //Check user name and passward
            var user = 
                await userRepository.AuthenticateAsync(loginRequest.Name, loginRequest.Passward);

            if(user != null)
            {
              //Generate JWT
              var token =  await tokenHandler.CreateTokenAsync(user);

                return Ok(token);
            }

            return BadRequest("user name or passward is incorrect");
        }
    }
}
