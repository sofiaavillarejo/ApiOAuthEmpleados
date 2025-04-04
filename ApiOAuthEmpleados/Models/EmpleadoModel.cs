using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiOAuthEmpleados.Models
{
    public class EmpleadoModel
    {
        //MODEL PARA USAR EN EL TOKEN SIN QUE APAREZCA EL SALARIO, POR SEGURIDAD -> ES LA CLASE PARA LOS TOKENS
        public int IdEmpleado { get; set; }
        public string Apellido { get; set; }
        public string Oficio { get; set; }
        public int IdDepartamento { get; set; }
    }
}
