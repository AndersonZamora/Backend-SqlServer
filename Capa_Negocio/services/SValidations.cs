using Capa_Entidada;

namespace Capa_Negocio
{
    public class SValidations : IValidations
    {
        readonly IValidarCampos mValidarCampos;
        public SValidations(IValidarCampos mValidarCampos)
        {
            this.mValidarCampos = mValidarCampos;
        }

        public object ValidarCodigos(Dev dev)
        {
            if (string.IsNullOrEmpty(dev.Code) || string.IsNullOrEmpty(dev.Token)) return new { ok = false, name = "Por favor hable con el administrador" };

            return null;
        }

        public object ValidarLogin(UsuarioL usuario)
        {
            if (mValidarCampos.ValidarEmail(usuario.Email)) return new { ok = false, msg = "Ingrese un correo valido" };
            if (string.IsNullOrEmpty(usuario.Password)) return new { ok = false, msg = "Ingrese un contraseña valida" };
            
            return null;
        }

        public object ValidarProducto(Producto producto)
        {
            if (string.IsNullOrEmpty(producto.Nombre)) return new { ok = false, msg = "El nombre es obligatorio" };
            if (producto.Precio < 0) return new { ok = false, msg = "El precio es obligatorio" };
            if (producto.Stock < 0) return new { ok = false, msg = "El stock es obligatorio" };
            if (string.IsNullOrEmpty(producto.Proveedor)) return new { ok = false, msg = "El Proveedor es obligatorio" };
            if (producto.Stock <= 0) return new { ok = false, msg = "El usuario es obligatorio" };

            return null;
        }

        public object ValidarProductoId(int Id_Producto)
        {
            if (Id_Producto <= 0) return new { ok = false, msg = "Id no valido" };

            return null;
        }

        public object ValidarUsuario(UsuarioR usuario)
        {
            if (string.IsNullOrEmpty(usuario.Name)) return new { ok = false, msg = "El nombre es obligatorio"};
            
            if (usuario.Password.Length < 6) return new { ok = false, msg = "La contraseña debe de ser de 6 caracteres" };

            if (mValidarCampos.ValidarEmail(usuario.Email)) return new { ok = false, msg = "Ingrese un correo valido" };

            return null;
        }
    }
}
