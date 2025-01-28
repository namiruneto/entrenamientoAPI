using entrenamientoAPI.Entities;
//using OpenAI_API;
//using OpenAI_API.Chat;
using System.Collections.Generic;
using System.Linq;

namespace entrenamientoAPI
{
    public class TokenValidator
    {
        private const int MaxTokensGPT4 = 8192; // Máximo permitido para GPT-4

        /// <summary>
        /// Calcula la cantidad de tokens utilizados por el historial de mensajes y valida si excede el límite.
        /// </summary>
        /// <param name="historialDeMensajes">Lista de mensajes en el formato esperado por OpenAI.</param>
        /// <returns>Un objeto que indica si es válido y la cantidad de tokens utilizados.</returns>
        public static (bool esValido, int tokensUsados) ValidarTokens(List<Message> historialDeMensajes)
        {
            int tokensUsados = CalcularTokens(historialDeMensajes);
            return (tokensUsados <= MaxTokensGPT4, tokensUsados);
        }

        /// <summary>
        /// Calcula el número de tokens para una lista de mensajes.
        /// </summary>
        /// <param name="mensajes">Lista de mensajes.</param>
        /// <returns>Número total de tokens utilizados.</returns>
        private static int CalcularTokens(List<Message> mensajes)
        {
            // Usamos una aproximación basada en el tamaño del texto.
            // Para mayor precisión, puedes utilizar herramientas específicas de tokenización como `tiktoken` en Python.

            int totalTokens = 0;

            foreach (var mensaje in mensajes)
            {
                totalTokens += TokenizarTexto(mensaje.role).Count() + TokenizarTexto(mensaje.content).Count();
            }

            // Añadimos un pequeño margen para metadatos adicionales que pueda agregar el modelo.
            return totalTokens + mensajes.Count * 2;
        }

        /// <summary>
        /// Divide el texto en tokens usando una aproximación simple.
        /// </summary>
        /// <param name="texto">El texto a dividir.</param>
        /// <returns>Lista de tokens.</returns>
        private static IEnumerable<string> TokenizarTexto(string texto)
        {
            return texto.Split(' '); // Simplificación: cada palabra como un token.
        }

        /// <summary>
        /// Genera un resumen si los tokens exceden el límite.
        /// </summary>
        /// <param name="mensajes">Historial de mensajes.</param>
        /// <returns>Historial resumido.</returns>
        public static List<Message> ResumirHistorial(List<Message> mensajes)
        {
            // Seleccionamos los mensajes más relevantes para incluirlos en el resumen.
            var resumen = mensajes.TakeLast(5).ToList();
            resumen.Insert(0, new Message
            {
                role = "system",
                content = "Resumen de la conversación: Se han omitido mensajes previos para optimizar los tokens."
            });
            return resumen;
        }
    }
}
