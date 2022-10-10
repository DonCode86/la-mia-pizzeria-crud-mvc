using la_mia_pizzeria.Models;
using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria.Models
{
    public class PizzasCategories
    {
        //pizza che avevamo gia'
        public Pizza Pizza { get; set; }

        public  List<Category>? Categories { get; set; }

        public PizzasCategories()
        {
            Pizza = new Pizza();
            Categories = new List<Category>();
        }
    }
}

//questo e' il modello della nostra vista