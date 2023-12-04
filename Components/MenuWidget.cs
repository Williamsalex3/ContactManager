using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
namespace ContactManager.Components
{
    public class MenuWidget:ViewComponent
    {
        //database context
        private readonly ApplicationDbContext _context;

        //constructeur initialise conexion a base de donnee
        public MenuWidget(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetCategorieAsync();
            return View(items);
        }
        public Task<List<Categorie>> GetCategorieAsync()
        {

            return _context.Categorie.ToListAsync();
        }
    }
}
