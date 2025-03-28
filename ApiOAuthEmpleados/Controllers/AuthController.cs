using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiOAuthEmpleados.Helpers;
using ApiOAuthEmpleados.Models;
using ApiOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ApiOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryHospital repo;
        //CONTROLADOR QUE NOS VA A PERMITIR ACCEDER AL TOKEN 
        //CUANDO GENEREMOS EL TOKEN, DEBEMOS INTEGRAR ALGUNOS DATOS COMO ISSUER Y DEMAS
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryHospital repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Empleado emp = await this.repo.LogInEmpleadoAsync(model.User, int.Parse(model.Password));
            if (emp == null)
            {
                return Unauthorized();
            }else
            {
                //DEBEMOS CREAR UNAS CREDENCIALES PARA INCLUIRLAS DENTRO DEL TOKEN Y QUE ESTARAN COMPUESTAS POR EL SECRET KEY CIFRADO Y EL TIPO DE CIFRADO QUE INCLUIREMOS EN EL TOKEN
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonEmpleado = JsonConvert.SerializeObject(emp);
                //CREAMOS UN ARRAY DE CLAIMS
                Claim[] info = new[]
                {
                    new Claim("UserData", jsonEmpleado)
                }; 
                //EL TOKEN SE GENERA CON UNA CLASE Y DEBEMOS INDICAR LOS DATOS QUE ALMACENARA EN SU INTERIOR
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: info,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                );

                //devolvemos la respuesta afirmativa con un objto que contenga el token (anonimo)
                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
    }
}
