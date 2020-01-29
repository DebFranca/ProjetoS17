using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string Message { get; set; }  //Para poder fazer uma mensagem personalizada

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); //Retorna se não for Null ou Vazio
    }
}