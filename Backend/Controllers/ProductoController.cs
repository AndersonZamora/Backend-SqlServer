using Backend.DB;
using Capa_Entidada;
using Capa_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : Controller
    {
        readonly INR_Producto mProducto;
        readonly IDBConexion dBConexion;
        readonly IValidations mValidations;
        readonly ITokenCreate mTokenCreate;
        public ProductoController(INR_Producto mProducto, IDBConexion dBConexion, IValidations mValidations, ITokenCreate mTokenCreate)
        {
            this.mProducto = mProducto;
            this.dBConexion = dBConexion;
            this.mValidations = mValidations;
            this.mTokenCreate = mTokenCreate;
        }

        [HttpGet("/producto")]
        public async Task<ActionResult<Producto>> GetAllProductos()
        {
            try
            {
                /** cadena de conexión a la base de datos y codigo de token **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                var list = await mProducto.NR_Mostar_Todas_Productos(con);
                if(list == null) return StatusCode(500, new { ok = false, msg = "sin productos" });

                return StatusCode(201, new { ok = true, Productos = list });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod9" });
            }
        }

        [HttpGet("/producto/usuario")]
        public async Task<ActionResult<Producto>> GetAllProductosUsuario()
        {
            try
            {
                /** obtener id del usuario **/
                ClaimsIdent user = mTokenCreate.GetUser(HttpContext.User.Identity as ClaimsIdentity);
                if(user.Id_Usuario <= 0) StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod10" });

                /** cadena de conexión a la base de datos y codigo de token  **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** cargamos todos los productos  **/
                var list = await mProducto.NR_Mostar_Todas_Productos_Usuario(user.Id_Usuario, con);
                if (list == null) return StatusCode(500, new { ok = false, msg = "sin productos" });

                return StatusCode(201, new { ok = true, Productos = list });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod9" });
            }
        }

        [HttpPost("/producto")]
        public async Task<ActionResult<Producto>> Create([FromBody] Producto Request)
        {
            try
            {
                /** cadena de conexión a la base de datos y codigo de token **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** obtener id del usuario **/
                ClaimsIdent user = mTokenCreate.GetUser(HttpContext.User.Identity as ClaimsIdentity);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod10" });

                Request.Id_Usuario = user.Id_Usuario;
                Request.Usuario = user.Name;
                Request.Estado = "Activo";
                /** validar producto **/
                var validar = mValidations.ValidarProducto(Request);
                if (validar != null) return StatusCode(500, validar);

                /** registrar producto **/
                int lastlast_insert = await mProducto.NR_Registrar_Producto(Request, con);
                if (lastlast_insert <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod11" });

                Request.Id = lastlast_insert;

                return StatusCode(201, new { ok = true, productoGuardado = Request } );
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has vOnitCr");
            }
        }

        [HttpPut("/producto")]
        public async Task<ActionResult<Producto>> Update([FromBody] Producto Request)
        {
            try
            {
                /** cadena de conexión a la base de datos y codigo de token **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** obtener id del usuario **/
                ClaimsIdent user = mTokenCreate.GetUser(HttpContext.User.Identity as ClaimsIdentity);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod10" });

                /** validar producto **/
                var validar = mValidations.ValidarProducto(Request);
                if (validar != null) return StatusCode(500, validar);
                var val = mValidations.ValidarProductoId(Request.Id);
                if (val != null) return StatusCode(500, val);

                /** buscamos el producto **/
                Producto DbProducto = await mProducto.NR_Buscar_Producto(Request.Id, con);
                if(DbProducto == null) return StatusCode(500, new { ok = false,  Request.Id, msg = "No existe un producto con ese id" });
                
                /** validamos que el usuario a modificar se el mismo que lo creo **/
                if (DbProducto.Id_Usuario != user.Id_Usuario) return StatusCode(500, new { ok = false, Request.Id, msg = "No tiene permisos para modificar este producto" });

                Request.Id_Usuario = user.Id_Usuario;
                Request.Usuario = user.Name;
                Request.Estado = "Activo";
                /** editamos el producto en la base de datos **/
                var up = await mProducto.NR_Editar_Producto(Request, con);
                if(up != null ) return StatusCode(500, up);

                return StatusCode(201, new { ok = true, productoActualizado = Request });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has vOnitCr");
            }
        }

        [HttpDelete("/producto")]
        public async Task<ActionResult<Producto>> Delete(int Id)
        {
            try
            {
                /** cadena de conexión a la base de datos y codigo de token **/
                var con = dBConexion.Conectar();
                if (con == null) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod1" });

                /** obtener id del usuario **/
                ClaimsIdent user = mTokenCreate.GetUser(HttpContext.User.Identity as ClaimsIdentity);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod10" });

                var val = mValidations.ValidarProductoId(Id);
                if (val != null) return StatusCode(500, val);

                /** buscamos el producto **/
                Producto DbProducto = await mProducto.NR_Buscar_Producto(Id, con);
                if (DbProducto == null) return StatusCode(500, new { ok = false, Id, msg = "No existe un producto con ese id" });

                /** validamos que el usuario a modificar se el mismo que lo creo **/
                if (DbProducto.Id_Usuario != user.Id_Usuario) return StatusCode(500, new { ok = false, Id, msg = "No tiene permisos para modificar este producto" });

                /** eliminamos el producto en la base de datos **/
                var del = await mProducto.NR_Eliminar_Producto(Id, con);
                if (del != null) return StatusCode(500, del);

                return StatusCode(201, new { ok = true });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has vOnitDele");
            }
        }
    }
}
