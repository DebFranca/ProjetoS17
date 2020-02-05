using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context; //Dependencia com Context. readonly previne para não ser alterada

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate) // Interrogação'?' indica argumento opcional
        {
            //ele vai pegar SalesRecord que é do tipo Dbset e construir para mim um obj result do tipo IQueryable e em cima 
            //desse objeto result vou poder acrescentar outros objetos na consulta
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);//usando uma expressão Lambda que expresse minha restrição de data
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) //isso faz o Join das tabelas para mim
                .Include(x => x.Seller.Department)//tb faço o Join com a tabela departamento
                .OrderByDescending(x => x.Date)// ordenando por data
                .ToListAsync();
        }

        //Nesse método vamos agrupar os resultados:
        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            //ele vai pegar SalesRecord que é do tipo Dbset e construir para mim um obj result do tipo IQueryable e em cima 
            //desse objeto result vou poder acrescentar outros objetos na consulta
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);//usando uma expressão Lambda que expresse minha restrição de data
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) //isso faz o Join das tabelas para mim
                .Include(x => x.Seller.Department)//tb faço o Join com a tabela departamento
                .OrderByDescending(x => x.Date)// ordenando por data
                .GroupBy(x => x.Seller.Department)//o tipo no método:Task<List<IGrouping<Department,SalesRecord>>> sem isso dá erro
                .ToListAsync();
        }
    }
}
