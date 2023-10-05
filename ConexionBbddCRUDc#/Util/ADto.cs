using ConexionBbddCRUDc_.Dto;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Util
{
    /// <summary>
    /// Clase ADTO del paquete util cuya función es pasar los datos del libro a Dto
    /// </summary>
    internal class ADto
    {
        public List<LibroDto> PasarDataReaderALibrosDto(NpgsqlDataReader resultadoConsulta)
        {

            List<LibroDto> listaLibros = new List<LibroDto>();

            while (resultadoConsulta.Read()) // Leemos el resultado de la consulta hasta que no queden filas
            {

                listaLibros.Add(new LibroDto(
                        long.Parse(resultadoConsulta[0].ToString()),
                        resultadoConsulta[1].ToString(),
                        resultadoConsulta[2].ToString(),
                        resultadoConsulta[3].ToString(),
                        Convert.ToInt32(resultadoConsulta[4])
                        ));
            }
            return listaLibros;
        }
    }
}
