namespace entrenamientoAPI.Entities.BaseDatos.JsonParametros
{
    /// <summary>
    /// Para consumir procedimiento almacenado 
    /// </summary>
    public class HistorialChatType : RespuestaDeApi
    {
        /// <summary>
        /// Se convierte en un json para ser guardado en la base de datos
        /// </summary>
        public Guid IdChat { get; set; }
        public string Pregunta { get; set; }
        public bool Asesor { get; set; }
    }
}
