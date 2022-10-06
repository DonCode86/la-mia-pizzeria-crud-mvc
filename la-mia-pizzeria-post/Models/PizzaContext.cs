using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria.Models
{
    public class PizzaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=db-pizzeria;Integrated Security=True;Pooling=False");
        }
        public DbSet<Pizza> Pizzas { get; set; }
    }
}
