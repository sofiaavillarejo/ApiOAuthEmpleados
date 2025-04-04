using System.Security.Claims;
using ApiOAuthEmpleados.Models;
using Newtonsoft.Json;

namespace ApiOAuthEmpleados.Helpers
{
    public class HelperEmpleadoToken
    {
        private IHttpContextAccessor contextAccessor;
        public HelperEmpleadoToken(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public EmpleadoModel GetEmpleado()//nos devuelve el empleado q esta dentro de la seguridad
        {
            Claim claim = this.contextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserData");
            //extraemos el json q tenemos almacenado
            string json = claim.Value; //-> EmpleadoModel
            string jsonEmpleado = HelperCryptography.DecryptString(json);
            EmpleadoModel model = JsonConvert.DeserializeObject<EmpleadoModel>(jsonEmpleado);
            return model;
        }

    }
}
