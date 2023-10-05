using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConexionBbddCRUDc_.Dto
{
    internal class LibroDto
    {
        //Atributos
        private long idLibro;
        private String titulo;
        private String autor;
        private String isbn;
        private int edicion;

        public LibroDto(long idLibro, string titulo, string autor, string isbn, int edicion)
        {
            this.IdLibro = idLibro;
            this.Titulo = titulo;
            this.Autor = autor;
            this.Isbn = isbn;
            this.Edicion = edicion;
        }
        public LibroDto(string titulo, string autor, string isbn, int edicion)
        {
            this.titulo = titulo;
            this.autor = autor;
            this.isbn = isbn;
            this.edicion = edicion;
        }

        //Getter y setter
        public long IdLibro { get => idLibro; set => idLibro = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Autor { get => autor; set => autor = value; }
        public string Isbn { get => isbn; set => isbn = value; }
        public int Edicion { get => edicion; set => edicion = value; }

        //Métodos
        public void MostrarDatos()
        {
            Console.Write("\n\n\t--- Datos del libro con ID {5} ---\n\n\tid_libro: {0}\n\tTitulo: {1}\n\tAutor: {2}\n\tISBN: {3}\n\tEdición: {4}\n\t", IdLibro, Titulo, Autor, Isbn, Edicion, IdLibro);
        }



    }
}
