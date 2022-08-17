using Capa_Entidada;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public class SDB_Producto : IDB_Producto
    {
        public async Task<DataTable> BD_Buscar_Producto(int Id_Pro, string con)
        {

            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new SqlDataAdapter("Sp_Buscar_Producto", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Id_Pro", Id_Pro);

                await cn.OpenAsync();

                DataTable data = new DataTable();
                da.Fill(data);
                da = null;
                cn.Close();

                return data;

            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return null;
            }
        }

        public async Task<bool> BD_Editar_Producto(Producto producto, string con)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = con;
                SqlCommand cmd = new SqlCommand("Sp_Editar_Producto", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@idPro", producto.Id);
                cmd.Parameters.AddWithValue("@idUsu", producto.Id_Usuario);
                cmd.Parameters.AddWithValue("@usuario", producto.Usuario);
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@precio", producto.Precio);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.Parameters.AddWithValue("@proveedor", producto.Proveedor);
                cmd.Parameters.AddWithValue("@estado", "Activo");

                await cn.OpenAsync();
                await cmd.ExecuteScalarAsync();

                cn.Close();

                return false;
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return true;
            }
        }

        public async Task<bool> BD_Eliminar_Producto(int Id_Pro, string con)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new SqlCommand("Sp_Eliminar_Producto", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idpro", Id_Pro);

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                cn.Close();

                return false;
            }
            catch (Exception)
            {

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return true;
            }
        }

        public async Task<int> BD_Registrar_Producto(Producto producto, string con)
        {
            SqlConnection cn = new SqlConnection();

            int respuesta;

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new SqlCommand("Sp_Add_Producto", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idUsu", producto.Id_Usuario);
                cmd.Parameters.AddWithValue("@usuario", producto.Usuario);
                cmd.Parameters.AddWithValue("@nombre", producto.Proveedor);
                cmd.Parameters.AddWithValue("@precio", producto.Precio);
                cmd.Parameters.AddWithValue("@stock", producto.Stock);
                cmd.Parameters.AddWithValue("@proveedor", producto.Proveedor);
                cmd.Parameters.AddWithValue("@estado", "Activo");

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(cmd.ExecuteScalar());

                if (getValue > 0)
                {
                    respuesta = getValue;
                }
                else
                {
                    respuesta = 0;
                }

                cn.Close();

                return respuesta;
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return 0;
            }
        }

        public async Task<DataTable> DB_Mostar_Todas_Productos(string con)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new SqlDataAdapter("Sp_Listar_Todos_Productos", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                await cn.OpenAsync();
                DataTable data = new DataTable();
                da.Fill(data);
                da = null;

                return data;

            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return null;
            }
        }

        public async Task<DataTable> DB_Mostar_Todas_Productos_Usuario(int Id_Usu,string con)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new SqlDataAdapter("Sp_Listar_Todos_Productos_Usuario", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idUsu", Id_Usu);

                await cn.OpenAsync();

                DataTable data = new DataTable();
                da.Fill(data);
                da = null;
                cn.Close();
                return data;

            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return null;
            }
        }
    }
}
