using Microsoft.AspNetCore.Mvc;
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
    }
}
