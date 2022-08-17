using Backend.DB;
using Capa_Entidada;
using Capa_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        readonly INR_Usuario mUsuario;
        readonly IDBConexion dBConexion;
        readonly IConfiguration mConfiguration;
        readonly ICreateHash mCreateHash;
        readonly ITokenCreate mTokenCreate;
        readonly IValidations mValidations;
        public AuthController(INR_Usuario mUsuario, IDBConexion dBConexion, IConfiguration mConfiguration, ICreateHash mCreateHash, ITokenCreate mTokenCreate, IValidations mValidations)
        {
            this.mUsuario = mUsuario;
            this.dBConexion = dBConexion;
            this.mConfiguration = mConfiguration;
            this.mCreateHash = mCreateHash;
            this.mTokenCreate = mTokenCreate;
            this.mValidations = mValidations;
        }

        [HttpPost("/auth/new")]
        public async Task<ActionResult<UsuarioR>> Register(UsuarioR request)
        {
            try
            {
                /** cadena de conexión a la base de datos y codigo de token **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** validar usuario **/
                var validar = mValidations.ValidarUsuario(request);
                if (validar != null) return StatusCode(500, validar);

                /** validar que en la base de datos no exista el correo a registrar **/
                var respuesta = await mUsuario.NR_Search_CorreoS(request.Email, con);
                if (respuesta != null) return StatusCode(500, respuesta);

                /** código de encriptación **/
                var code = mConfiguration.GetSection("AppSettings:Dev").Get<Dev>();

                /** encriptación de contraseña **/
                string hsp = mCreateHash.CreatePasswordEncrypt(request.Password, code.Code);

                /** registrar usuario en la base de datos **/
                request.Password = hsp;
                var uid = await mUsuario.NR_Registrar_Usuario(request, con);
                if(uid <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod2" });

                /** crear token **/
                var token = mTokenCreate.TokenCreate(uid,request.Name, code.Token);

                /** retornamos un nuevo objeto con la respuesta **/
                return StatusCode(201, new { ok = true, uid, name = request.Name, token });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod3" });
            }
        }
        [HttpPost("/auth")]
        public async Task<ActionResult<UsuarioL>> Login(UsuarioL request)
        {
            try
            {
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** validar login **/
                var validar = mValidations.ValidarLogin(request);
                if (validar != null) return StatusCode(500, validar);

                /** consultamos en la base de datos si existe el email **/
                var login = await mUsuario.NR_Login(request.Email, con);
                if (login == null) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                /** código de encriptación **/
                var code = mConfiguration.GetSection("AppSettings:Dev").Get<Dev>();

                /** desencriptar de contraseña **/
                string hsp = mCreateHash.PasswordDecrypt(login.Password, code.Code);

                if(!hsp.Equals(request.Password)) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                /** crear token **/
                var token = mTokenCreate.TokenCreate(login.Id_Usuario,login.Name, code.Token);

                return StatusCode(201, new { ok = true, uid = login.Id_Usuario, name = login.Name, token });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: codLO" });
            }

        }

        [Authorize]
        [HttpPost("/auth/renew")]
        public ActionResult Renew(string token)
        {
            try
            {
                /** código de encriptación **/
                var code = mConfiguration.GetSection("AppSettings:Dev").Get<Dev>();

                /** validar token y generar uno nuevo**/
                var renew = mTokenCreate.ValidarToken(token, code.Token, HttpContext.User.Identity as ClaimsIdentity);
                if(renew == null) return StatusCode(500, new { ok = false, msg = "Token no valido" });

                return StatusCode(201, new { ok = true, token = renew.Token });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Token no valido" });
            }

        }
    }
}
