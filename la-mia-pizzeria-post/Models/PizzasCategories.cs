using la_mia_pizzeria.Models;

namespace la_mia_pizzeria.Models
{
    public class PizzasCategories
    {
        public Pizza Pizza { get; set; }

        public  List<Category> Categories { get; set; }

        public PizzasCategories()
        {
            Pizza = new Pizza();
            Categories = new List<Category>();
        }
    }
}
