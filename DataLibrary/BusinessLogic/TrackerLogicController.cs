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

        public static int saveCheckOut(string ToolNo, string Promise_Return_Date, string WC_From, string WC_To, string EmpNo)
        {
            // Turns data into local toolMeasure class
            BorrowedToolModel data = new BorrowedToolModel
            {
                Promise_Return_Date = Promise_Return_Date,
                WC_From = WC_From,
                ToolNo = ToolNo,
                WC_To = WC_To,
                EmpNo = EmpNo,
            };

            string sql = @"INSERT into dbo.Tool_Moves1 (ToolNo, WC_From, WC_To, EmpNo) 
                            Values (ToolNo, WC_From, WC_To, EmpNo);";

            return SqlDataAccess.SaveData(sql, data);
        }

    }
}
