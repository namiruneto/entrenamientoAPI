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
            apiKey = "";
            apiUrl = "https://api.openai.com/v1/chat/completions";
        }
        public async Task<(RespuestaDeApi, int)> ConsumirApiAsync(List<Message> messages)
        {
            RespuestaDeApi resul = new RespuestaDeApi();
            int tokenUtilizados = 0;
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
                    tokenUtilizados = apiResponse.usage.total_tokens;
                }
                catch (HttpRequestException ex)
                {
                    // Maneja errores en la solicitud
                    resul = new RespuestaDeApi { respuesta = "No entendi tu respuesta", categoria = "ErrorAPI", subcategoria = "" };
                }
            }
            return (resul, tokenUtilizados);
        }

        public async Task<string> ConsumirApiSencilla(List<Message> messages)
        {
            string resul = "";
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
                    resul = apiResponse.choices[0].message.content;
                }
                catch (HttpRequestException ex)
                {
                    // Maneja errores en la solicitud
                    resul = "";
                }
            }
            return resul;
        }



        private string CrearJsonPeticion(List<Message> mensajes)
        {
            string resul = "";
            OpenAiRequest openAiRequest = new OpenAiRequest();
            openAiRequest.model = "gpt-4o-mini";
            openAiRequest.messages = mensajes;
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
                resul = new RespuestaDeApi { respuesta = "No entendi tu respuesta", categoria = "ErrorMensaje", subcategoria = "" };
            }
            return resul;
        }
    }
}
