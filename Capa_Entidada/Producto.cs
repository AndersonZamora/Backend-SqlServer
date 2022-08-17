using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidada
{
    public class Producto
    {
        public int Id{ get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public double Stock { get; set; }
        public string Proveedor { get; set; }
        public string Estado { get; set; }
        public int Id_Usuario { get; set; }
        public string Usuario { get; set; }
    }
}
