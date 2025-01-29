using entrenamientoAPI.Entities;
using entrenamientoAPI.Entities.BaseDatos.JsonParametros;
using entrenamientoAPI.Entities.BaseDatos.Pametros;
using entrenamientoAPI.Entities.BaseDatos.Respuestas;
using entrenamientoAPI.Infrastructure.Interfaz;
using entrenamientoAPI.Infrastructure.Repositorio;
using System.Data;
using System.Text.Json;

namespace entrenamientoAPI
{
    /// <summary>
    /// Ejemplo de ejecucion y realizacion de pruebas para un chat
    /// </summary>
    public class Chat
    {
        private HistorialChat _historialChat { get; set; }

        /// <summary>
        /// Almacenar la cantidad de token por el ultimo promt enviado para validar y saber realmente la cantidad y reducir costos si es necesario hacer un resumen
        /// </summary>
        private int TotalTokenAnterior { get; set; }

        /// <summary>
        /// Validacion por si esta con un asesor real o esta con el IA
        /// </summary>
        private bool AsesorReal { get; set; }

        private IAChatGPT IA {  get; set; }
        private TokenValidator tokenValidator { get; set; }

        //construtor o iniciador de la clase
        public Chat()
        {
            //cuando inicia la clase creamos la historial del chat para manejar y guardar las respuestas para entrenamiento local
            _historialChat = new();
            //Creamos una nueva ID para este chat 
            _historialChat.Id = Guid.NewGuid();
            //Inciamos las primeras conversaciones 
            _historialChat.listaDeMensajes = new List<Message>();
            //iniciamos las respuestas para cada pregunta
            _historialChat.respuestaPreguntas = new List<RespuestaPreguntas>();

            ////TODO: Andres - validar con chatgpt el carge de informacion para reducir costos en el promt como el que se esta enviando el modelo que deseo que me responda
            
            //Creamos el primer promt que va ser del sistema para iniciar la conversacion
            _historialChat.listaDeMensajes.Add(MensajeIncialDeSystem());
            //realizamos el inico de la conexion y parametros para la inteligencia artificial
            //ya se va organizar que empresa o entidad se va utilizar 
            IA = new IAChatGPT();  
            tokenValidator = new TokenValidator();
            //iniciamos el total de tokens en 0 para posteiror validacion
            TotalTokenAnterior = 0;
        }


        private Message MensajeIncialDeSystem()
        {
            return new Message
            {
                role = "system",
                content = "Eres un asistente virtual de una cooperativa financiera llamada Namiruneto, " +
                        "nos encargamos de realizar prestamos, cuentas de ahorro y cdts entre otros," +
                        "responde exclusivamente en formato JSON con el estado \"escalar\". " +
                        "Si necesitas más datos, indica \"incompleto\". " +
                        "Si puedes reponder correcto, indica \"completado\". " +
                        "Usa la estructura:" +
                        "\n\n{\n  \"categoria\": \"...\",\n " +
                        " \"subcategoria\": \"...\",\n " +
                        " \"respuesta\": \"...\",\n " +
                        " \"estado\": \"...\" // \"completado\", \"incompleto\" o \"escalar\"\n}",

            };
        }

        /// <summary>
        /// Clase encargada para recibir los mensajes y emitir la respuesta.
        /// </summary>
        /// <param name="MensajeRecibido"></param>
        /// <returns></returns>       
        public string MensajeNuevo(string MensajeRecibido)
        {
            RespuestaDeApi SolucionProblema = new RespuestaDeApi();
            //si esta activo el asesor se emite el mensaje directamente para que se encarge de seguir con el chat
            if (AsesorReal)
            {  
                return Asesor(MensajeRecibido);
            }

            //validar la cantidad de token para procesar el siguiente paso que es generar el resumen si no es apropiado
            if (!tokenValidator.ValidarTokens(MensajeRecibido, TotalTokenAnterior))
            {
                List<Message> men = new List<Message>();
                men.AddRange(_historialChat.listaDeMensajes.Where(x => x.role != "system"));
                men.Add(new Message
                {
                    role = "system",
                    content = "Realiza un resumen de toda la conversación que va desde las preguntas y respuestas del asistente en pocas lineas para reducir el tamaño del promt y tokens pero sin perder la historia y sea utilizada para una nueva conversacion"
                });
                string resumen = IA.ConsumirApiSencilla(men).GetAwaiter().GetResult();

                _historialChat.listaDeMensajes = new List<Message>();
                _historialChat.listaDeMensajes.Add(MensajeIncialDeSystem());
                _historialChat.listaDeMensajes.Add(new Message
                {
                    role = "system",
                    content = $"El usuario desea continuar con la conversación anterior. Aquí tienes un resumen de lo que se habló:\n\n{resumen}\n\nPor favor, responde a la nueva petición del usuario."
                }); 
            }        
            
            //Luego de recibir se agrega a las respuestas o historial que se tiene en el momento
            _historialChat.listaDeMensajes.Add(
                new Message
                {
                    role = "user",
                    content = MensajeRecibido,
                });

            //luego de tener el mensaje nuevo agregado en el historial realizamos la peticion
            
            var (RespuestaApi, TokenUtilizados) = IA.ConsumirApiAsync(_historialChat.listaDeMensajes).GetAwaiter().GetResult();
            SolucionProblema = RespuestaApi;
            TotalTokenAnterior = TokenUtilizados;
            //se realiza el guardado de la informacion en la lista para posteiormente procesar y guardar la informacion 
            _historialChat.respuestaPreguntas.Add(new RespuestaPreguntas
            {
                Pregunta = MensajeRecibido,
                Respuesta = SolucionProblema,
                Asesor = false,
            });  
            _historialChat.listaDeMensajes.Add(new Message
            {
                role = "assistant",
                content = SolucionProblema.respuesta,
            });

            //se valida si el estado de la respuesta para proceder a realizar un tramite correspondiente con un asesor real
            if(SolucionProblema.estado == "escalar")
            {
                return Asesor(MensajeRecibido);
            }

            ////TODO: Andres - luego de recibir la respuesta validar el estado para saber que proceso seguir si pasar a un asesor o realizar otra logica por categoria
            
            ////TODO: Andres - queda pendiente agregar cuando se requiere hacer el tema de validacion de autentificacion
            
            ////TODO: Andres - queda pendiente la validacion cuando es finalizacion de chat cuando el usuario escribe algo relacionado que se acabo
            ///esto es cuando son mensajes como "gracias por la informacion" , "quedo claro", "adios" etc algo relacionado cuando entra en la categoria Final o algo relacionado esta categoria queda pendiente por hacer
            
                     
            //realizamos la validacion de codigo par saber el estado de la respuesta para saber que paso seguir 
            return SolucionProblema.respuesta.ToString();

        }

        private string Asesor(string MensajeRecibido)
        {
            //se realiza la validacion para enviar al asesor real para continuar con el proceso que lleva con el cliente 
            //ya con la respuesta que nos da la guardamos aca ya no manejamos guardar e historial de chat ya que es
            //para solo tema de ia y esto es para guardar en base de datos
            RespuestaDeApi resul = new RespuestaDeApi();

            _historialChat.respuestaPreguntas.Add(
                   new RespuestaPreguntas
                   {
                       Pregunta = MensajeRecibido,
                       Respuesta = resul,
                       Asesor = true,
                   });

            ////TODO: queda pendiente hacer la logica para enviar al asesor real la informacion para su respuesta

            return resul.respuesta;

        }

        

        /// <summary>
        /// Clase encargada de finalizar el chat guardar datos y procesar la informacion
        /// </summary>
        public void FinalizarChat()
        {
            List<HistorialChatType> historialChatType = _historialChat.respuestaPreguntas
                .Select(h => new HistorialChatType
                {
                    IdChat = _historialChat.Id,
                    Pregunta = h.Pregunta,
                    respuesta = h.Respuesta.respuesta,
                    Asesor = h.Asesor,
                    categoria = h.Respuesta.categoria,
                    subcategoria = h.Respuesta.subcategoria,
                }).ToList();

            IHistorialChatRepositorio<Pa_Insert_HistorialChat, RespuestaGenericaBD> _Repository = new HistorialChatRepositorio<Pa_Insert_HistorialChat, RespuestaGenericaBD>();
            Pa_Insert_HistorialChat pa = new Pa_Insert_HistorialChat
            {
                HistorialChat = JsonSerializer.Serialize(historialChatType),
            };
            _Repository.Parameters = pa;
            var rese = _Repository.GuardarDatos().Result;       
        }     

        ////TODO: Andres - nueva clase para cuando se finalice el chat enviar un mensaje preterminado y  luego genear finalizarchat



    }
}
