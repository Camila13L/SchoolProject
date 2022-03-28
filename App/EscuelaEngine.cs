using System;
using System.Collections.Generic;
using CoreEscuela.Entidades;
using System.Linq;
using CoreEscuela.Util;

namespace CoreEscuela.App
{
    public sealed class EscuelaEngine
    {
        public EscuelaEngine(Escuela escuela)
        {
            this.Escuela = escuela;

        }
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.PreEscolar,
                                    pais: "Colombia", ciudad: "Bogota"
                                );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();
        }

        public Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioDeObjetos()
        {
            var listaTmp = new List<Evaluación>();
            var listaTmpAl = new List<Alumno>();
            var listaTmpAs = new List<Asignatura>();
            var diccionario = new Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>>();
           
            diccionario.Add(LlaveDiccionario.Curso, Escuela.Cursos.Cast<ObjetoEscuelaBase>());

            foreach (var curso in Escuela.Cursos)
            {
                listaTmpAl.AddRange(curso.Alumnos);
                listaTmpAs.AddRange(curso.Asignaturas);

                foreach (var al in curso.Alumnos)
                {
                    listaTmp.AddRange(al.Evaluaciones);
                }

            }
            diccionario.Add(LlaveDiccionario.Alumno, listaTmpAl.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Asignatura, listaTmpAs.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlaveDiccionario.Evaluación, listaTmp.Cast<ObjetoEscuelaBase>());
            return diccionario;
        }

        public void ImprimirDiccionario(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dic,
                  bool imprEval = false)
        {
            foreach (var objdic in dic)
            {
                Printer.WriteTitle(objdic.Key.ToString());

                foreach (var val in objdic.Value)
                {
                    switch (objdic.Key)
                    {
                        case LlaveDiccionario.Evaluación:
                            if (imprEval)
                                Console.WriteLine(val);
                            break;
                        case LlaveDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                            break;
                        case LlaveDiccionario.Alumno:
                            Console.WriteLine("Alumno: " + val.Nombre);
                            break;
                        case LlaveDiccionario.Curso:
                            var curtmp = val as Curso;
                            if (curtmp != null)
                            {
                                int count = curtmp.Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + " Cantidad Alumnos: " + count);
                            }
                            break;
                        default:
                            Console.WriteLine(val);
                            break;
                    }
                }
            }
        }



        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipe", "Farid", "Donald", "Alvaro", "Nicolas", "Pedro" };
            string[] nombre2 = { "Fredy", "Anabel", "Rick", "Morty", "Silvana", "Homero", "Bart" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trup", "Toledo", "Fernández" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

 
            return listaAlumnos.OrderBy((a1) => a1.UniqueId).Take(cantidad).ToList();
        }



        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosescuela(
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumno = true,
            bool traeAsignatura = true
            )
        {
            return GetObjetosescuela(out int dummy, out dummy, out dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosescuela(
        out int cEV,
        bool traeEvaluaciones = true,
        bool traeCursos = true,
        bool traeAlumno = true,
        bool traeAsignatura = true
        )
        {
            return GetObjetosescuela(out cEV, out int dummy, out dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosescuela(
        out int cEV,
        out int cCu,
        bool traeEvaluaciones = true,
        bool traeCursos = true,
        bool traeAlumno = true,
        bool traeAsignatura = true
        )
        {
            return GetObjetosescuela(out cEV, out cCu, out int dummy, out dummy);
        }
        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosescuela(
        out int cEV,
        out int cCu,
        out int cAs,
        bool traeEvaluaciones = true,
        bool traeCursos = true,
        bool traeAlumno = true,
        bool traeAsignatura = true
        )
        {
            return GetObjetosescuela(out cEV, out cCu, out cAs, out int dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosescuela(
            out int cEv,
            out int cCu,
            out int cAl,
            out int cAs,
            bool traeEvaluaciones = true,
            bool traeCursos = true,
            bool traeAlumno = true,
            bool traeAsignatura = true
            )
        {
            cEv = 0;
            cAs = 0;
            cAl = 0;

            var listObj = new List<ObjetoEscuelaBase>();
            listObj.Add(Escuela);

            if (traeCursos)
                listObj.AddRange(Escuela.Cursos);
            cCu = Escuela.Cursos.Count;

            foreach (var curso in Escuela.Cursos)
            {
                cAs += curso.Asignaturas.Count;
                cAl += curso.Alumnos.Count;

                if (traeAsignatura)
                    listObj.AddRange(curso.Asignaturas);


                if (traeAlumno)
                    listObj.AddRange(curso.Alumnos);

                if (traeEvaluaciones)
                {
                    foreach (var alumno in curso.Alumnos)
                    {
                        listObj.AddRange(alumno.Evaluaciones);
                        cEv = cEv + alumno.Evaluaciones.Count;
                    }
                }
            }
            return listObj.AsReadOnly();
        }


        #region Metodos de carga


        private void CargarEvaluaciones()
        {

            var rnd = new Random();

            foreach (var curso in Escuela.Cursos)
            {
                foreach (var materia in curso.Asignaturas)
                {
                    foreach (var estudiante in curso.Alumnos)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluación
                            {
                                Asignatura = materia,
                                Nombre = $"{materia.Nombre} Ev#{i + 1}",
                                Nota = Math.Round(5 * (float)rnd.NextDouble(),2),
                                Alumno = estudiante
                            };
                            //System.Console.WriteLine($"{ev.Nota}");
                            estudiante.Evaluaciones.Add(ev);
                        }
                    }
                }
            }
        }


        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>()
                {
                    new Asignatura{Nombre = "Matemáticas"},
                    new Asignatura{Nombre = "Educación Física"},
                    new Asignatura{Nombre = "Castellano"},
                    new Asignatura{Nombre = "Ciencias Narurales"},
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }



        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
               new Curso(){ Nombre = "101", TipoJornada = TiposJornada.Mañana},
               new Curso(){ Nombre = "402", TipoJornada = TiposJornada.Tarde},
               new Curso(){ Nombre = "301", TipoJornada = TiposJornada.Noche},
               new Curso(){ Nombre = "401", TipoJornada = TiposJornada.Mañana},
               new Curso(){ Nombre = "501", TipoJornada = TiposJornada.Mañana },
               new Curso(){ Nombre = "502", TipoJornada = TiposJornada.Tarde}
            };
            Random rnd = new Random();

            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}