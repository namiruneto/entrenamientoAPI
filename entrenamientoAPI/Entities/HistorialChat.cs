﻿namespace entrenamientoAPI.Entities
{
    public class HistorialChat
    {
        public Guid Id { get; set; }
        public List<Message> listaDeMensajes {  get; set; }

        public List<RespuestaPreguntas> respuestaPreguntas { get; set; }

    }


}
