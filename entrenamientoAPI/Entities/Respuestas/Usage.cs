﻿namespace entrenamientoAPI.Entities.Respuestas
{
    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
        public PromptTokensDetails prompt_tokens_details { get; set; }
        public CompletionTokensDetails completion_tokens_details { get; set; }
    }
}
