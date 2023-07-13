

using Microsoft.AspNetCore.Authorization;
using static DataLibrary.BusinessLogic.Fields_Change;

namespace ToolProgramCore.Policies.Requirements
{
    public class AdminRequirement : IAuthorizationRequirement
    {
        // constructor
        public AdminRequirement() =>
        AuthorizedUsers = getAuthorizedUsers();

        // get authorized user
        public List<string> AuthorizedUsers { get; }

        public List<string> getAuthorizedUsers()
        {

            

            // [username]
            List<List<string>> wrappedUsers = Load_Auth_List();

            List<string> authorizedUsers = new List<string>();

            foreach (List<string> innerList in  wrappedUsers)
            {
                authorizedUsers.Add(innerList[0].Trim());

            }

            return authorizedUsers;
        }
    }
}
