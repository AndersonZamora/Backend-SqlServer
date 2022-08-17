using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public interface ICreateHash
    {
        string PasswordDecrypt(string password, string code);
        string CreatePasswordEncrypt(string password, string code);
    }
}
