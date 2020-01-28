using System;
using System.Collections.Generic;


namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; }
        /*estes dois acima ajuda o Framework a reconhecer os dados e aí na hora da conversão dos dados HTTP para Objeto, 
        ele saberá fazer automaticamente, por isso é são importantes */





    }
}
