using Capa_Datos;
using Capa_Entidada;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class SNR_Producto : INR_Producto
    {
        readonly IDB_Producto mProducto;
        public SNR_Producto(IDB_Producto mProducto)
        {
            this.mProducto = mProducto;
        }

        public async Task<Producto> NR_Buscar_Producto(int Id_Pro, string con)
        {
            DataTable data = await mProducto.BD_Buscar_Producto(Id_Pro, con);

            if (data.Rows.Count > 0)
            {
                Producto usuario = new Producto()
                {
                    Id = Convert.ToInt16(data.Rows[0]["Id_Pro"]),
                    Id_Usuario = Convert.ToInt16(data.Rows[0]["Id_Usu"].ToString()),
                    Usuario = data.Rows[0]["Usuario"].ToString(),
                    Nombre = data.Rows[0]["Nombre"].ToString(),
                    Precio = Convert.ToDouble(data.Rows[0]["Precio"]),
                    Stock = Convert.ToDouble(data.Rows[0]["Stock"]),
                    Proveedor = data.Rows[0]["Proveedor"].ToString(),
                    Estado = data.Rows[0]["Estado"].ToString()
                };

                return usuario;
            }

            return null;
        }

        public async Task<object> NR_Editar_Producto(Producto producto, string con)
        {
            var resp = await mProducto.BD_Editar_Producto(producto, con);
            if (resp) return new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" };

            return null;
        }
        
        public async Task<object> NR_Eliminar_Producto(int Id_Pro, string con)
        {
            var resp = await mProducto.BD_Eliminar_Producto(Id_Pro,con);

            if (resp) return new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod8" };

            return null;
        }

        public async Task<List<Producto>> NR_Mostar_Todas_Productos(string con)
        {
            DataTable productos = await mProducto.DB_Mostar_Todas_Productos(con);

            var evaluar = EvaluarLista(productos);

            return evaluar;
        }

        public async Task<List<Producto>> NR_Mostar_Todas_Productos_Usuario(int Id_Usu, string con)
        {
            DataTable productos = await mProducto.DB_Mostar_Todas_Productos_Usuario(Id_Usu, con);

            var evaluar = EvaluarLista(productos);

            return evaluar;
        }

        public async Task<int> NR_Registrar_Producto(Producto producto, string con)
        {
            var resp = await mProducto.BD_Registrar_Producto(producto, con);
            return resp;
        }

        private List<Producto> EvaluarLista(DataTable productos)
        {
            List<Producto> list = new List<Producto>();

            try
            {
                if (productos.Rows.Count > 0)
                {
                    foreach (DataRow row in productos.Rows)
                    {
                        Producto prod = new Producto()
                        {
                            Id = int.Parse(row["Id_Pro"].ToString()),
                            Id_Usuario = int.Parse(row["Id_Usu"].ToString()),
                            Usuario = row["Usuario"].ToString(),
                            Nombre = row["Nombre"].ToString(),
                            Precio = double.Parse(row["Precio"].ToString()),
                            Stock = double.Parse(row["Stock"].ToString()),
                            Proveedor = row["Proveedor"].ToString(),
                            Estado = row["Estado"].ToString()
                        };

                        list.Add(prod);
                    }
                    return list;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
