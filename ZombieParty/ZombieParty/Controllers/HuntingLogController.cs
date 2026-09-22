using Microsoft.AspNetCore.Mvc;
using ZombieParty.Models;
using ZombieParty.Models.Data;

namespace ZombieParty.Controllers
{
    public class HuntingLogController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public HuntingLogController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }
        public IActionResult Index()
        {
            List<HuntingLog> huntingLogs = _baseDonnees.HuntingLogs.ToList();
            return View(huntingLogs);
        }
        public IActionResult Upsert(int? Id)
        {
            if (Id == null || Id == 0)
            {
                //Create
                return View(new HuntingLog());
            }
            else
            {
                //update
                return View(_baseDonnees.HuntingLogs.Find(Id));
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(HuntingLog huntingLog)
        {
            if (ModelState.IsValid)
            {
                if (huntingLog.Id == 0)
                {
                    _baseDonnees.HuntingLogs.Add(huntingLog);
                    TempData["Success"] = $"{huntingLog.Title} hunting log added";

                    _baseDonnees.SaveChanges();
                }
                else
                {
                    _baseDonnees.HuntingLogs.Update(huntingLog);
                    TempData["Success"] = $"{huntingLog.Title} hunting log updated";
                }
                _baseDonnees.SaveChanges();
                return this.RedirectToAction("Index");

            }

            return this.View(huntingLog);
        }
    }
}
