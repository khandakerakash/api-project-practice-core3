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
        }
        
        public static class StudentReport
        {
            public const string StudentDepartmentInfo = Base + "student-with-department-info";
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