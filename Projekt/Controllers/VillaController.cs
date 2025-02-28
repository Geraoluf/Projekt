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
        {      //custom validation
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot match the name");
            }

            if (ModelState.IsValid)
            {


                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been Created.";

                return RedirectToAction("Index", "Villa");
            }

            return View();
        }

        public IActionResult Update(int villaId)
        {                                           //find element(x), hvor x´s Id er lig med VillaId
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);

        }


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            
            if (ModelState.IsValid && obj.Id>0)
            {


                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been updated.";

                return RedirectToAction("Index", "Villa");
            }

            return View();
        }



        public IActionResult Delete(int villaId)
        {                                           //find element(x), hvor x´s Id er lig med VillaId
            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);

        }


        [HttpPost]
        public IActionResult Delete(Villa obj)
        {                                          //find element(x), hvor x´s Id er lig med VillaId
            Villa? objFromDb = _db.Villas.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb is not null)
            {


                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"]="The villa has been deleted.";

                return RedirectToAction("Index", "Villa");
            }
            TempData["error"]="The villa has NOT been deleted.";
            return View();
        }

    }


}
