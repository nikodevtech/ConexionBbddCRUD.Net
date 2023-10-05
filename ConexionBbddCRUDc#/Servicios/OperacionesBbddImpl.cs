using ConexionBbddCRUDc_.Dto;
using ConexionBbddCRUDc_.Util;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Servicios
{
    /// <summary>
    ///  Clase que implementa la interfaz que dan servicio al CRUD para las operaciones con BBDD
    /// </summary>
    internal class OperacionesBbddImpl : OperacionesBbddInterface
    {

        public void ReadLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros)
        {
            NpgsqlCommand declaracionSQL = null;
            NpgsqlDataReader resultadoConsulta = null;
            ADto adto = new ADto();
            StreamWriter sW = File.AppendText("./ficheroLog.txt");

            try
            {
                Console.Clear();
                Console.WriteLine("\n\t------ Mostrando libro/s ------");

                string query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros";
                // Se abre una declaración y se define la query concreta para la declaración
                declaracionSQL = new NpgsqlCommand(query, conexionGenerada);
                // Se ejecuta la consulta y devuelve un DataReader con los datos
                resultadoConsulta = declaracionSQL.ExecuteReader();

                // Llamada a la conversión a LibroDTO y añadido en su lista
                listaLibros = adto.PasarDataReaderALibrosDto(resultadoConsulta);

                // Mostrando los libros al usuario
                if (listaLibros.Count() == 0)
                {
                    Console.WriteLine("\n\t¡¡No hay libros registrados que mostrar!!");
                }
                else
                {
                    bool mostrarTodosLibros = Utilidades.PreguntaSiNo("Quiere que se muestren todos los libros o mostrar por id (S = todos / N = por id)");
                    if (mostrarTodosLibros)
                    {
                        mostarTodosLosLibros(listaLibros);
                    }
                    else
                    {
                        int primerId = int.Parse(listaLibros[0].IdLibro.ToString());
                        int ultimoId = int.Parse(listaLibros[listaLibros.Count() - 1].IdLibro.ToString());
                        int idLibroAmostrar = Utilidades.CapturaEntero("Introduce el id del libro a mostrar: ", primerId, ultimoId);
                        mostrarLibroPorID(listaLibros, idLibroAmostrar);
                    }
                }
                Utilidades.Pausa("volver al menú");
                sW.WriteLine("\n\t** INFO OperacionesBbddImpl ReadLibro** Cierre conexión a bbdd, npgsqlcommand y datareader");

            }
            catch (ArgumentNullException ane)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl ReadLibro** Error referencia null como valor : " + ane.Message);
            }
            catch (IOException ioe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl ReadLibro** Error con el input/output al introducir un valor: " + ioe.Message);
            }
            catch (OverflowException oe)
            {
                sW.WriteLine("** ERROR OperacionesBbddImpl ReadLibro** Error, desbordamiento al realizar conversion/calculo aritmetico  : " + oe.Message);
            }
            finally
            {
                // Liberando los recursos
                sW.Close();
                resultadoConsulta.Close();
                declaracionSQL.Dispose();
                conexionGenerada.Close();
            }


        }
        /// <summary>
        /// Itera la lista con los libros y los muestra todos al usuario
        /// </summary>
        /// <param name="listaLibros"></param>
        private void mostarTodosLosLibros(List<LibroDto> listaLibros)
        {
            foreach (LibroDto libro in listaLibros)
            {
                libro.MostrarDatos();
            }
        }
        /// <summary>
        /// Itera la lista con los libros y muestra al usuario el libro con id que introduzca
        /// </summary>
        /// <param name="listaLibros"></param>
        /// <param name="idLibroAmostrar">Número entero que hace referencia al id del libro que quiere ver</param>
        private void mostrarLibroPorID(List<LibroDto> listaLibros, int idLibroAmostrar)
        {
            bool libroEncontrado = false;
            for (int i = 0; i < listaLibros.Count(); i++)
            {
                if (listaLibros[i].IdLibro == idLibroAmostrar)
                {
                    listaLibros[i].MostrarDatos();
                    libroEncontrado = true;
                }
            }
            if (libroEncontrado == false)
            {
                Console.WriteLine("\n\tNo existe libro con id " + idLibroAmostrar);
            }
        }
        public void CreateLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros)
        {
            List<LibroDto> listaNuevosLibros = new List<LibroDto>();
            bool registrarOtroLibro;
            NpgsqlCommand consultaPreparada = null;
            int registrosInsertados = 0;
            StreamWriter sW = File.AppendText("./ficheroLog.txt");
            try
            {
                // Recopila datos de libros desde el usuario
                do
                {
                    Console.Clear();
                    Console.WriteLine("\n\t------ Registrando nuevo libro ------");
                    Console.Write("\n\tIntroduce el título del nuevo libro a registrar: ");
                    string titulo = Console.ReadLine();
                    Console.Write("\n\tIntroduce el autor del nuevo libro a registrar: ");
                    string autor = Console.ReadLine();
                    Console.Write("\n\tIntroduce el isbn del nuevo libro a registrar: ");
                    string isbn = Console.ReadLine();
                    int edicion = Utilidades.CapturaEntero("Introduce la edición del nuevo libro a registrar: ", 1, 1000);

                    listaNuevosLibros.Add(new LibroDto(titulo, autor, isbn, edicion)); //Agregando en la lista los libros a registrar
                    registrarOtroLibro = Utilidades.PreguntaSiNo("Quiere registrar otro libro más (S = si / N = no)");

                } while (registrarOtroLibro);

                // Verifica que la lista de los nuevos libros a registrar no esté vacía
                if (listaNuevosLibros.Count() != 0)
                {
                    // Definida query para el insert con los valores parametrizados --> @nombreDeCampo
                    string query = "INSERT INTO gbp_almacen.gbp_alm_cat_libros (titulo, autor, isbn, edicion) VALUES (@titulo,@autor,@isbn,@edicion)";
                    // Consulta preparada que recibe por argumento la query antes declarada y la conexión ya abierta
                    consultaPreparada = new NpgsqlCommand(query, conexionGenerada);

                    // Iteramos la lista con los nuevos libros a registrar
                    foreach (LibroDto nuevoLibro in listaNuevosLibros)
                    {
                        consultaPreparada.Parameters.Clear();//Limpia los parametros si los hubiera para establecerlos posteriormente
                        consultaPreparada.Parameters.AddWithValue("@titulo", nuevoLibro.Titulo); // Se establecen los parametros con los valores exactos
                        consultaPreparada.Parameters.AddWithValue("@autor", nuevoLibro.Autor);
                        consultaPreparada.Parameters.AddWithValue("@isbn", nuevoLibro.Isbn);
                        consultaPreparada.Parameters.AddWithValue("@edicion", nuevoLibro.Edicion);

                        registrosInsertados += consultaPreparada.ExecuteNonQuery(); //Ejecuta la consulta a la bbdd y devuelve un int como filas afectadas

                        listaLibros.Add(nuevoLibro); //Se añaden los nuevos a los que ya había registrados

                    }
                    if (registrosInsertados > 0)
                    {
                        Console.WriteLine("\n\n\tSe insertaron {0} libro/s exitosamente en la BBDD!!", registrosInsertados);
                    }
                    else
                    {
                        Console.WriteLine("\n\n\tNo se insertaron libro/s.");
                    }
                }
                Utilidades.Pausa("continuar");
            }
            catch (ArgumentNullException ane)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl CreateLibro ** Error referencia null como valor: " + ane.Message);
            }
            catch (IOException ioe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl CreateLibro ** Error con el input/output al introducir un valor: " + ioe.Message);
            }
            catch (OutOfMemoryException aome)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl CreateLibro ** Error, no hay suficiente memoria para continuar: " + aome.Message);
            }
            catch (OverflowException oe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl CreateLibro ** Error, desbordamiento al realizar conversion/calculo aritmetico  : " + oe.Message);
            }
            catch (ArgumentOutOfRangeException aore)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl CreateLibro ** Error con el rango de valores permitidos: " + aore.Message);
            }
            finally
            {
                sW.Close();
                consultaPreparada.Dispose(); // Liberando recursos
                conexionGenerada.Close(); // Liberando recursos 
            }
        }
        public void UpdateLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros)
        {
            listaLibros = CargaListaLibrosActuales(conexionGenerada);
            Console.Clear();
            mostarTodosLosLibros(listaLibros); // Muestra libros actuales para que pueda identificar cual modificar
            bool libroAmodificarEsValido = false;
            int idLibroAmodificar;
            NpgsqlCommand consultaPreparada = null;
            StreamWriter sW = File.AppendText("./ficheroLog.txt");

            try
            {
                int primerId = int.Parse(listaLibros[0].IdLibro.ToString());
                int ultimoId = int.Parse(listaLibros[listaLibros.Count() - 1].IdLibro.ToString());
                idLibroAmodificar = Utilidades.CapturaEntero("Introduce el id del libro a modificar: ", primerId, ultimoId);
                libroAmodificarEsValido = CompruebaIdLibro(listaLibros, idLibroAmodificar);

                if (libroAmodificarEsValido)
                {
                    Console.Write("\n\tIntroduce el campo del libro que quiere modificar (titulo, autor, isbn o edicion): ");
                    string campoAmodificar = Console.ReadLine();
                    switch (campoAmodificar)
                    {

                        case "titulo":
                            Console.Write("\n\tIntroduce el nuevo título del libro a modificar: ");
                            string nuevoTitulo = Console.ReadLine();
                            string queryModificarTitulo = "UPDATE gbp_almacen.gbp_alm_cat_libros SET titulo = @titulo WHERE id_libro = @id_libro";
                            consultaPreparada = new NpgsqlCommand(queryModificarTitulo, conexionGenerada);
                            consultaPreparada.Parameters.Clear();//Limpia los parametros si los hubiera para establecerlos posteriormente
                            consultaPreparada.Parameters.AddWithValue("@titulo", nuevoTitulo);
                            consultaPreparada.Parameters.AddWithValue("@id_libro", idLibroAmodificar);
                            break;
                        case "autor":
                            Console.Write("\n\tIntroduce el nuevo autor del libro a modificar: ");
                            string nuevoAutor = Console.ReadLine();
                            string queryModificarAutor = "UPDATE gbp_almacen.gbp_alm_cat_libros SET autor = @autor WHERE id_libro = @id_libro";
                            consultaPreparada = new NpgsqlCommand(queryModificarAutor, conexionGenerada);
                            consultaPreparada.Parameters.Clear();//Limpia los parametros si los hubiera para establecerlos posteriormente
                            consultaPreparada.Parameters.AddWithValue("@autor", nuevoAutor);
                            consultaPreparada.Parameters.AddWithValue("@id_libro", idLibroAmodificar);
                            break;
                        case "isbn":
                            Console.Write("\n\tIntroduce el nuevo ISBN del libro a modificar: ");
                            string nuevoISBN = Console.ReadLine();
                            string queryModificarISBN = "UPDATE gbp_almacen.gbp_alm_cat_libros SET isbn = @isbn WHERE id_libro = @id_libro";
                            consultaPreparada = new NpgsqlCommand(queryModificarISBN, conexionGenerada);
                            consultaPreparada.Parameters.Clear();//Limpia los parametros si los hubiera para establecerlos posteriormente
                            consultaPreparada.Parameters.AddWithValue("@isbn", nuevoISBN);
                            consultaPreparada.Parameters.AddWithValue("@id_libro", idLibroAmodificar);
                            break;
                        case "edicion":
                            int nuevaEdicion = Utilidades.CapturaEntero("Introduce la nueva edición del libro a modificar: ", 1, 1000);
                            string queryModificarEdicion = "UPDATE gbp_almacen.gbp_alm_cat_libros SET edicion = @edicion WHERE id_libro = @id_libro";
                            consultaPreparada = new NpgsqlCommand(queryModificarEdicion, conexionGenerada);
                            consultaPreparada.Parameters.Clear();//Limpia los parametros si los hubiera para establecerlos posteriormente
                            consultaPreparada.Parameters.AddWithValue("@edicion", nuevaEdicion);
                            consultaPreparada.Parameters.AddWithValue("@id_libro", idLibroAmodificar);
                            break;
                        default:
                            Console.WriteLine("\n\tNo se reconoce el campo " + campoAmodificar + " introducido");
                            break;
                    }
                    consultaPreparada.ExecuteNonQuery(); //Ejecuta la consulta a la bbdd
                    Console.WriteLine("\n\tCampo {0} modificado correctamente en la bbdd", campoAmodificar);

                }
                else
                {
                    Console.WriteLine("\n\t Libro con id {0} no encontrado en el sistema", idLibroAmodificar);
                }
                Utilidades.Pausa("para volver al menú");

            }
            catch (IOException ioe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl UpdateLibro ** Error con el input/output al introducir un valor: " + ioe.Message);
            }
            catch (OutOfMemoryException aome)
            {
                sW.WriteLine("** ERROR OperacionesBbddImpl UpdateLibro ** Error, no hay suficiente memoria para continuar: " + aome.Message);
            }
            catch (OverflowException oe)
            {
                sW.WriteLine("** ERROR OperacionesBbddImpl UpdateLibro ** Error, desbordamiento al realizar conversion/calculo aritmetico  : " + oe.Message);
            }
            catch (ArgumentNullException ane)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl UpdateLibro ** Error referencia null como valor: " + ane.Message);
            }
            catch (FormatException fe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl UpdateLibro ** Error con el formato con el argumento introducido: " + fe.Message);
            }
            catch (ArgumentOutOfRangeException aore)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl UpdateLibro ** Error con el rango de valores permitidos: " + aore.Message);
            }
            finally
            {
                // Liberando los recursos
                sW.Close();
                conexionGenerada.Close();
                consultaPreparada.Dispose();
            }
        }

        /// <summary>
        /// Comprueba el id de un libro introducido por el usuario existe realmente en la bbdd
        /// </summary>
        /// <param name="listaLibros">lista de los libros actuales registrados</param>
        /// <param name="idLibroAencontrar">id del libro a buscar</param>
        /// <returns>true si existe el libro con el id dado, false en caso contrario</returns>
        private bool CompruebaIdLibro(List<LibroDto> listaLibros, int idLibroAencontrar)
        {
            foreach (LibroDto libro in listaLibros)
            {
                if (libro.IdLibro == idLibroAencontrar)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Método que realiza una consulta en BBDD y carga todos los libros en la lista para poder trabajar con ellos
        /// </summary>
        /// <param name="conexionGenerada">Conexión abierta a bbdd</param>
        /// <returns>lista con los libros actuales registrados en la bbdd</returns>
        private List<LibroDto> CargaListaLibrosActuales(NpgsqlConnection conexionGenerada)
        {
            NpgsqlCommand declaracionSQL = null;
            NpgsqlDataReader resultadoConsulta = null;
            ADto adto = new ADto();
            List<LibroDto> listaLibros = new List<LibroDto>();
            StreamWriter sW = File.AppendText("./ficheroLog.txt");

            try
            {
                // Definida la query para consultar todos los libros registrados
                string query = "SELECT * FROM gbp_almacen.gbp_alm_cat_libros";
                // Se abre una declaración
                declaracionSQL = new NpgsqlCommand(query, conexionGenerada);
                // Se define la consulta de la declaración y se ejecuta
                resultadoConsulta = declaracionSQL.ExecuteReader();

                // Llamada a la conversión a LibroDTO y añadido en su lista
                listaLibros = adto.PasarDataReaderALibrosDto(resultadoConsulta);

                sW.WriteLine("\n\t** INFO OperacionesBbddImpl CargaListaLibrosActuales ** Cierre conexión a bbdd, datareader y npgsqlcommand");

            }
            catch (Exception e)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl cargaListaLibrosActuales** Error generando o ejecutando la declaracionSQL: " + e.Message);
            }
            finally
            {
                // Liberando los recursos
                sW.Close();
                resultadoConsulta.Close();
                declaracionSQL.Dispose();
            }
            return listaLibros;
        }
        public void DeleteLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros)
        {

            listaLibros = CargaListaLibrosActuales(conexionGenerada);
            mostarTodosLosLibros(listaLibros); // Muestra libros actuales para que pueda identificar cual eliminar
            bool libroAeliminarEsValido = false;
            int idLibroAeliminar;
            NpgsqlCommand consultaPreparada = null;
            StreamWriter sW = File.AppendText("./ficheroLog.txt");

            try
            {
                int primerId = int.Parse(listaLibros[0].IdLibro.ToString());
                int ultimoId = int.Parse(listaLibros[listaLibros.Count() - 1].IdLibro.ToString());
                idLibroAeliminar = Utilidades.CapturaEntero("Introduce el id del libro a eliminar: ", primerId, ultimoId);
                libroAeliminarEsValido = CompruebaIdLibro(listaLibros, idLibroAeliminar);

                if (libroAeliminarEsValido)
                {
                    String queryEliminar = "DELETE FROM gbp_almacen.gbp_alm_cat_libros WHERE id_libro = @id_libro";
                    consultaPreparada = new NpgsqlCommand(queryEliminar, conexionGenerada);
                    consultaPreparada.Parameters.Clear();
                    consultaPreparada.Parameters.AddWithValue("@id_libro", idLibroAeliminar);
                    consultaPreparada.ExecuteNonQuery();
                    Console.WriteLine("\n\tRegistro eliminado correctamente");
                }
                else
                {
                    Console.WriteLine("\n\tNo se ha eliminado nada");
                }
                Utilidades.Pausa("volver al menú");
            }
            catch (IOException ioe)
            {
                sW.WriteLine("\n\t** ERROR OperacionesBbddImpl DeleteLibro ** Error con el input/output al introducir un valor: " + ioe.Message);
            }
            finally
            {
                // Liberando recursos
                sW.Close();
                conexionGenerada.Close();
                consultaPreparada.Dispose();
            }
        }
    }
}
