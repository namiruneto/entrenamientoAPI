using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entrenamientoAPI.Entities.Peticiones
{
    public class OpenAiRequest
    {
        /// <summary>
        /// Modelo a usar (por ejemplo, "gpt-4")
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Lista de mensajes que define la conversación
        /// </summary>
        public List<Message> Messages { get; set; }

        /// <summary>
        /// Opcional: Controla la aleatoriedad en las respuestas
        /// </summary>
        public double Temperature { get; set; } = 1.0;

        /// <summary>
        /// Opcional: Máximo de tokens para la respuesta
        /// </summary>
        public int MaxTokens { get; set; } = 200;

        /// <summary>
        /// Opcional: Controla la diversidad de las respuestas
        /// </summary>
        public double TopP { get; set; } = 1.0;
    }
}
