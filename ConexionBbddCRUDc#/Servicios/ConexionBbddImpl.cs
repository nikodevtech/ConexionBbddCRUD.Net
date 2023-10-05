using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionBbddCRUDc_.Util;
using System.IO;

namespace ConexionBbddCRUDc_.Servicios
{
    /// <summary>
    /// Clase que implementa la interfaz que establece y configura la conexión a la BBDD definiendo el detalle
    /// </summary>
    internal class ConexionBbddImpl : ConexionBbddInterface
    {
        public NpgsqlConnection GenerarConexion()
        {
            //Se lee la cadena de conexión a Postgresql del archivo de configuración
            string urlConexion = ConfigurationManager.ConnectionStrings["MiConexionBD"].ConnectionString;

            NpgsqlConnection conexion = null;
            string estado = "";
            StreamWriter sW = File.AppendText("./ficheroLog.txt");


            if (!string.IsNullOrWhiteSpace(urlConexion))
            {
                try
                {
                    conexion = new NpgsqlConnection(urlConexion);
                    conexion.Open();
                    //Se obtiene el estado de conexión para informarlo por consola
                    estado = conexion.State.ToString();
                    if (estado.Equals("Open"))
                    {
                        sW.WriteLine("\n\t** INFO ConexionBbddImpl configuracionConexion ** Estado conexión: " + estado);
                    }
                    else
                    {
                        conexion = null;
                        sW.WriteLine("\n\t** INFO ConexionBbddImpl configuracionConexion ** Estado conexión: " + estado);
                    }
                }
                catch (Exception e)
                {
                    sW.WriteLine("\n\t** ERROR ConexionBbddImpl configuracionConexion ** Error al establecer la conexion." + e);
                    conexion = null;
                    return conexion;
                }
                finally
                {
                    sW.Close();
                }
            }
            return conexion;
        }
    }
}
