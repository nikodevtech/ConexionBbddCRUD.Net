using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexionBbddCRUDc_.Dto;
using ConexionBbddCRUDc_.Servicios;
using ConexionBbddCRUDc_.Util;
using Npgsql;

namespace ConexionBbddCRUDc_
{
    /// <summary>
    /// Clase por la que inicia la aplicación
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            bool cerrarMenu = false;
            int opcion;

            ConexionBbddInterface conexionBbdd = new ConexionBbddImpl();
            OperacionesBbddInterface operacionBbdd = new OperacionesBbddImpl();
            List<LibroDto> listaLibros = new List<LibroDto>();
            NpgsqlConnection conexion = null;

            do
            {
                try
                {
                    conexion = conexionBbdd.GenerarConexion();

                    if (conexion != null)
                    {
                        opcion = Utilidades.MostrarMenu();

                        switch (opcion)
                        {

                            case 1:
                                operacionBbdd.CreateLibro(conexion, listaLibros);
                                break;
                            case 2:
                                operacionBbdd.ReadLibro(conexion, listaLibros);
                                break;
                            case 3:
                                operacionBbdd.UpdateLibro(conexion, listaLibros);
                                break;
                            case 4:
                                operacionBbdd.DeleteLibro(conexion, listaLibros);
                                break;
                            case 0:
                                cerrarMenu = true;
                                break;
                        }
                    }

                }
                catch (Exception e)
                {
                    StreamWriter sW = File.AppendText("./ficheroLog.txt");
                    sW.WriteLine("\n\t** ERROR ocurrió una excepción no esperada ** " + e.Message);
                    sW.Close();
                    Console.WriteLine("\n\t** ERROR ocurrió una excepción no esperada ** " + e.Message);

                }

            } while (!cerrarMenu);
            Utilidades.Pausa("salir de la aplicación");

        }
    }
}
