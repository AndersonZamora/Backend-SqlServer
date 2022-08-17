using Capa_Entidada;
using System.Security.Claims;

namespace Capa_Negocio
{
    public interface ITokenCreate
    {
        string TokenCreate(int id, string nombre, string code);
        ClaimsIdent ValidarToken(string token, string code, ClaimsIdentity identity);
        ClaimsIdent GetUser(ClaimsIdentity identity);
    }
}
