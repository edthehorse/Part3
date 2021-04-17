using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Packt.CS7;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindWeb.Pages
{
    public class SuppliersModel : PageModel
    {
        private Northwind db;

        public SuppliersModel(Northwind injectedContext)
        {     db = injectedContext;    }

        public IEnumerable<string> Suppliers { get; set; }

        public void OnGet() // From the URL: /Suppliers
        {
            ViewData["Title"] = "Northwind Web Site - Suppliers";

            Suppliers = db.Suppliers.Select(s => s.CompanyName).ToArray(); // GEts the list of suppliers from the dbs and puts them into the Suppliers array.
        }

        [BindProperty]
        public Supplier Supplier { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)// ModelState is a property of ModelState
            {
                db.Suppliers.Add(Supplier); // New supplier.

                db.SaveChanges();

                return RedirectToPage("/suppliers"); // After posting the form dish up the Suppliers form showing all of the suppliers & not the new supplier form. ?? ed
            }
            return Page(); // ?
        }

    }
}