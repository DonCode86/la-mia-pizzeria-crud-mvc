using la_mia_pizzeria.Models;
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
            PizzasCategories pizzasCategories = new PizzasCategories(); 

            pizzasCategories.Categories = new PizzaContext().Categories.ToList();

            return View(pizzasCategories);
        }

        [HttpPost]        
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
            //richiamo il db
            PizzaContext pizzaContext = new PizzaContext(); //gli passo il model
            //recupero la pizza tramite id e lo restituisco alla vista
            Pizza pizza = pizzaContext.Pizzas.Where(pizza=>pizza.Id == id).FirstOrDefault();    //vado a prendere dalle istanze del db la pizza

            if(pizza == null)
            {
                return NotFound("Non trovato");
            }
            return View(pizza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //metodo che gestisce i dati del form (gli passo l'id della pizza, e il modello con i dati inseriti nel form(formData))
        public IActionResult Update(int id, Pizza formData)
        {
            PizzaContext pizzaContext = new PizzaContext();//richiamo il db

            Pizza pizza = pizzaContext.Pizzas.Where(pizzaContext=>pizzaContext.Id == id).FirstOrDefault();//mi riprendo i dati da modificare
            // e li modifico
            if (pizza != null)
            {
                pizza.Name = formData.Name;
                pizza.Ingredients = formData.Ingredients;
                pizza.Price = formData.Price;
                pizza.Image = formData.Image;
            } else
            {
                return NotFound("Non trovato");
            }
            
            pizzaContext.SaveChanges();

            return RedirectToAction("Index");//.net costruisce l'url 302/200
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //gli passo l'id dell'oggetto che voglio eliminare come parametro
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