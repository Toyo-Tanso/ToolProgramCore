﻿using Microsoft.AspNetCore.Authorization;
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
            Employee newEmployee = new Employee();
            
            return View(newEmployee);
        }

        // POST: EmployeeController/AddEmployee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(IFormCollection collection)
        {
            try
            {
                // Helper Function to insert employee
                AddEmplHelper(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // TODO: change description
        // [Helper Function] Takes out values and uses the MeasuerLogic Controller to
        // add tool check in
        private void AddEmplHelper(IFormCollection collection)
        {

            string FirstName = collection["FirstName"].ToString();
            string LastName = collection["LastName"].ToString();
            string Clock_Code = collection["Clock_Code"];

            // Clock_Code will be unique because of model class data annotation
            // combine first and last name
            string fullName = LastName.Trim() + ", " + FirstName.Trim();

            // ensure length is less than 50
            // shouldn't be activated but just in case
            if (fullName.Length > 50) {
                throw new Exception("Name too long");
            }

            // enter the new employee with active status
            // call to data library
            AddEmployeeDL(fullName, Clock_Code);

        }

        // This verifies input in the form (Helper) [called in the model class]
        // Returns error if the employee does not exist in the Database
        // TODO: remove duplicates update desc
        // TODO check EMP unactives
        [AllowAnonymous]
        public IActionResult VerifyClockCode(string Clock_Code)
        {
            List<List<string>> cur_empl = LoadFields_dbl_lst("EMP_ALL");

            bool EmplExists = false;

            // if it's in the list
            foreach (var row in cur_empl)
            {
                if (row[1].Trim() == Clock_Code)
                {
                    EmplExists = true;
                }
            }

            if (EmplExists)
            {
                return Json($"Employee Number Exists. Please enter another code.");
            }
            else
            {
                return Json(true);
            }
        }

        [AllowAnonymous]

        // Ensures that every word is capitalized
        // Not 100% necessary but it's nice to have consistency in the database
        public IActionResult VerifyCapitalization(string FirstName = "", string LastName = "")
        {

            bool AllCap = true;
            string Name;

            if (FirstName.Equals(""))
            {
                Name = LastName;
            }
            else
            {
                Name = FirstName;
            }

            // check first character of every string
            foreach (string word in Name.Trim().Split(' '))
            {
                if (word.Length > 0 && !Char.IsUpper(word[0]))
                {
                    AllCap = false;
                }
            }

            if (! AllCap)
            {
                return Json($"Each word must be capitalized.");
            }
            else
            {
                return Json(true);
            }
        }

        // TODO do I need a helper to use Load Fields_dbl_lst where it was made

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
                    LastName = row[0].Split(',')[0].Trim(),
                    FullName = row[0],
                    Clock_Code = row[1]

                });
            }
            employeeList = employeesTemp;
        }


    }
}
