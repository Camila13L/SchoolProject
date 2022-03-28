
namespace CoreEscuela.Entidades
{

    public class AlumnoPromedio
    {
        public float promedio {get; set;}
        public string alumnoid; 

        public string alumnoNombre; 
        public override string ToString()
        {
            return $"{alumnoNombre}, {promedio}";
        }
    }


}