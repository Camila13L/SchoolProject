using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlaveDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObjEscuela)
        {
            if (dicObjEscuela == null)
            {
                throw new ArgumentNullException(nameof(dicObjEscuela));
            }
            _diccionario = dicObjEscuela;
        }
        public IEnumerable<Evaluación> GetListaEvaluaciones()
        {
            if (_diccionario.TryGetValue(LlaveDiccionario.Evaluación,
                                                 out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            else
            {
                return new List<Evaluación>();
            }
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> listaevaluaciones)
        {
            listaevaluaciones = GetListaEvaluaciones();
            return (from ev in listaevaluaciones
                    select ev.Asignatura.Nombre).Distinct();
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetDicEvaluacionesPorAsignatura()
        {
            var dicRta = new Dictionary<string, IEnumerable<Evaluación>>();
            var listaAsig = GetListaAsignaturas(out var listaEval);

            foreach (var asig in listaAsig)
            {
                var evalsAsig = from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dicRta.Add(asig, evalsAsig);
            }

            return dicRta;
            ;

        }
        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetPromeAlumnPorAsignatura()
        {
            var rta = new Dictionary<string, IEnumerable<AlumnoPromedio>>();
            var dicEvalXAsig = GetDicEvaluacionesPorAsignatura();

            foreach (var asigConEval in dicEvalXAsig)
            {
                var promsAlumn = from eval in asigConEval.Value
                                 group eval by new
                                 {
                                     eval.Alumno.UniqueId,
                                     eval.Alumno.Nombre
                                 }
                                 into grupoEvalsAlumno
                                 select new AlumnoPromedio
                                 {
                                     alumnoid = grupoEvalsAlumno.Key.UniqueId,
                                     alumnoNombre = grupoEvalsAlumno.Key.Nombre,
                                     promedio = (float)grupoEvalsAlumno.Average(evaluacion => evaluacion.Nota)
                                 };

                rta.Add(asigConEval.Key, promsAlumn);
            }

            return rta;
        }

        public Dictionary<string, IEnumerable<AlumnoPromedio>> GetBetterScores(int topAlumnos)
        {
            var promediosAlumnos = GetPromeAlumnPorAsignatura();
            var top = new Dictionary<string, IEnumerable<AlumnoPromedio>>();
            foreach (var p in promediosAlumnos)
            {
                var topx = (from ll in p.Value  orderby ll.promedio descending select ll).Take(topAlumnos);
                top.Add(p.Key, topx);
            } 

            

            return top;
        }
    }
}