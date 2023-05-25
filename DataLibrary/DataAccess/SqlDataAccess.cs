using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    internal class SqlDataAccess
    {
        public static string GetConnectionString(string connectionName = "NCPR_PROG1")
        {
            return "Data Source = DATALINKSRVR\\SQLEXPRESS; Initial Catalog = NC_PROG; Integrated Security = True";
        }

        public static List<T> LoadData<T>(String sql)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Query<T>(sql).ToList();

            }
        }

        public static List<List<string>> LoadDictData<T>(string sql) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                // Use Query<T> to get an IEnumerable<T> of each row as an object
                var result = cnn.Query<dynamic>(sql).ToList();

                List<List<string>> EmplList = new List<List<string>>();

                // Get each row and but into a list
                foreach (var row in result)
                {
                    EmplList.Add(new List<string> { row.Name, row.Clock_Code });
                }
                return EmplList;
            }
        }

        public static List<List<string>> LoadWCData<T>(string sql) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                // Use Query<T> to get an IEnumerable<T> of each row as an object
                var result = cnn.Query<dynamic>(sql).ToList();

                List<List<string>> EmplList = new List<List<string>>();

                // Get each row and but into a list
                foreach (var row in result)
                {
                    EmplList.Add(new List<string> { row.Name, row.Description, row.WCUnder });
                }
                return EmplList;
            }
        }

        public static List<List<string>> LoadToolData<T>(string sql) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                // Use Query<T> to get an IEnumerable<T> of each row as an object
                var result = cnn.Query<dynamic>(sql).ToList();

                List<List<string>> EmplList = new List<List<string>>();

                // Get each row and but into a list
                foreach (var row in result)
                {
                    EmplList.Add(new List<string> { row.ID.ToString(), row.Tool_ID, row.Description }) ;
                }
                return EmplList;
            }
        }

        public static int SaveData<T>(String sql, T data)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return cnn.Execute(sql, data);
            }
        }

        public static int GetMax(string sqlMax)
        {
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                List<string> maxQuery = cnn.Query<string>(sqlMax).ToList();

                foreach (string Row in maxQuery)
                {
                    return int.Parse(Row);
                }
                return 0;

            }
        }


        public static int ReturnTool<T>(String sqlDelete, String sqlInsert, T data)
        {


            int returnStatus = 0;
            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                returnStatus = cnn.Execute(sqlDelete, data);
            }



            using (IDbConnection cnn = new SqlConnection(GetConnectionString()))
            {
                return returnStatus & cnn.Execute(sqlInsert, data);
            }


        }
    }
}
