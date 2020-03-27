namespace API.Utility
{
    public static class ApiRoutes
    {
        private const string Base = "";
        
        public static class Student
        {
            public const string GetOne = Base + "{id}";
            public const string Create = Base + "";
            public const string Update = Base + "{id}";
            public const string Delete = Base + "{id}";
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