using Capa_Entidada;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public interface INR_Producto
    {
        Task<Producto> NR_Buscar_Producto(int Id_Pro, string con);
        Task<object> NR_Editar_Producto(Producto producto, string con);
        Task<object> NR_Eliminar_Producto(int Id_Pro, string con);
        Task<List<Producto>> NR_Mostar_Todas_Productos(string con);
        Task<List<Producto>> NR_Mostar_Todas_Productos_Usuario(int Id_Usu, string con);
        Task<int> NR_Registrar_Producto(Producto producto, string con);
    }
}
