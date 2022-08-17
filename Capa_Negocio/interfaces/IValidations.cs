using Capa_Entidada;

namespace Capa_Negocio
{
    public interface IValidations
    {
        object ValidarCodigos(Dev dev);
        object ValidarUsuario(UsuarioR usuario);
        object ValidarLogin(UsuarioL usuario);
        object ValidarProducto(Producto producto);
        object ValidarProductoId(int Id_Producto);
    }
}
