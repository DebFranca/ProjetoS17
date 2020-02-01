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

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);//mas so a Add não confirma a operação no BD, pois só acessa a memória
            await _context.SaveChangesAsync(); //fazer isso confirma, porque acessa ao banco de dados, então dela deve ter a função Async
        }

        public async Task<Seller> FindByIdAsync(int id)//vai retornar o vendedor q tem esse Id, se ele não existir, vai retornar Null
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Seller.FindAsync(id);//Pego o Objeto chamando _context.Seller.Find
            _context.Seller.Remove(obj);//com o objeto na mão, coloco ele dentro do Remove, mas isso dentro do DbSet
           await _context.SaveChangesAsync();//Agora tenho que confirmar essa alteração dentro do Entity Framework usando o SaveChanges
        }

        public async Task UpdateAsync(Seller obj)
        {
            /*testar se o obj já existe no banco, pq pra atualizar tem q existir, 
            "Any" é serve para falar se existi algum registro no banco com a condição que você colocar aqui. */

            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)//se não existe no banco algum vendedor cujo o ID seja igual ao Id do Obj, faça:
            {
                throw new NotFoundException("Id not found");

                /*Porém quando você chama a operação de atualizar o banco, ele pode retornar uma excessão de conflito de concorrência, então vamos por 
              a chamada dentro do try e se ocorrer o conflito vamos usar por o cath para capturar uma possível excessão de concorrencia do Bd que 
              é DbUpdateConcurrencyException, então se vier vamos relançar em DbConcurrencyException */

            }
            try
            {
                _context.Update(obj); //se exitir atualize
                await _context.SaveChangesAsync();//confirme a atualização.

                /* Estamos Relançando em NIVEL DE SERVIÇO, isso é importante para segregar as camadas, a minha camada de serviço, ela NAO 
               * vai propagar uma excessão do NIVEL DE ACESSO A DADOS DbUpdateConcurrencyException, mas se uma excessão de NIVEL DE ACESSO A DADOS acontecer, minha camada
               * de serviço vai lançar um excessão da camada dela DbConcurrencyException e ai o meu controlador "SellerController" vai ter que lidar APENAS
               * com as execessões da camada de serviço, isso é uma forma da gente respeitar a arquitera do projeto, o controlador conversa com a camada 
               * de serviço, execessões de nivel de acesso a dados são capturadas pelo serviço e relançadas na forma de excessões de serviço para o 
               * controlador*/

            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }

    }

}
