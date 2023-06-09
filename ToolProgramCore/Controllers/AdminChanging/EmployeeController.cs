using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToolProgramCore.Models;
using static DataLibrary.BusinessLogic.Fields_Change;

namespace ToolProgramCore.Controllers.AdminChanging
{
    public class EmployeeController : Controller
    {
        //TODO add authentication data


        // GET: EmployeeController
        public ActionResult Index()
        {
            GetEmployeeList();
            
            return View(employeeList);
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

        // ** Data Loaders **

        // holds current list
        public List<Employee>? employeeList;

        [AllowAnonymous]
        // Gets the employee list and stores them in a new list with local Employee class
        // This loads each time the index runs, gets new values every refresh
        public void GetEmployeeList()
        {

            List<List<string>> data = LoadFields_dbl_lst("EMP");
            List<Employee> employeesTemp = new();

            // row = [name, Clock] 
            foreach (List<string> row in data)
            {
                // Enter values into this model: Employee. Then add to list
                employeesTemp.Add(new Employee
                {
                    FirstName = row[0].Split(',')[1].Trim(),
                    LastName = row[0].Split(',')[1].Trim(),
                    FullName = row[0],
                    Clock_Code = row[1]

                });
            }
            employeeList = employeesTemp;
        }


    }
}
