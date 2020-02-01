using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context; //Dependencia com Context. readonly previne para não ser alterada

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync() //Método para retornar todos os departamentos 
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }

        /*vamos usar Tasks que é um objeto que encapsula o processamento assíncrono deixando a programação muito mais fácil para nós.
        1o Mudar de FindAll para FindAllAsync() essa sufixo Async é uma recomendação da Plataforma C#, não é obrigatório, mas é um
        um padrão adotado. 

        2o Agora vamos retornar de List<Department> para Task<List<Department>> 

        3o Antes do Método Task<List<Department>>  colocar a palavra async 

        4o importar: using System.Threading.Tasks;

        5o Quando chamamos alguma operação do Linq, no caso By(x => x.Name), que na vdd não é executada, porque a expressão Linq, ela só
        prepara a minha consulta e que será executada quando chamarmos alguma outra coisa que provoca a execução dela, que no caso é a
        ToList();

        6o O ToList() é executa a consulta e transforma o resulta em lista, porém o ToList é uma operação Síncrona, então a operação
        fica bloqueada executando o ToList, e para solucionar isso, usamos uma outra versão do ToList(), que não é do Linq é do Entity
        Framework, ou seja preciso importar using Microsoft.EntityFrameworkCore;  para que funcione o ToListAsync();

        7o Avisar o compilador que neste método estamos fazendo uma chamada Assíncrona, colocando await após a palavra return


        */


    }
}
    