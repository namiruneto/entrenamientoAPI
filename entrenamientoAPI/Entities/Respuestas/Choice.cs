namespace entrenamientoAPI.Entities.Respuestas
{
    public class Choice
    {
        public int Index { get; set; }
        public Message message { get; set; }
        public object logprobs { get; set; } // Puede ser null o un objeto, cambiar según necesidad
        public string finish_reason { get; set; }

    }
}
