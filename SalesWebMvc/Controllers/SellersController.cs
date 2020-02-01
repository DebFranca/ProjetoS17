using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        //2. o Index vai ter que chamar nossa operação FindAll do SellerService, 1o fazer a dependência
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;//vou acrescentar ele no meu construtor para ser injetado no meu objeto

        public string RequestId { get; private set; }

        //3.Para injetar a dependência fazer o construtor
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        //1.Por padrão criou Index, para testar essa ação, vamos criar uma pagina de Index,a view, vá a pasta View para criar.
        public IActionResult Index()
        {
            //4.Agora para chamar e passar como argumento na View, para gerar IActionResult contendo a lista
            var list = _sellerService.FindAll();
            return View(list);
        }

        //Entao assim: No MVC, eu chamo o controlador "Index" que acessou meu Model "var list = _sellerService.FindAll();"
        //pegou o dado na "list" e vai encaminhar esses dados para a "View" essa é a dinamica MVC acontecendo. 

        //IActionResult é o tipo de retorno de TODAS as ações
        public IActionResult Create()
        {  
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments }; //Vamos iniciar com esta lista de departamentos
            return View(viewModel);//A tela de cadastro qdo for acionada pela 1a vez, vai receber o objeto "viewModel" com deptos populados
        }

        [HttpPost]//como essa ação "Create(Seller seller)" é um POST preciso indicar fazendo isso "[HttpPost]"
        [ValidateAntiForgeryToken] //Isso serve para proteger contra ataques CSRF, ataques maliciosos.
        public IActionResult Create(Seller seller)//no parametro recebe e instanicia um objeto vendedor que veio da requisição,
        {
            //ModelState.IsValid serve para testar se o Modelo foi validado, se ele não foi validado, então retorno meu obj viewModel
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // então isso vai ficar acontecendo até o usuário preencher corretamente o formulário
            }

            _sellerService.Insert(seller); //Ação para inserir no banco da dados
            return RedirectToAction(nameof(Index)); //após inserir vou redirecionar a ação para o método Index, para voltar a tela principal.
            //nameof colocamos porque se um dia eu mudar o name "index" por outra palavra não vou precisar mudar aqui embaixo tb. 

        }

        public IActionResult Delete(int? id)//recebe um int opcional "?" indica opcional
        {
            if (id == null) //1o testo se o Id foi Null, se for sig que foi feito de forma indevida
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _sellerService.FindById(id.Value); //pegar o objeto que estou mandando deletar, devo por .Value porque é um Numble, obj opcional
            if (obj == null) //Esse Id que passei pode ser um Id que não existe, se não existir, meu método FindById retorna Null
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj); // se tudo deu certo, vou mandar meu método retornar uma View passando o obj como argumento
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id)
        {
            _sellerService.Remove(Id);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Details(int? id)//recebe um int opcional "?" indica opcional
        {
            if (id == null) //1o testo se o Id foi Null, se for sig que foi feito de forma indevida
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _sellerService.FindById(id.Value); //pegar o objeto que estou mandando deletar, devo por .Value porque é um Numble, obj opcional
            if (obj == null) //Esse Id que passei, se não existir, meu método FindById retorna Null
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj); // se tudo deu certo, vou mandar meu método retornar uma View passando o obj como argumento
        }

        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null) //Se for igual a null sig que meu obj não existia no meu banco de dados
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            //Para Abrir a tela de edição e caso de existênicia do objeto
            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, Seller seller)
        {
            //ModelState.IsValid serve para testar se o Modelo foi validado, se ele não foi validado, então retorno meu obj viewModel
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); // então isso vai ficar acontecendo até o usuário preencher corretamente o formulário
            }

            if (Id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not mismatch" });
            }
            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
           
        }

        public IActionResult Error(string message)
        {
            //1o instanciar um viewModel q vai receber a view Error que criamos
            var viewModel = new ErrorViewModel
            {
                Message = message, //A atributo dele vai a mensagem que esta como argumento
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                /* foi usado o machete do Frameword para PEGAR esse Id interno da requesição. Current é opcional pois tem "?" se for
                 null vamos usar o operador de coeficiencia nulla "??" e vou querer no lugar o objeto HttpContext.TraceIdentifier   */

            };
            return View(viewModel);
        }


    }
}