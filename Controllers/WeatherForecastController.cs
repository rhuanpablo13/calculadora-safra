using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

using System.Security;
using calculadora_api.Models;
using System.Text.Json;
using System.Security.Claims;

namespace calculadora_safra.Controllers
{
    
    [ApiController]
    [Route("teste")]
    public class WeatherForecastController : ControllerBase
    {
       
        private readonly AuthenticatedUser _user;

        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public WeatherForecastController(IHttpContextAccessor httpContextAccessor, AuthenticatedUser user)
        {
            _httpContextAccessor = httpContextAccessor;
            _user = user;
        }

        [HttpGet]
        public ActionResult<WeatherForecast> Get()
        {
            // var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // Console.WriteLine("--------------------------");
            // Console.WriteLine("userId: " + userId);
            // Console.WriteLine("Nome: " + _user.Name);
            // Console.WriteLine("Email: " + _user.Email);
            // Console.WriteLine(_user.GetClaimsIdentity().Count());

            

            return Ok();
        }
    }
}
