using Capa_Entidada;
using System.Data;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public interface IBD_Usuario
    {
        Task<bool> BD_Search_Correo(string email, string con);
        Task<int> BD_Registrar_Usuario(UsuarioR usuario, string con);
        Task<DataTable> DB_Login(string email, string con);
    }
}
