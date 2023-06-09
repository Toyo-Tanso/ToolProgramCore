using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class EmployeeController : Controller
    {
        //TODO add authentication data


        // GET: EmployeeController
        public ActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            return View(employees);
        }

        // GET: EmployeeController/AddEmployee
        public ActionResult AddEmployee()
        {
            return View();
        }

        // POST: EmployeeController/AddEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(IFormCollection collection)
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

        //// NOT Allowed? because of loss of Data? or does it not matter?
        //// GET: EmployeeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: EmployeeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
