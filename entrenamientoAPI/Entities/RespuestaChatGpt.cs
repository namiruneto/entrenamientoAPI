using entrenamientoAPI.Entities.Respuestas;

namespace entrenamientoAPI.Entities
{
    /// <summary>
    /// Modelo de datos para deserializar el json de ChatGpt
    /// </summary>
    public class RespuestaChatGpt
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public string model { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
        public string service_tier { get; set; }
        public string system_fingerprint { get; set; }
    }
}
