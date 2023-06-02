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

        // Loads the all the tools currently checked Out
        public static List<BorrowedToolModel> LoadBorrowedTools()
        {
            // 
            string sql = @"SELECT ID, ToolNo, Date_Removed, Promise_Return_Date,
                            WC_From, WC_To, EmpNo
                            FROM dbo.Tool_Moves1
                            WHERE Returned_Date is NULL
                            ORDER BY Date_Removed ASC
                            ; ";

            return SqlDataAccess.LoadData<BorrowedToolModel>(sql);
        }

        // Loads the all records of tools being checked Out
        public static List<BorrowedToolModel> LoadAllBorrowedTools()
        {

            string sql = @"SELECT ID, ToolNo, Date_Removed, Promise_Return_Date,
                            WC_From, WC_To, EmpNo, Returned_Date, Return_EmpNo
                            FROM dbo.Tool_Moves1
                            ORDER BY Date_Removed DESC
                            ; ";

            return SqlDataAccess.LoadData<BorrowedToolModel>(sql);
        }

    }
}
