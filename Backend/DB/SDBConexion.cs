using Capa_Entidada;

namespace Backend.DB
{
    public class SDBConexion : IDBConexion
    {
        private readonly IConfiguration mConfiguration;
        public SDBConexion(IConfiguration mConfiguration)
        {
            this.mConfiguration = mConfiguration;
        }

        public string Conectar()
        {
            var cadena = mConfiguration.GetSection("AppSettings:ConnectionStrings").Get<Cadena>();

            return cadena.DevConnection;
        }
    }
}
