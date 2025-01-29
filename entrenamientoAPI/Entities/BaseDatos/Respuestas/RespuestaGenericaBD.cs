namespace entrenamientoAPI.Entities.BaseDatos.Respuestas
{
    /// <summary>
    /// Respuesta generica de la base de datos al consumir Procedimientos Almacenados
    /// </summary>
    public class RespuestaGenericaBD
    {
        public string Message { get; set; }
        public bool Estatus { get; set; }

    }
}
