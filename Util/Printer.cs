using static System.Console;
namespace CoreEscuela.Util
{
    public static class Printer
    {
        public static void DrawLine(int tamano = 10)
        {
            Write(("".PadLeft(tamano, '=')) + "\n");
        }
        public static void WriteTitle(string titulo)
        {
            var tamano = titulo.Length + 4;
            DrawLine(tamano);
            WriteLine($"|{titulo.ToUpper()}|");
            DrawLine(tamano);
        }
        // public static void PrinterBeep(int hz, int tiempo, int cantidad)
        // {
        //     for (var i = 0; i <= cantidad ; i ++){
        //         System.Console.Beep(hz, tiempo); 
        //     }
        // }
    }
}