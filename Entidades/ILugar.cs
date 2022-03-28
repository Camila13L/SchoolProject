namespace CoreEscuela.Entidades
{
    public interface ILugar
    {
        //TODOS los atributos de una interface
        //Son publicos
         string Dirección { get; set; }
         void LimpiarLugar();

    }
}