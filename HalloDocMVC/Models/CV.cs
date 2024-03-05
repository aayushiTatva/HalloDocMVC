namespace HalloDocMVC.Models
{
    public static class CV
    {
        private static IHttpContextAccessor _httpContextAccessor;

        static CV()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        public static string? UserName()
        {
            string? UserName = null;
            if (_httpContextAccessor.HttpContext.Session.GetString("UserName") != null)
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("UserName").ToString();
            }
            return UserName;
        }
        public static string? LastName()
        {
            string? LastName = null;
            if (_httpContextAccessor.HttpContext.Session.GetString("LastName") != null)
            {
                LastName = _httpContextAccessor.HttpContext.Session.GetString("LastName").ToString();
            }
            return LastName;
        }

        public static string? UserID()
        {
            string? UserID = null;

            if (_httpContextAccessor.HttpContext.Session.GetString("UserID") != null)
            {
                UserID = _httpContextAccessor.HttpContext.Session.GetString("UserID");

            }
            return UserID;
        }

    }
}
