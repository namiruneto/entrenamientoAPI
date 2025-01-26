namespace entrenamientoAPI.Entities
{
    /// <summary>
    /// Modelo encargado de la respuesta de la IA y poderlo procesar en el equipo 
    /// y dar respuesta al cliente y saber que ya se termino el proceso.
    /// </summary>
    public class RespuestaDeApi
    {
        public string categoria { get; set; }
        public string respuesta { get; set; }
    }
}
