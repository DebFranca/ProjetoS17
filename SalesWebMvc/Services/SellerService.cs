using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context; //Dependencia com Context. readonly previne para não ser alterada

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            _context.Add(obj);//mas so a Add não confirma a operação no BD, 
            _context.SaveChanges(); //fazer isso confirma.
        }

        public Seller FindById(int id)//vai retornar o vendedor q tem esse Id, se ele não existir, vai retornar Null
        {
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);//Pego o Objeto chamando _context.Seller.Find
            _context.Seller.Remove(obj);//com o objeto na mão, coloco ele dentro do Remove, mas isso dentro do DbSet
            _context.SaveChanges();//Agora tenho que confirmar essa alteração dentro do Entity Framework usando o SaveChanges
        }

    }

}
