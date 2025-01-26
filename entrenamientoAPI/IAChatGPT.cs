using entrenamientoAPI.Entities;
using entrenamientoAPI.Entities.Peticiones;
using Newtonsoft.Json;
using System.Text;

namespace entrenamientoAPI
{
    public class IAChatGPT
    {
        private string apiKey { get; set; }
        private string apiUrl { get; set; }
        public IAChatGPT() 
        {
            apiKey = "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";
            apiUrl = "https://api.openai.com/v1/chat/completions";
        }
        public async Task<RespuestaDeApi> ConsumirApiAsync(List<Message> messages)
        {
            RespuestaDeApi resul = new RespuestaDeApi();
            using (HttpClient client = new HttpClient())
            {
                // Configura el encabezado de autorización con tu clave API
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var content = new StringContent(CrearJsonPeticion(messages), Encoding.UTF8, "application/json");
                try
                {
                    // Realiza la solicitud POST a la API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    // Asegúrate de que la respuesta fue exitosa
                    response.EnsureSuccessStatusCode();

                    // Lee y muestra la respuesta de la API
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //deserializamos la respuesta completa
                    var apiResponse = JsonConvert.DeserializeObject<RespuestaChatGpt>(responseBody);
                    //y obtenemos el resultado y retornamos
                    resul = RespuestaAPI(apiResponse.choices[0].message.content);                    
                }
                catch (HttpRequestException ex)
                {
                    // Maneja errores en la solicitud
                    resul = new RespuestaDeApi { respuesta = "No entendi tu respuesta", categoria = "ErrorAPI" };
                }
            }
            return resul;
        }

        private string CrearJsonPeticion(List<Message> mensajes)
        {
            string resul = "";
            OpenAiRequest openAiRequest = new OpenAiRequest();
            openAiRequest.Model = "gpt-4o-mini";
            openAiRequest.Messages = mensajes;
            resul = JsonConvert.SerializeObject(openAiRequest);
            return resul;
        }

        private RespuestaDeApi RespuestaAPI(string respuesta)
        {
            RespuestaDeApi resul = new RespuestaDeApi();

            try
            {
                resul = JsonConvert.DeserializeObject<RespuestaDeApi>(respuesta);           
            }
            catch(Exception ex)
            {
                ////@TODOS queda pendiente VALIDAR QUE HACER EN CASO DE EXCEPCION
                resul = new RespuestaDeApi { respuesta = "No entendi tu respuesta", categoria = "ErrorMensaje" };
            }
            return resul;
        }
    }
}
