using Capa_Datos;
using Capa_Entidada;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class SNR_Usuario : INR_Usuario
    {
        readonly IBD_Usuario mUsuario;
        public SNR_Usuario(IBD_Usuario mUsuario)
        {
            this.mUsuario = mUsuario;
        }

        public async Task<object> NR_Search_CorreoS(string email, string con)
        {
            var resp = await mUsuario.BD_Search_Correo(email, con); //debe retornar falso

            if (resp) return new { ok = false, msg = "El correo ya esta registrado" };
            
            return null;
        }
        
        public async Task<int> NR_Registrar_Usuario(UsuarioR usuario, string con)
        {
            var resp = await mUsuario.BD_Registrar_Usuario(usuario, con); // tiene que devolver mayor que cero

            return resp;
        }

        public async Task<Usuario> NR_Login(string email, string con)
        {

            DataTable data = await mUsuario.DB_Login(email, con);

            if (data.Rows.Count > 0)
            {
                Usuario usuario = new Usuario()
                {
                    Id_Usuario = Convert.ToInt16(data.Rows[0]["Id_Usu"]),
                    Name = data.Rows[0]["Name"].ToString(),
                    Password = data.Rows[0]["Password"].ToString()
                };

                return usuario;
            }

            return null;
        }
    }
}
