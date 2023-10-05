using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Util
{
    /// <summary>
    /// Clase Util que contiene varios métodos de utilidades y static que no pertenecen a ninguna clase
    /// </summary>
    internal class Utilidades
    {
        /// <summary>
        /// Método encargado de mostrar al usuario un menú por consola con las opciónes disponibles 
        /// </summary>
        /// <returns>Un número entero que es la opción seleccionada en el menú</returns>
        public static int MostrarMenu()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("\n\n\n\n\t\t\tAlumno: Nicolás Álvarez Zapata");
                Console.WriteLine("\t\t\t╔══════════════════════════════╗");
                Console.WriteLine("\t\t\t║             MENU             ║");
                Console.WriteLine("\t\t\t╠══════════════════════════════╣");
                Console.WriteLine("\t\t\t║                              ║");
                Console.WriteLine("\t\t\t║    1.- Registrar libro       ║");
                Console.WriteLine("\t\t\t║    2.- Mostrar libro         ║");
                Console.WriteLine("\t\t\t║    3.- Modificar libro       ║");
                Console.WriteLine("\t\t\t║    4.- Eliminar libro        ║");
                Console.WriteLine("\t\t\t║                              ║");
                Console.WriteLine("\t\t\t║          0) Salir            ║");
                Console.WriteLine("\t\t\t║                              ║");
                Console.WriteLine("\t\t\t╚══════════════════════════════╝");
                Console.Write("\t\t\tIntroduce la opción deseada: ");
                opcion = Console.ReadKey(true).KeyChar - '0';

                if (opcion < 0 || opcion > 4)
                {
                    Console.WriteLine("\n\t** Error la opcion introducida no se encuentra entre 0 y 4 **");
                    Utilidades.Pausa("continuar");
                }

            } while (opcion < 0 || opcion > 4);

            return opcion;
        }
        /// <summary>
        /// Método que pide un número entero al usuario y controla que no tenga error con el tipo de dato introducido
        /// </summary>
        /// <param name="mensaje">Texto informativo para mostrar al usuario</param>
        /// <param name="max">Número entero máximo que puede introducir</param>
        /// <param name="min">Número entero mínimo que puede introducir</param>
        /// <returns>El número entero sin errores introducido</returns>
        public static int CapturaEntero(string mensaje, int min, int max)
        {
            int valor = 0;
            bool esCorrecto = false;
            do
            {
                Console.Write("\n\t{0} [{1}..{2}]: ", mensaje, min, max);
                esCorrecto = Int32.TryParse(Console.ReadLine(), out valor);
                if (!esCorrecto)
                    Console.WriteLine("\n\t** Error, debe introducir un número entero  **");
                else if (valor < min || valor > max)
                    Console.WriteLine("\n\t** Error, el número introducido no se encuentra en el rango disponible **");

            }
            while (!esCorrecto || valor < min || valor > max);

            return valor;
        }

        /// <summary>
        /// Método que pide un al usuario confirmar una acción realizada con S o N
        /// </summary>
        /// <param name="pregunta">Texto informativo para el user</param>
        /// <returns>True si confirma la acción y false en caso contrario</returns>
        public static bool PreguntaSiNo(string pregunta)
        {
            char respuesta;
            do
            {
                Console.Write("\n\t{0}?: ", pregunta);
                respuesta = Console.ReadKey().KeyChar;

                if (respuesta != 'S' && respuesta != 's' && respuesta != 'N' && respuesta != 'n')
                    Console.WriteLine("\n\n\t** Error, no es válida la tecla pulsada **");

            } while (respuesta != 'S' && respuesta != 's' && respuesta != 'N' && respuesta != 'n');

            if (respuesta == 'S' || respuesta == 's')
                return true;
            else
                return false;

        }

        /// <summary>
        /// Método que permite realizar pausa en la consola evitando que borre lo que se muestra o que la cierre sin previo aviso
        /// </summary>
        /// <param name="mensaje">Texto informativo para el user</param>
        public static void Pausa(string mensaje)
        {
            Console.Write("\n\n\tPulse una tecla para {0}", mensaje);
            Console.ReadKey(true);
        }
    }
}
