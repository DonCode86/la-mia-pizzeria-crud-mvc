using la_mia_pizzeria.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            using (PizzaContext context = new PizzaContext())
            {
                //MI RECUPERO DAL CONTEXT LA LISTA DELLE PIZZE
                IQueryable<Pizza> pizzas = context.Pizzas;
                //E LI PASSO ALLA VISTA
                return View("Index", pizzas.ToList());  
            }
        }

        public IActionResult Details(int id)
        { 
            using (PizzaContext context = new PizzaContext())
            {
                //FACCIO RICHIESTA DELLE PIZZE ANDANDO A SELEZIONARE LA PIZZA SPECIFICA
                //pizzaFound e' LINQ (questa e' la method syntax)
                Pizza pizzaFound = context.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();
                //SE IL POST NON VIENE TROVATO
                if (pizzaFound == null)
                {
                    return NotFound($"La pizza con id {id} non è stato trovata");
                }
                else //ALTRIMENTI VIENE PASSATO ALLA VISTA DI DETTAGLIO CON PIZZAFOUND
                {
                    return View("Details", pizzaFound);
                }
            }
        }

        //QUESTA CREATE E' LA NOSTRA GET CHE PRODUCE IL FORM CHE DOVRA' ESSERE POST
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza formData) //APPENA SALVO PRIMA DI ENTRARE FORMDATA=NEW ECC... INIZIALIZZA L'ISTANZA PER NOI IN AUTOMATICO
        {
            PizzaContext db = new PizzaContext();

            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }
            
            using (PizzaContext context = new PizzaContext())
            {
                //AGGIUNGO I DATI NEL FORMDATA AL DB E SALVO I CAMBIAMENTI
                db.Pizzas.Add(formData);
                db.SaveChanges();
                //RITORNA ALLA LISTA DEI POST
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            PizzaContext pizzaContext = new PizzaContext();
            Pizza pizza = pizzaContext.Pizzas.Where(pizza=>pizza.Id == id).FirstOrDefault();
            

            if(pizza == null)
            {
                return NotFound("Non trovato");
            }
            return View(pizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            PizzaContext pizzaContext = new PizzaContext();
            Pizza pizza = pizzaContext.Pizzas.Where(pizza=> pizza.Id == id).FirstOrDefault();

            if (pizza == null)
            {
                return NotFound("Non trovato");
            }
            pizzaContext.Pizzas.Remove(pizza);
            pizzaContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}