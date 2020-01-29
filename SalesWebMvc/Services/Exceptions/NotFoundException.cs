using System;


namespace SalesWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base (message)
        {

        }
    }
}
//estamos criando isso porque queremos ter excessões específicas da nossa camada de serviço, quando temos um excessão personalizada, 
//temos a possibilidade de tratar esclusivamente essa excessão, logo, você tem um controle maior sobre como tratar cada  tipo excessão que pode ocorrer