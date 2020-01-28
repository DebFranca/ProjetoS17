using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context; //Dependencia com Context. readonly previne para não ser alterada

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Department> FindAll() //Método para retornar todos os departamentos 
        {
            return _context.Department.OrderBy(x => x.Name).ToList();
        }


    }
}
