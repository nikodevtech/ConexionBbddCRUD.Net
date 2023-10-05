using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Servicios
{
    /// <summary>
    /// Interface que configura, genera y establece conexión a BBDD con la configuración dada en App.config
    /// </summary>
    internal interface ConexionBbddInterface
    {
        /// <summary>
        /// Establece un canal de comunicación con BBDD
        /// </summary>
        /// <returns></returns>
        NpgsqlConnection GenerarConexion();

    }
}
