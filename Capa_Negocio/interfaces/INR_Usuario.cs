using Capa_Entidada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public interface INR_Usuario
    {
        Task<object> NR_Search_CorreoS(string email, string con);
        Task<int> NR_Registrar_Usuario(UsuarioR usuario, string con);
        Task<Usuario> NR_Login(string email, string con);
    }
}
