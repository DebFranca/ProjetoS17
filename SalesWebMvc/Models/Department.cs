using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();
        //Para garantir q será instanciada, ja vamos instanciar

        public Department()
        {
        }
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            //Estou pegando cada vendedor da lista, chamando 
            //o TotalSales do vend. no período inicial e final e ai faço uma soma "Sum"
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }

    }
}
