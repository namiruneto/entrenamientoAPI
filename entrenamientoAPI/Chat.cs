using entrenamientoAPI.Entities;
using static Program;

namespace entrenamientoAPI
{
    /// <summary>
    /// Ejemplo de ejecucion y realizacion de pruebas para un chat
    /// </summary>
    public class Chat
    {
        private HistorialChat _historialChat { get; set; }

        /// <summary>
        /// Validacion por si esta con un asesor real o esta con el IA
        /// </summary>
        private bool AsesorReal { get; set; }

        private IAChatGPT IA {  get; set; }
        
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
            _historialChat.listaDeMensajes.Add
                (
                    new Message
                    {
                        role = "system",
                        content = "Eres un asistente virtual de una cooperativa financiera. " +
                        "Si no tienes suficiente información para responder una consulta, " +
                        "responde exclusivamente en formato JSON con el estado \"escalar\". " +
                        "Si necesitas más datos, indica \"incompleto\". " +
                        "Usa la estructura:" +
                        "\n\n{\n  \"categoria\": \"...\",\n " +
                        " \"subcategoria\": \"...\",\n " +
                        " \"respuesta\": \"...\",\n " +
                        " \"estado\": \"...\" // \"completado\", \"incompleto\" o \"escalar\"\n}",
                        
                    }
                );
            //realizamos el inico de la conexion y parametros para la inteligencia artificial
            //ya se va organizar que empresa o entidad se va utilizar 
            IA = new IAChatGPT();            
        }

        /// <summary>
        /// Clase encargada para recibir los mensajes y emitir la respuesta.
        /// </summary>
        /// <param name="MensajeRecibido"></param>
        /// <returns></returns>       
        public string MensajeNuevo(string MensajeRecibido)
        {   
            ////TODO: Andres - validar después de cierta cantidad de palabras al llenar el historial de mensajes
            ///generar un prom para un resumen de toda la información y procesar para que sea mas liviano el promt y reducir token
            ///
            RespuestaDeApi SolucionProblema = new RespuestaDeApi();
            //si esta activo el asesor se emite el mensaje directamente para que se encarge de seguir con el chat
            if ( AsesorReal)
            {
                SolucionProblema = Asesor(MensajeRecibido);
                _historialChat.respuestaPreguntas.Add(
                    new RespuestaPreguntas
                    {
                        Pregunta = MensajeRecibido,
                        Respuesta = SolucionProblema,
                    });
                return SolucionProblema.respuesta;
            }
            
            //Luego de recibir se agrega a las respuestas o historial que se tiene en el momento
            _historialChat.listaDeMensajes.Add(
                new Message
                {
                    role = "user",
                    content = MensajeRecibido,
                });

            //luego de tener el mensaje nuevo agregado en el historial realizamos la peticion
            SolucionProblema = IA.ConsumirApiAsync(_historialChat.listaDeMensajes).GetAwaiter().GetResult();  
            ////TODO: Andres - luego de recibir la respuesta validar el estado para saber que proceso seguir si pasar a un asesor o realizar otra logica por categoria
            
            ////TODO: Andres - queda pendiente agregar cuando se requiere hacer el tema de validacion de autentificacion
            
            ////TODO: Andres - queda pendiente la validacion cuando es finalizacion de chat cuando el usuario escribe algo relacionado que se acabo
            ///esto es cuando son mensajes como "gracias por la informacion" , "quedo claro", "adios" etc algo relacionado cuando entra en la categoria Final o algo relacionado esta categoria queda pendiente por hacer
            
            
            
            
                     
            //realizamos la validacion de codigo par saber el estado de la respuesta para saber que paso seguir 
            return SolucionProblema.respuesta.ToString();

        }

        private RespuestaDeApi Asesor(string MensajeRecibido)
        {
            //se realiza la validacion para enviar al asesor real para continuar con el proceso que lleva con el cliente 
            RespuestaDeApi resul = new RespuestaDeApi();

            ////TODO: queda pendiente hacer la logica para enviar al asesor real la informacion para su respuesta

            return resul;

        }

        /// <summary>
        /// Clase encargada de finalizar el chat guardar datos y procesar la informacion
        /// </summary>
        public void FinalizarChat()
        {
            //se inicia la base de datos y se envia el modelo de datos para que este se encarger de guardarlo
            ////TODO: Cesar - cuando se finaliza la tarea enviar los datos para guardar             
            
            
        }

        ////TODO: Andres - nueva clase para cuando se finalice el chat enviar un mensaje preterminado y  luego genear finalizarchat

        

    }
}
