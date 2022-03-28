using System; 
using System.Collections.Generic;
using CoreEscuela.Util;

namespace CoreEscuela.Entidades //con mas objetos nos 
//hace mas facil encontrar los objetos
{
    public class Escuela : ObjetoEscuelaBase, ILugar
    {
        public int AñoDeCreacion { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Dirección { get; set; }
        public TiposEscuela TipoEscuela { get; set; }
        public List<Curso> Cursos { get; set; }
        public Escuela(string nombre, int año) => (Nombre, AñoDeCreacion) = (nombre, año);
        public Escuela(string nombre, int año,
                        TiposEscuela tipo,
                        string pais = "",
                        string ciudad = "")
        {
            (Nombre, AñoDeCreacion) = (nombre, año);
            Pais = pais;
            Ciudad = ciudad;

        }

        public override string ToString()
        {
            return $"\tNombre: {Nombre}, {System.Environment.NewLine} \tTipo:{TipoEscuela},\n \tPais:{Pais}, Ciudad{Ciudad}";
        }

        public void LimpiarLugar()
        {
            Printer.DrawLine();
            Printer.DrawLine(100);
            Console.WriteLine("Limpiando Escuela");
            foreach (var curso in Cursos)
            {
                curso.LimpiarLugar();
            }
            Printer.WriteTitle($"Escuela: {Nombre} Limpia"); 
        }

    }
}