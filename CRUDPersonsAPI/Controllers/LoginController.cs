using CRUDPersonsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDPersonsAPI.Controllers
{
    [ApiController]
    [Route("api/v1/login")]
    public class LoginController: ControllerBase
    {
        private readonly JwtService _jwtService;
        public LoginController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }


        //public Task<IActionResult> Get(string username,string password)
        //{ 

        //        _jwtService.CreateJwtToken()
        //}
    }
}
