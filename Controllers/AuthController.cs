using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using calculadora_api.Services;
using calculadora_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;

namespace calculadora_api.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthController : Controller 
    {

        private readonly ApplicationContext _context;

        private readonly UserRepository _userRepository;

        private readonly AuthenticatedUser _authUser;

        public AuthController(ApplicationContext context, AuthenticatedUser authUser)
        {
            _authUser = authUser;
            _context = context;
            if (_userRepository == null) {
                _userRepository = new UserRepository(context);
            }
        }

        
        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public ActionResult<dynamic> Authenticate()
        {
            if (!_authUser.IsAuthenticated)
                return Unauthorized("Usuário não autenticado");

            // Recupera o nome do usuário logado no Windows
            string usuarioLogado = _authUser.Name();
            if (usuarioLogado == null)
                return NotFound(new { message = "Usuário autenticado não encontrado" });

            Console.WriteLine(usuarioLogado);

            // Recupera o usuário do banco
            var user = _userRepository.Get(usuarioLogado);

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário inválido" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Retorna os dados
            return new ObjectResult( new {
                user, token
            });
        }
    }
}