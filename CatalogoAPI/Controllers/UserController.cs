using System.Text;
using System.Security.Claims;
using System.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CatalogoAPI.DTOs;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CatalogoAPI.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        public UserController(UserManager<IdentityUser> userManager, 
                              SignInManager<IdentityUser> signInManager,
                              IConfiguration config) 
        {
            _userManager= userManager;
            _signInManager = signInManager;
            _config = config;
        }

        /// <summary>
        ///     Registra um novo usuário liberando o acesso via Token JWT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody]UsuarioDTO model) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true 
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);
            return Ok(GenerateToken(model));
        }

        /// <summary>
        ///     Efetua o login e libera o JWT Token para acesso as funcionalidades da API
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult> UserLogin([FromBody]UsuarioDTO model)        
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var result = await _signInManager.PasswordSignInAsync(model.Email, 
                                                                  model.Password, 
                                                                  isPersistent: false, 
                                                                  lockoutOnFailure: false);
            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Login Inválido.");
                return BadRequest(ModelState);
            }

            return Ok(GenerateToken(model));
        }

        private UsuarioTokenDTO GenerateToken(UsuarioDTO user)
        {
            //define user declarations
            var claims = new[] {
                new Claim (JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim("CATALOGOAPI", "aclogtao pia mcail"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //generate a key based on a simetric algorithm
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            //generate a digital signature for the token using Hmac & private key
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //token expiration time
            var expiracao = _config["TokenConfiguration:ExpireHours"];
            var tmpExpiracao = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            //JWT token class representation and creation
            JwtSecurityToken token = new JwtSecurityToken (
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims,
                expires: tmpExpiracao,
                signingCredentials: credenciais
            );

            //returns data with token and information
            return new UsuarioTokenDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = tmpExpiracao,
                Message = "JWT Token gerado com sucesso!"
            };
        }
    }
}