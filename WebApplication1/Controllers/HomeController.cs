using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // Este endpoint recibe las notificaciones de WhatsApp
        [HttpPost]
        public IActionResult RecibirMensaje([FromBody] JObject data)
        {
            try
            {
                // Imprimir la data recibida para ver qué información contiene
                Console.WriteLine("Mensaje recibido: " + data.ToString());

                // Procesar la información del mensaje recibido
                if (data["entry"] != null)
                {
                    foreach (var entry in data["entry"])
                    {
                        var messaging = entry["messaging"];
                        foreach (var message in messaging)
                        {
                            string senderId = message["from"].ToString();  // El número de teléfono del remitente
                            string texto = message["message"]["text"].ToString();  // El mensaje recibido
                            Console.WriteLine($"Mensaje de {senderId}: {texto}");

                            // Aquí podrías agregar lógica para responder automáticamente o almacenar los mensajes
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el mensaje: " + ex.Message);
                return BadRequest("Error al procesar el mensaje.");
            }
        }

        // Este endpoint es utilizado para verificar el Webhook (Meta te lo solicitará)
        [HttpGet]
        public IActionResult VerificarWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
        {
            const string verifyToken = "TU_TOKEN_DE_VERIFICACION";

            if (hub_mode == "subscribe" && hub_verify_token == verifyToken)
            {
                return Ok(hub_challenge);
            }

            return Unauthorized();
        }
    }
}
