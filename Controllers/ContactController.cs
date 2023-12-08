using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactManager.Data;
using ContactManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ContactManager.Controllers
{

    [Authorize]//donne page d'identification si on n'est pas connecte
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;//connexion a la DB
        private readonly UserManager<IdentityUser> _userManager;// etape 1

        public ContactController(ApplicationDbContext context, UserManager<IdentityUser> userManager)//etape 2
        {
            _context = context;
            _userManager = userManager;//etape 3
        }

        //4
        private Task<IdentityUser?> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        // GET: Contact
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();//etape 5
            if(user == null)
            {
                return NotFound();
            }

            var applicationDbContext = _context.Contact.Include(c => c.Categorie)
                .Where(c=>c.UserId == user.Id);//etape 6
            return View(await applicationDbContext.ToListAsync());
        }

        //ajouter ContactCategorie
        public async Task<IActionResult> ContactCategorie(int? id)
        {
            if (id == null || _context.Contact == null)
            {//souris droite add view
                return NotFound();
            }
            var applicationDbContext = _context.Contact.Include(m => m.Categorie)
                .Where(m => m.CategorieID == id);

            
            var ContactCategorie = _context.Categorie.Where(m => m.CategorieID == id);
            ViewData["Categorie"] = ContactCategorie.Single().CategorieName;

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.Categorie)
                .FirstOrDefaultAsync(m => m.ContactId == id);

            // on arrure que contact existe et utilisateur est connecte
            var user = await GetCurrentUserAsync();
            if (contact == null || contact.UserId != user.Id)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            ViewData["CategorieID"] = new SelectList(_context.Categorie, "CategorieID", "CategorieName");
            return View();
        }

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,Prenom,Nom,Adresse,Ville,Province,CodePostal,Telephone,Courriel,DateCreation,CategorieID,UserName,UserID")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();//etape 5
                if (user == null)
                {
                    return NotFound();
                }
                contact.UserId = user.Id;//etape 6
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieID"] = new SelectList(_context.Categorie, "CategorieID", "CategorieName", contact.CategorieID);
            return View(contact);
        }

        // GET: Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.Categorie)
                .FirstOrDefaultAsync(c => c.ContactId == id);

            // on arrure que contact existe et utilisateur est connecte
            var user = await GetCurrentUserAsync();
            if (contact == null || contact.UserId != user.Id)
            {
                return NotFound();
            }

            ViewData["CategorieID"] = new SelectList(_context.Categorie, "CategorieID", "CategorieName", contact.CategorieID);
            return View(contact);
        }

        // POST: Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,Prenom,Nom,Adresse,Ville,Province,CodePostal,Telephone,Courriel,DateCreation,CategorieID,UserName")] Contact contact)
        {

            if (id != contact.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.ContactId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategorieID"] = new SelectList(_context.Categorie, "CategorieID", "CategorieName", contact.CategorieID);
            return View(contact);
        }

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await GetCurrentUserAsync();//etape 5
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .Include(c => c.Categorie)
                .FirstOrDefaultAsync(c => c.ContactId == id &&c.UserId == user.Id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            if (contact != null)
            {
                _context.Contact.Remove(contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.ContactId == id);
        }
    }
}
