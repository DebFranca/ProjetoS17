using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //Por padrão criou Index, para testar essa ação, vamos criar uma pagina de Index,a view, vá a pasta View para criar.
        public IActionResult Index()
        {
            
            return View();
        }
    }
}