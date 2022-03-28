using System;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += AcctionDelEvento;
            AppDomain.CurrentDomain.ProcessExit += (o,s) => Printer.DrawLine(100);

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUAL");
           

            var dic = engine.GetDiccionarioDeObjetos();
            var reporteador = new Reporteador(dic){};
            var evalList = reporteador.GetDicEvaluacionesPorAsignatura();
            var promedios = reporteador.GetPromeAlumnPorAsignatura();
            var top = reporteador.GetBetterScores(5);

            foreach (var v1c in top)
            {
                foreach (var v2 in v1c.Value)
                {
                    Console.WriteLine($"Alumno: {v2.alumnoNombre} \t - \t {v2.promedio}");
                }
            }
            
            //Console.WriteLine(evalList);
             //imprimirCursoEscuela(engine.Escuela);
            //engine.ImprimirDiccionario(dic, true); 
            //var outs = engine.GetObjetosescuela();

            



            // var listaILugar = from obj in ListaObjetos
            //                   where obj is ILugar
            //                   select (ILugar)obj;

            //engine.Escuela.LimpiarLugar(); 


        }

        private static void AcctionDelEvento(object sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO DE LA APLICACION"); 
        }

        private static void imprimirCursoEscuela(Escuela escuela)
        {
            Printer.WriteTitle("Cursos Escuela");
            if (escuela?.Cursos != null) //Pregunta priemro escuela y luego pregunta cursos
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre: {curso.Nombre}, Curso: {curso.UniqueId}");
                }
            }

        }
        
    }
}
