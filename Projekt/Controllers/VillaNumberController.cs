using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;


namespace Projekt.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly ApplicaationDbContext _db;

        public VillaNumberController(ApplicaationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            var villaNumbers =  _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }


        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u=> new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            ViewData["VillaList"] = list;
            return View();
        }


        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            ModelState.Remove("Villa");

            // Tjek om denne Villa_Number allerede findes (fordi det er primary key)
            bool villaNumberAlreadyExists = _db.VillaNumbers.Any(v => v.Villa_Number == obj.Villa_Number);

            if (villaNumberAlreadyExists)
            {
                ModelState.AddModelError("Villa_Number", "Dette villa-nummer findes allerede. Vælg et unikt nummer.");
            }

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa-nummeret er blevet oprettet.";
                return RedirectToAction("Index");
            }

            // Husk at gensende VillaList ved fejl
            ViewData["VillaList"] = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            return View(obj);
        }







        public IActionResult Update(int villaNumber)
        {
            // Find VillaNumber med det givne Villa_Number (primærnøgle)
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumber);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // For at vise dropdown med Villa-navne i viewet
            IEnumerable<SelectListItem> villaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            ViewData["VillaList"] = villaList;

            return View(obj);
        }



        [HttpPost]
        public IActionResult Update(VillaNumber obj)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa Number updated successfully!";
                return RedirectToAction("Index");
            }

            // Hvis der er fejl i modellen, skal dropdown-listen vises igen
            ViewData["VillaList"] = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(obj);
        }




        public IActionResult Delete(int villaNumber)
        {
            VillaNumber? obj = _db.VillaNumbers.Include(v => v.Villa).FirstOrDefault(x => x.Villa_Number == villaNumber);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }



        [HttpPost]
        public IActionResult Delete(VillaNumber obj)
        {
            VillaNumber? objFromDb = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.Villa_Number);
            if (objFromDb != null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "Villa-nummeret blev slettet.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Villa-nummeret blev ikke fundet.";
            return RedirectToAction("Index");
        }


    }


}
