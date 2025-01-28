
namespace entrenamientoAPI
{
    public class TokenValidator
    {
        /// <summary>
        /// Actualmente el maximo para chatgpt es de 8192 se reduce un porcentaje para tener mas control y no estar al limite
        /// </summary>
        private const int MaxTokensGPT4 = 7000;
        
        /// <summary>
        /// Token utilizado para la respuesta de cada promt con el formato json,
        /// </summary>
        private const int TokenRespuesta = 40;

        /// <summary>
        /// Calcula la cantidad de tokens utilizados por el historial de mensajes y valida si excede el límite.
        /// </summary>
        /// <param name="historialDeMensajes">Lista de mensajes en el formato esperado por OpenAI.</param>
        /// <returns>Un objeto que indica si es válido y la cantidad de tokens utilizados.</returns>
        public bool ValidarTokens(string NuevoMensaje, int ultimaCantidad)
        {
            if(ultimaCantidad == 0)
            {
                return true;
            }
         
            return MaxTokensGPT4 > (CalcularTokens(NuevoMensaje)+ ultimaCantidad);
        }

        /// <summary>
        /// Calcula el número de tokens para una lista de mensajes.
        /// </summary>
        /// <param name="mensajes">mensajes nuevo para calcular la cantidad.</param>
        /// <returns>Número total de tokens utilizados.</returns>
        private int CalcularTokens(string mensajes)
        {
            //validamos la cantidad de lo que hay y hacermos un aumento de token que
            //gasta una respuesta con la misma cantidad aumentada por el 300% para hacer una estimacion mas lo que cuesta el formato de respuesta que son 
            //lo que hay en el la variable de respuesta contante    
            return (mensajes.Split(' ').Count()*3)+ TokenRespuesta;
        }       
    }
}
