using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using calculadora_api.Services;
using calculadora_api.Repositories;

namespace calculadora_api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] User model)
        {
            // var user = UserRepository.VerifyUser(model.Username, model.Password);

            // if (user == null)
            //     return NotFound(new { message = "Usuário ou senha inválidos" });

            // var token = TokenService.GenerateToken(user);
            // user.Password = null;

            // return new
            // {
            //     user = user,
            //     token = token
            // };
            return null;
        }
    }
}