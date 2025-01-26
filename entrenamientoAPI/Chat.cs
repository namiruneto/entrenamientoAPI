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
            //Creamos el primer promt que va ser del sistema para iniciar la conversacion
            _historialChat.listaDeMensajes.Add
                (
                    new Message
                    {
                        role = "system",
                        content = "Eres un asistente. Somos una cooperativa financiera que realiza " +
                        "préstamos y manejamos tarjetas de débito y crédito a nuestros clientes. " +
                        "Responde exclusivamente en formato JSON con la estructura: { 'categoria': '...', 'respuesta': '...'}.",
                    }
                );
            //realizamos el inico de la conexion y parametros para la inteligencia artificial
            //ya se va organizar que empresa o entidad se va utilizar 
            IA = new IAChatGPT();            
        }

        //Clase encargada para recibir los mensajes y emitir la respuesta
        public string MensajeNuevo(string MensajeRecibido)
        {
            string resul = "";
            //Luego de recibir se agrega a las respuestas o historial que se tiene en el momento
            _historialChat.listaDeMensajes.Add(
                new Message
                {
                    role = "user",
                    content = MensajeRecibido,
                });

            //luego de tener el mensaje nuevo agregado en el historial realizamos la peticion
            RespuestaDeApi respuesta = IA.ConsumirApiAsync(_historialChat.listaDeMensajes).GetAwaiter().GetResult();            
            return resul;
        }

    }
}
