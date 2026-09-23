using System.Security.Claims;
using Atividade_SAEP_3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atividade_SAEP_3.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Consultas");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Busca o paciente pelo CPF
            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(p => p.CPF == model.Cpf);

            if (paciente == null)
            {
                ModelState.AddModelError("", "CPF não encontrado. Faça seu cadastro primeiro.");
                return View(model);
            }

            // Criar a identidade (Claims) do usuário logado
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, paciente.Id.ToString()),
                new Claim(ClaimTypes.Name, paciente.Nome),
                new Claim("CPF", paciente.CPF)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Consulta");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}