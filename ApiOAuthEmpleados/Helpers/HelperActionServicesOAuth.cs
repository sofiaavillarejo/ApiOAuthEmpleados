﻿using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiOAuthEmpleados.Helpers
{
    public class HelperActionServicesOAuth
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public HelperActionServicesOAuth(IConfiguration configuration)
        {
            this.Issuer = configuration.GetValue<string>("ApiOAuthToken:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuthToken:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuthToken:SecretKey");
        }

        //NECESITAMOS UN METODO PARA GENERAR EL TOKEN -> SE BASA EN NUESTRO SECRETKEY
        public SymmetricSecurityKey GetKeyToken()
        {
            //CONVERTIMOS EL SECRET KEY A BYTES
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            //DEVOLVEMOS LA KEY GENERADA A PARTIR DE LOS BYTES
            return new SymmetricSecurityKey(data);
        }

        //ESTA CLASE LA HEMOS CREADO TAMBIEN PARA QUITAR CÓDIGO DEL PROGRAM
        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //aquí indicamos qué debemos validar para el token
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = this.GetKeyToken()
                };
            });
            return options;
        }

        public Action<AuthenticationOptions> GetAuthenticateSchema()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }

}
