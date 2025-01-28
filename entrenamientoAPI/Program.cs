using System.Text;
using entrenamientoAPI;
using entrenamientoAPI.Entities;
using Newtonsoft.Json;

public class Program
{
    public class listamensajes
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    List<listamensajes> HistorialMensajes = new List<listamensajes>();

    static async Task Main(string[] args)
    {
        Program program = new Program();
        await program.ejecucion();
    }
    public async Task ejecucion()
    {
        Chat chat = new Chat();


        while (true) // Bucle infinito
        {
            Console.WriteLine("Escribe algo (o escribe 'salir' para terminar):");

            string input = Console.ReadLine();

            if (input.ToLower() == "salir")
            {
                Console.WriteLine("Terminando el programa...");
                break;
            }
            Console.WriteLine(chat.MensajeNuevo(input));
        }
    }

    public async Task consumirAPiAsync(string pregunta)
    {
        string apiKey = "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";
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        HistorialMensajes.Add(
            new listamensajes
            {
                content = pregunta,
                role = "user"
            });

        using (HttpClient client = new HttpClient())
        {
            // Configura el encabezado de autorización con tu clave API
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Prepara el cuerpo de la solicitud
            var requestBody = new
            {
                model = "gpt-4o-mini",
                store = true,
                messages = new[]
                {
                    HistorialMensajes,
                }
            };


            // Serializa el cuerpo de la solicitud como JSON
            string jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            try
            {
                // Realiza la solicitud POST a la API
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Asegúrate de que la respuesta fue exitosa
                response.EnsureSuccessStatusCode();

                // Lee y muestra la respuesta de la API
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<RespuestaChatGpt>(responseBody);
                string respuesta = apiResponse.choices[0].message.content;

                // Deserializar el JSON dentro de "content"
                ContentJson contentData = JsonConvert.DeserializeObject<ContentJson>(respuesta);
                Console.WriteLine("Respuesta de la API:");
                HistorialMensajes.Add(
                    new listamensajes
                    {
                        role = "assistant",
                        content = contentData.respuesta
                    });
                Console.WriteLine(contentData.respuesta);
            }
            catch (HttpRequestException ex)
            {
                // Maneja errores en la solicitud
                Console.WriteLine($"Error al llamar a la API: {ex.Message}");
            }
        }



    }
    public class ContentJson
    {
        public string categoria { get; set; }
        public string respuesta { get; set; }
    }

}