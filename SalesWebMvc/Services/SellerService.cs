using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);//Pego o Objeto chamando _context.Seller.Find
            _context.Seller.Remove(obj);//com o objeto na mão, coloco ele dentro do Remove, mas isso dentro do DbSet
            _context.SaveChanges();//Agora tenho que confirmar essa alteração dentro do Entity Framework usando o SaveChanges
        }

        public void Update(Seller obj)
        {
            /*testar se o obj já existe no banco, pq pra atualizar tem q existir, 
            "Any" é serve para falar se existi algum registro no banco com a condição que você colocar aqui. */
            if (! _context.Seller.Any(x => x.Id == obj.Id))//se não existe no banco algum vendedor cujo o ID seja igual ao Id do Obj, faça:
            {
                
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj); //se exitir atualize
                _context.SaveChanges();//confirme a atualização.

                /*Porém quando você chama a operação de atualizar o banco, ele pode retornar uma excessão de conflito de concorrência, então vamos por 
                a chamada dentro do try e se ocorrer o conflito vamos usar por o cath para capturar uma possível excessão de concorrencia do Bd que 
                é DbUpdateConcurrencyException, então se vier vamos relançar em DbConcurrencyException */
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);

                /* Estamos Relançando em NIVEL DE SERVIÇO, isso é importante para segregar as camadas, a minha camada de serviço, ela NAO vai propagar uma 
                 * excessão do NIVEL DE ACESSO A DADOS DbUpdateConcurrencyException, mas se uma excessão de NIVEL DE ACESSO A DADOS acontecer, minha camada
                 * de serviço vai lançar um excessão da camada dela DbConcurrencyException e ai o meu controlador "SellerController" vai ter que lidar APENAS
                 * com as execessões da camada de serviço, isso é uma forma da gente respeitar a arquitera do projeto, o controlador conversa com a camada 
                 * de serviço, execessões de nivel de acesso a dados são capturadas pelo serviço e relançadas na forma de excessões de serviço para o 
                 * controlador*/
            }

        }

    }

}
