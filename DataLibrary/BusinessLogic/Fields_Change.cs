using DataLibrary.DataAccess;
using DataLibrary.Models;
using System.ComponentModel.Design;

namespace DataLibrary.BusinessLogic
{
    // Intended to be the class to manipulate some of the fields
    public class Fields_Change
    {
        // TODO: just make this into several files

        // Creates a new tool with it being active,
        // does not verify format nor does it add a locations
        // Used by Measure
        public static int CreateTool(string ToolNo)
        {
            ToolDB data = new ToolDB
            {
                Tool_ID = ToolNo,
                Active = 1
            };

            string sql = @"INSERT into dbo.Gage_List_Main (Tool_ID, Active) 
                            Values (@Tool_ID, @Active);";

            return SqlDataAccess.SaveData(sql, data);
        }

        // Finds the ID of the tool, if it exists
        // Used by Measure
        public static int FindToolID(string ToolNo)
        {

            string sql = @"SELECT ID FROM dbo.Gage_List_Main  
                            WHERE Tool_ID='" + ToolNo + "';";

            return SqlDataAccess.LoadData<int>(sql)[0];
        }


        // Adds Location given a toolID, and a WCID
        // Used by Measure
        public static int AddLocation(int ToolID, int WCID)
        {

            Locations_DB data = new Locations_DB
            {
                Tool_ID = (ToolID),
                WC_ID = (WCID),
                Status = 1
            };

            string sql = @"INSERT INTO dbo.Tool_Locations1
                            (Tool_ID, WC_ID, Status) 
                            VALUES (@Tool_ID, @WC_ID, @Status)
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        // TODO implement: sets tool as inactive in the DB
        // Used by Measure
        public static int DeleteTool(string ToolNo)
        {
            throw new NotImplementedException();
        }

        // Find the wc ID given a WC
        public static int FindWCID(string WC)
        {

            string sql = @"SELECT ID FROM dbo.WorkCenter3  
                            WHERE Name='" + WC + "'" +
                            "AND Active=1;";

            return SqlDataAccess.LoadData<int>(sql)[0];
        }

        // Find the wc status given a WC even if inactive
        // Returns -1 if not found
        public static int WC_Exists(string WC)
        {

            string sql = @"SELECT Active FROM dbo.WorkCenter3  
                            WHERE Name='" + WC + "'" +
                            ";";

            List<int> returnValue = SqlDataAccess.LoadData<int>(sql);


            return (returnValue == null || returnValue.Count < 1 ? -1 : returnValue[0]);
        }

        // Insert new WC with active status
        // DL stands for data libary
        public static int AddWCDL(string Name, string Description, string WCUnder)
        {

            WC_DB data = new WC_DB
            {
                Name = Name,
                Description = Description,
                WCUnder = WCUnder,
                Active = 1
            };

            string sql = @"INSERT INTO dbo.WorkCenter3
                            (Name, Description, WCUnder, Active) 
                            VALUES (@Name, @Description, @WCUnder, @Active)
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        // Insert new employee with active status
        // DL stands for data libary
        public static int AddEmployeeDL(string Name, string Clock_Code)
        {

            Employee_DB data = new Employee_DB
            {
                Name = Name,
                Clock_Code = Clock_Code,
                Active = 1
            };

            string sql = @"INSERT INTO dbo.Employee1
                            (Name, Clock_Code, Active) 
                            VALUES (@Name, @Clock_Code, @Active)
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static WC_DB getWCDetails(int id)
        {
            string sql = @"SELECT Name, Description, WCUnder, Active
                            FROM dbo.WorkCenter3
                            WHERE ID=" + id  +
                            ";";

            return SqlDataAccess.LoadData<WC_DB>(sql)[0];
        }


        // Recieves type of list needed and executes sql code to return a list
        //      for dropdown lists
        // Used by Measure
        // TODO: remove all of the _alls into an new function
        public static List<List<string>> LoadFields_dbl_lst(string type)
        {
            string sql = "";

            if (type.Equals("EMP"))
            {
                sql = @"SELECT Name, Clock_Code
                            FROM dbo.Employee1
                            WHERE Active = 1;";

                return SqlDataAccess.LoadListData<List<string>>(sql, "Name", "Clock_Code");
            }
            else if (type.Equals("EMP_ALL"))
            {
                sql = @"SELECT Name, Clock_Code
                            FROM dbo.Employee1
                            ;";

                return SqlDataAccess.LoadListData<List<string>>(sql, "Name", "Clock_Code");
            }

            else if (type.Equals("WC_ALL"))
            {
                sql = @"SELECT Name, Description, WCUnder, ID, Active
                            FROM dbo.WorkCenter3                            
                            ORDER BY Name ASC;";

                return SqlDataAccess.LoadListData<List<string>>(sql, 
                    "Name",
                    "Description", "WCUnder", "ID", "Active");
            }

            else if (type.Equals("WC"))
            {
                sql = @"SELECT Name, Description, WCUnder, ID
                            FROM dbo.WorkCenter3
                            WHERE Active = 1
                            ORDER BY Name ASC;";

                return SqlDataAccess.LoadListData<List<string>>(sql,
                                                "Name", "Description", "WCUnder", "ID");
            }
            else if (type.Equals("TOOL"))
            {
                sql = @"SELECT ID, Tool_ID, Description
                            FROM dbo.Gage_List_Main
                            WHERE Active = 1
                            ORDER BY Tool_ID ASC;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "ID",
                                                "Tool_ID", "Description");

            }

            else if (type.Equals("TOOL_ALL"))
            {
                sql = @"SELECT ID, Tool_ID, Description, Active
                            FROM dbo.Gage_List_Main
                            ORDER BY Tool_ID ASC;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "ID",
                                                "Tool_ID", "Description", "Active");

            }

            else if (type.Equals("LOCATE"))
            {
                sql = @"SELECT Tool_ID, WC_ID
                            FROM dbo.Tool_Locations1
                            WHERE Status = 1
                            ;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "Tool_ID", "WC_ID");

            }
            else { throw new Exception("Incorrect type entered"); }

        }

        // Used to check if a WC can be deactivated
        public static bool CheckOutToolExists(string WCName)
        {
            string sql = @"SELECT ID
                            
                            FROM dbo.Tool_Moves1
                            WHERE Returned_Date is NULL
                            AND (WC_From like '" + WCName + "' " +
                            "OR WC_To like '" + WCName + "'); ";

            return SqlDataAccess.LoadData<int>(sql).Count > 0;
        }

        // Reactivates a work center
        public static int DeactivateWC(int id)
        {
            // Deactivates
            return activateWCHelper(id, 0);

        }

        // Reactivates a work center
        public static int ReactivateWC(int id)
        {
            // Deactivates
            return activateWCHelper(id, 1);

        }

        // Helper Function that helps make code reusable
        public static int activateWCHelper(int id, int active)
        {

            WC_DB data = new WC_DB
            {
                ID = id,
                Active = active,
            };

            string sql = @"UPDATE dbo.WorkCenter3
                            SET Active = @Active 
                            WHERE ID = @ID
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteWCToolLocation(int id)
        {

            var data = new WC_DB
            {
                ID=id,
            };

            string sql = @"DELETE FROM dbo.Tool_Locations1
                            WHERE WC_ID = @ID AND Status = 1 AND Borrowed = 0
                            ;";
                             

            return SqlDataAccess.SaveData(sql, data);
        }

        // Insert new tool with active status
        // DL stands for data libary
        public static int AddToolDL(string Tool_ID, string Description)
        {

            ToolDB data = new ToolDB
            {
                Tool_ID = Tool_ID,
                Description = Description,
                Active = 1
            };

            string sql = @"INSERT INTO dbo.Gage_List_Main
                            (Tool_ID, Description, Active) 
                            VALUES (@Tool_ID, @Description, @Active)
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }




        public static List<List<string>> getToolofWC (int id)
        {

            // build in Access, and you can copy and paste it in an access to view the relationship
            // basically gets WC ID and uses location to get all tool names and description
            string sql = @"SELECT dbo.Gage_List_Main.Tool_ID, dbo.Gage_List_Main.Description
                            FROM (dbo.Tool_Locations1 INNER JOIN dbo.Gage_List_Main ON dbo.Tool_Locations1.Tool_ID = dbo.Gage_List_Main.ID) INNER JOIN dbo.WorkCenter3 ON dbo.Tool_Locations1.WC_ID = dbo.WorkCenter3.ID
                            WHERE (((dbo.WorkCenter3.ID)="+ id + 
                            ") AND ((dbo.Gage_List_Main.Active)=1));";

            // Note: deactivated WC should not have any tools

            return SqlDataAccess.LoadListData<List<string>>(sql, "Tool_ID", "Description");

        }


    }
}
