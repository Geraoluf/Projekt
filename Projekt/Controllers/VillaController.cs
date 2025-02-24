using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;


namespace Projekt.Controllers
{
    public class VillaController : Controller
    {

        private readonly ApplicaationDbContext _db;

        public VillaController(ApplicaationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            var villas =  _db.Villas.ToList();
            return View(villas);
        }


        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot match the name");
            }
            if (ModelState.IsValid)
            {


                var ville = _db.Villas.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index", "Villa");
            }

            return View();
        }
    }


}
