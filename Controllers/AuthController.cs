using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using calculadora_api.Services;
using calculadora_api.Repositories;


namespace calculadora_api.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller 
    {

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            // Recupera o usuário do banco
            var user = UserRepository.Get(model.Name);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário inválido" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);


            // Retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }
    }
}