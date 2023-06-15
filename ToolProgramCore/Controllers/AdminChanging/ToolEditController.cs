using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class ToolEditController : Controller
    {
        // GET: ToolEditController
        // TODO: implement change desc page numbers
        public ActionResult Index()
        {
            return View();
        }

        // GET: ToolEditController/Details/5
        // TODO: implement change desc
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToolEditController/AddTool
        // TODO: implement change desc
        public ActionResult AddTool()
        {
            // TODO: add model
            //return View();
            throw new NotImplementedException();

        }


        // POST: ToolEditController/AddTool
        // TODO: implement change desc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTool(IFormCollection collection)
        {
            //try
            //{
            //    // Helper Function to insert employee
            //    AddEmplHelper(collection);
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
            throw new NotImplementedException();
        }

        // GET: ToolEditController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToolEditController/Edit/5
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

        // GET: ToolEditController/RemoveTool/5
        public ActionResult RemoveTool(int id)
        {
            return View();
        }

        // POST: ToolEditController/RemoveTool/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTool(int id, IFormCollection collection)
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

        [AllowAnonymous]

        // Ensures that every word is capitalized
        // Not 100% necessary but it's nice to have consistency in the database
        // TODO: implement change desc
        public IActionResult VerifyStartsWithTTU(string Tool_ID)
        {

            //if (!AllCap)
            //{
            //    return Json($"Each word must be capitalized.");
            //}
            //else
            //{
            //    return Json(true);
            //}
            throw new NotImplementedException();
        }

        // TODO: implement add desc
        [AllowAnonymous]
        public IActionResult VerifyUniqueID(string Tool_ID)
        {
            //List<List<string>> cur_empl = LoadFields_dbl_lst("EMP_ALL");

            //bool EmplExists = false;

            //// if it's in the list
            //foreach (var row in cur_empl)
            //{
            //    if (row[1].Trim() == Clock_Code)
            //    {
            //        EmplExists = true;
            //    }
            //}

            //if (EmplExists)
            //{
            //    return Json($"Employee Number Exists. Please enter another code.");
            //}
            //else
            //{
            //    return Json(true);
            //}

            throw new NotImplementedException();
        }



        // ** Data Loaders **

                // holds current list
        public List<Employee>? employeeList;

        [AllowAnonymous]
        // Gets the employee list and stores them in a new list with local Employee class
        // This loads each time the index runs, gets new values every refresh
        // TODO: implement change desc
        public void GetToolList()
        {

            //List<List<string>> data = LoadFields_dbl_lst("EMP");
            //List<Employee> employeesTemp = new();

            //// row = [name, Clock] 
            //foreach (List<string> row in data)
            //{
            //    // Enter values into this model: Employee. Then add to list
            //    employeesTemp.Add(new Employee
            //    {
            //        FirstName = row[0].Split(',')[1].Trim(),
            //        LastName = row[0].Split(',')[0].Trim(),
            //        FullName = row[0],
            //        Clock_Code = row[1]

            //    });
            //}
            //employeeList = employeesTemp;

            throw new NotImplementedException();
        }

    }
}
