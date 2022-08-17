using Capa_Entidada;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public interface IDB_Producto
    {
        Task<DataTable> BD_Buscar_Producto(int Id_Pro, string con);
        Task<bool> BD_Editar_Producto(Producto producto, string con);
        Task<bool> BD_Eliminar_Producto(int Id_Pro, string con);
        Task<DataTable> DB_Mostar_Todas_Productos(string con);
        Task<DataTable> DB_Mostar_Todas_Productos_Usuario(int Id_Usu, string con);
        Task<int> BD_Registrar_Producto(Producto producto, string con);
    }
}
