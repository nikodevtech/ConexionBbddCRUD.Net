using ConexionBbddCRUDc_.Dto;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Servicios
{
    /// <summary>
    /// Interfaz que contiene los métodos CRUD que realizan operaciones con la BBDD
    /// </summary>
    internal interface OperacionesBbddInterface
    {
        /// <summary>
        /// Método que realiza el SELECT para ver los registros de los libros
        /// </summary>
        /// <param name="conexionGenerada"></param>
        /// <param name="listaLibros"></param>
        void ReadLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros);
        /// <summary>
        /// Método que realiza el INSERT INTO para realizar un nuevo registro de libro
        /// </summary>
        /// <param name="conexionGenerada"></param>
        /// <param name="listaLibros"></param>
        void CreateLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros);
        /// <summary>
        ///  Método que realiza el UPDATE para modificar un registro de libro
        /// </summary>
        /// <param name="conexionGenerada"></param>
        /// <param name="listaLibros"></param>
        void UpdateLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros);
        /// <summary>
        /// Método que realiza el DELETE para elinminar un registro de libro
        /// </summary>
        /// <param name="conexionGenerada"></param>
        /// <param name="listaLibros"></param>
        void DeleteLibro(NpgsqlConnection conexionGenerada, List<LibroDto> listaLibros);


    }
}
