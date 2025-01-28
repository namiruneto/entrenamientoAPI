namespace entrenamientoAPI.Entities.Respuestas
{
    public class RespuestaAPIChatGpt
    {
        public string id { get; set; }
        public string Object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public List<Choice> choices { get; set; }
        public Usage usage { get; set; }
        public string service_tier { get; set; }
        public string system_fingerprint { get; set; }
    }
}
