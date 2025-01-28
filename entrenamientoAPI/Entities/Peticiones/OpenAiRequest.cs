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
        public string model { get; set; }

        /// <summary>
        /// Lista de mensajes que define la conversación
        /// </summary>
        public List<Message> messages { get; set; }

        ///// <summary>
        ///// Opcional: Controla la aleatoriedad en las respuestas
        ///// </summary>
        //public double temperature { get; set; } = 1.0;

        ///// <summary>
        ///// Opcional: Máximo de tokens para la respuesta
        ///// </summary>
        //public int maxTokens { get; set; } = 200;

        ///// <summary>
        ///// Opcional: Controla la diversidad de las respuestas
        ///// </summary>
        //public double topP { get; set; } = 1.0;
    }
}
