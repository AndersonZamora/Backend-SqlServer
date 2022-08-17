using Capa_Entidada;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public class SBD_Usuario : IBD_Usuario
    {
        public async Task<bool> BD_Search_Correo(string email, string con)
        {
            SqlConnection cn = new SqlConnection();

            bool respuesta = false;

            try
            {

                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = con;
                cmd.CommandText = "Sp_Validar_Correo";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@email ", email);

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(cmd.ExecuteScalar());

                if (getValue > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }

                cmd.Parameters.Clear();
                cmd.Dispose();

                cmd = null;
                cn.Close();

                return respuesta;
            }
            catch (Exception)
            {

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return respuesta;
            }
        }
        public async Task<int> BD_Registrar_Usuario(UsuarioR usuario, string con)
        {
            SqlConnection cn = new SqlConnection();

            int respuesta;

            try
            {

                SqlCommand cmd = new SqlCommand();
                cn.ConnectionString = con;
                cmd.CommandText = "Sp_Add_Usuario";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@name", usuario.Name.ToString());
                cmd.Parameters.AddWithValue("@password", usuario.Password);
                cmd.Parameters.AddWithValue("@email", usuario.Email.ToString());


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

                cmd.Parameters.Clear();
                cmd.Dispose();

                cmd = null;
                cn.Close();
            }
            catch (Exception)
            {

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return 0;
            }

            return respuesta;
        }
        public async Task<DataTable> DB_Login(string email, string con)
        {
            SqlConnection cn = new SqlConnection();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new SqlDataAdapter("Sp_Usuario_Login", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Email", email);

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
