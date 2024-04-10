using Dapper;
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

        // Finds the ID of the tool, if it exists in locations
        // Used by Measure
        public static bool ToolLocationExist(string Tool_ID)
        {

            string sql = @"SELECT WC_ID FROM dbo.Tool_Locations1 
                            WHERE Tool_ID='" + Tool_ID + "';";

            return SqlDataAccess.LoadData<int>(sql).Count != 0;
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
                            WHERE ID=" + id +
                            ";";

            return SqlDataAccess.LoadData<WC_DB>(sql)[0];
        }

        public static ToolDB getToolDetails(int id)
        {
            string sql = @"SELECT Tool_ID, Description, Active
                            FROM dbo.Gage_List_Main
                            WHERE ID=" + id +
                            ";";

            return SqlDataAccess.LoadData<ToolDB>(sql)[0];
        }

        // Get all the tool details including all WC and if they are borrowed or not
        public static List<List<string>> getToolWCDetails(int id)
        {
            string sql = @"SELECT ID_T, ID_W, WC, Tool_ID, Status, Borrowed
                            FROM dbo.View_Tool_WorkCenters
                            WHERE ID_T = " + id +
                            " " +
                            "  ORDER BY Status ASC;";

            return SqlDataAccess.LoadListData<List<string>>(sql, "ID_T",
                "ID_W", "WC", "Tool_ID", "Status", "Borrowed");
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

            else if (type.Equals("POWER_USERS"))
            {
                sql = @"SELECT UserName, Date_Changed, Access, UpdatedBy, SuperAdmin
                            FROM dbo.Admin_Authorization
                            ;";
                return SqlDataAccess.LoadListData<List<string>>(sql, "UserName",
                                                "Date_Changed", "Access", "UpdatedBy"
                                                , "SuperAdmin");

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

        public static bool userSuperAdmin(string UserName)
        {
            

            string sql = @"SELECT UserName
                        FROM dbo.Admin_Authorization
                        WHERE UserName = @UserName
                        AND SuperAdmin = 1
                        ;";

            DynamicParameters sqlParams = new DynamicParameters();
            sqlParams.Add("UserName", UserName);


            List<List<string>> superAdminLookUp = SqlDataAccess.LoadListDataWithParams<List<string>>(
                sql,
                sqlParams,
                "UserName",
                                            "Date_Changed", "Access", "UpdatedBy"
                                            , "SuperAdmin");
            if (superAdminLookUp.Count > 0) 
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static int AddNewUser(string userName, string updatedBy,
                                                bool superAdmin)
        {

            var data = new 
            {
                UserName = userName,
                UpdatedBy = updatedBy,
                SuperAdmin = superAdmin

            };

            string sql = @"INSERT INTO
                        dbo.Admin_Authorization
                        (UserName, Access, UpdatedBy, SuperAdmin)
                        VALUES(@UserName, 1, @UpdatedBy, @SuperAdmin)
                        ;";


            return SqlDataAccess.SaveData(sql, data);
        }

        //TODO : create the description
        public static int RemoveUser(string userName, string curUser)
        {

            var data = new
            {
                UserName = userName,
                curUser,
                today = DateTime.Now,

                
            };

            string sql = @"UPDATE
                        dbo.Admin_Authorization
                        SET Access = 0, UpdatedBy = @curUser,
                        Date_Changed = @today
                        WHERE UserName = @userName
                        ;";


            return SqlDataAccess.SaveData(sql, data);
        }

        //TODO : create the description
        // Repeated code
        public static int GiveBackAccess(string userName, string curUser)
        {

            var data = new
            {
                UserName = userName
                , CurUser = curUser,
                today = DateTime.Now
            };

            string sql = @"UPDATE
                        dbo.Admin_Authorization
                        SET Access = 1, UpdatedBy = @curUser,
                        Date_Changed = @today
                        WHERE UserName = @userName
                        ;";


            return SqlDataAccess.SaveData(sql, data);
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
                ID = id,
            };

            string sql = @"DELETE FROM dbo.Tool_Locations1
                            WHERE WC_ID = @ID AND Status = 1 AND Borrowed = 0
                            ;";


            return SqlDataAccess.SaveData(sql, data);
        }

        public static int DeleteWCToolIDLocation(int id)
        {

            var data = new ToolDB
            {
                ID = id,
            };

            string sql = @"DELETE FROM dbo.Tool_Locations1
                            WHERE Tool_ID = @ID
                            ;";


            return SqlDataAccess.SaveData(sql, data);
        }

        public static int AddWCToolIDLocation(int W_ID, int T_ID)
        {

            var data = new ToolDB
            {
                ID = T_ID,
                WC_ID = W_ID,

            };

            string sql = @"INSERT INTO dbo.Tool_Locations1
                            (Tool_ID, WC_ID, Status, Borrowed)
                             VALUES (@ID, @WC_ID, 1, 0)
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


        // Update tool
        // DL stands for data libary
        public static int UpdateToolDL(int id, string Description)
        {

            ToolDB data = new ToolDB
            {
                Description = Description,
                Active = 1,
                ID = id
            };

            string sql = @"UPDATE dbo.Gage_List_Main
                            SET Description = @Description, 
                            Active = @Active 
                            WHERE ID = @ID
                          
                             ;";

            return SqlDataAccess.SaveData(sql, data);
        }

        public static List<List<string>> getToolofWC(int id)
        {

            // build in Access, and you can copy and paste it in an access to view the relationship
            // basically gets WC ID and uses location to get all tool names and description
            string sql = @"SELECT dbo.Gage_List_Main.Tool_ID, dbo.Gage_List_Main.Description
                            FROM (dbo.Tool_Locations1 INNER JOIN dbo.Gage_List_Main ON dbo.Tool_Locations1.Tool_ID = dbo.Gage_List_Main.ID) INNER JOIN dbo.WorkCenter3 ON dbo.Tool_Locations1.WC_ID = dbo.WorkCenter3.ID
                            WHERE (((dbo.WorkCenter3.ID)=" + id +
                            ") AND ((dbo.Gage_List_Main.Active)=1));";

            // Note: deactivated WC should not have any tools

            return SqlDataAccess.LoadListData<List<string>>(sql, "Tool_ID", "Description");

        }

        public static WC_DB findWCByToolID(string Name)
        {
            string sql = @"SELECT ID
                            FROM dbo.WorkCenter3
                            WHERE Name='" + Name +
                            "';";
            return SqlDataAccess.LoadData<WC_DB>(sql)[0];
        }

        public static List<List<string>> Load_Auth_List()
        {
            string sql = "";

            sql = @"SELECT UserName
                        FROM dbo.Admin_Authorization
                        WHERE Access = 1;";

            return SqlDataAccess.LoadListData<List<string>>(sql, "UserName");

        }
    }
}
