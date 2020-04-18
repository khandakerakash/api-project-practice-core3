namespace Utility.Routes
{
    public class ApiRoutes
    {
        private const string Base = "";

        public static class Account
        {
            public const string Login = Base + "login";
            public const string Logout = Base + "logout";
            public const string Refresh = Base + "refresh";
        }
        
        public static class Student
        {
            public const string GetOne = Base + "{id}";
            public const string Create = Base + "";
            public const string Update = Base + "{id}";
            public const string Delete = Base + "{id}";
            public const string DepartmentWiseStudents = Base + "department-wise-students/{DepartmentId}";
        }
        
        public static class Department
        {
            public const string GetOne = Base + "{id}";
            public const string Create = Base + "";
            public const string Update = Base + "{id}";
            public const string Delete = Base + "{id}";
        }
    }
}