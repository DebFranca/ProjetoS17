using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordsService;

        public SalesRecordsController(SalesRecordService salesRecordsService)
        {
            _salesRecordsService = salesRecordsService;
        }

        public IActionResult Index()
        {
            return View();
        }
            
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue) //Se minha data minima não possui valor, entao vou atribuir um valor padrão a ela: 1/jan/ano atual
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue) //Se minha data maxima não possui valor, atribua a data atual
            {
                maxDate = DateTime.Now;
            }
            //agora passando minDate e maxDate para minha View, utilizando o dicionário view data
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            //preciso chamar o serviço com a operação FyndByDate, só que para usar o serviço dentro do controlador tenho que declarar 
            //a dependência dele aqui pondo o  private readonly ... e fazer o construtor dele, após isso faço:
            var result = await _salesRecordsService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}