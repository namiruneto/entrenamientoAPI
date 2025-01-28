//using System;
//using System.Net.Http;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//class Program
//{
//    private static readonly string apiUrl = "https://graph.facebook.com/v21.0/583827761475507/messages";
//    private static readonly string token = "EAAVsIg0ZARZCABO4HV0cZCLB81f8Fh6ZAex4IJ562xqZB4FRoODU4V4AakpjJXzt9MiZA2xXgLDsE6a7WZBsTqSxYfqCjeDZCIEgTrZABk7FhpoAQOefVjoV6uFQX6L4donlczDM5BSozKeZAYeIAIk1PQ4VPL8OrhphFZCsgZC9jEz2sI6Srf6njrZCoXei0kukuoppYVwou3EhW5uEvynhwtzJ3RrHw2gZDZD";

//    public static async Task Main(string[] args)
//    {
//        await EnviarMensaje();
//    }

//    public static async Task EnviarMensaje()
//    {
//        using (HttpClient client = new HttpClient())
//        {
//            // Configurar el encabezado de autorización
//            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

//            // Crear el cuerpo de la solicitud
//            var requestBody = new
//            {
//                messaging_product = "whatsapp",
//                to = "573184460466",
//                type = "template",
//                template = new
//                {
//                    name = "hello_world",
//                    language = new
//                    {
//                        code = "en_US"
//                    }
//                }
//            };

//            // Serializar el cuerpo a JSON
//            string jsonBody = JsonSerializer.Serialize(requestBody);
//            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

//            try
//            {
//                // Enviar la solicitud POST
//                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
//                string responseContent = await response.Content.ReadAsStringAsync();

//                if (response.IsSuccessStatusCode)
//                {
//                    Console.WriteLine("Mensaje enviado con éxito.");
//                    Console.WriteLine("Respuesta: " + responseContent);
//                }
//                else
//                {
//                    Console.WriteLine("Error al enviar el mensaje.");
//                    Console.WriteLine("Código de estado: " + response.StatusCode);
//                    Console.WriteLine("Respuesta: " + responseContent);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Error al realizar la solicitud: " + ex.Message);
//            }
//        }
//    }
//}
