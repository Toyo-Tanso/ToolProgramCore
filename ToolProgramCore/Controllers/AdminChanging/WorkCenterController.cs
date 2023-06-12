using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class WorkCenterController : Controller
    {
        // GET: WorkCenterController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WorkCenterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkCenterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkCenterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkCenterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkCenterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkCenterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkCenterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
