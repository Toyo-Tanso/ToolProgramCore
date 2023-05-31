using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class TrackerLogicController
    {

        // Loads the all the checkedOut
        public static List<BorrowedToolModel> LoadBorrowedTools()
        {
            // TODO: update sql
            string sql = @"SELECT ID, T_Date, WC, ToolNo, S_Size, EmpNo, Condition
                            FROM dbo.Daily_Check
                            ORDER BY ID DESC
                            ; ";

            return SqlDataAccess.LoadData<BorrowedToolModel>(sql);
        }

    }
}
