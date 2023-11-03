using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary.DataAccess
{
    internal class SqlDataAccess
    {
        // For now it is where the connection string is kept
        // TODO : find a way to abstract this out to another project
        public static string GetConnectionString(string connectionName = "NCPR_PROG1")
        {
            return "Data Source = SQLSRVR; Initial Catalog = Tool_Tracking; Integrated Security = True";
        }

        // Loads data into an List of an unnamed object
        public static List<T> LoadData<T>(String sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();

            }
        }

        // Generic method that takes sql, and 1 or more parameters of the attributes it is looking for
        public static List<List<string>> LoadListData<T>(string sql, params string[] fields) where T : class
        {
            return LoadListDataImplemented<List<string>>(sql, null, fields);
        }

        // Generic method that takes sql, and 1 or more parameters of the attributes it is looking for
        public static List<List<string>> LoadListDataWithParams<T>(string sql, DynamicParameters? sqlParams = null, params string[] fields) where T : class
        {

            return LoadListDataImplemented<List<string>>(sql, sqlParams, fields);

        }

        // Generic method that takes sql, and 1 or more parameters of the attributes it is looking for
        public static List<List<string>> LoadListDataImplemented<T>(string sql, DynamicParameters? sqlParams, params string[] fields) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                // Use Query<T> to get an IEnumerable<T> of each row as an object
                var result = cnn.Query<dynamic>(sql, sqlParams).ToList();
                List<List<string>> EmplList = new List<List<string>>();

                // Get each row and but into a list
                foreach (var row in result)
                {
                    List<string> values = new List<string>();

                    // Cast the row to a dictionary type
                    var rowDict = (IDictionary<string, object>)row;
                    foreach (var field in fields)
                    {
                        if (rowDict.ContainsKey(field))
                        {
                            // IF it cannot find column
                            if (rowDict[field] == null)
                            {
                                values.Add("");
                            }
                            else
                            {
                                values.Add(rowDict[field].ToString());
                            }

                        }
                        else
                        {
                            // Use a default value if the field key is not found
                            values.Add("N/A");
                        }
                    }
                    EmplList.Add(values);
                }
                return EmplList;
            }
        }

        // Executes sql code and returns the status
        public static int SaveData<T>(String sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }

    }
}
