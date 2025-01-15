namespace OrderSystemApp.Utils
{
    public class AuthenticateSession
    {
        private readonly HttpContext _httpContext;

        public AuthenticateSession(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public bool AuthenticateUserSession()
        {
            var currentUser = _httpContext.Session.GetString("CurrentUserId");
            return currentUser is not null;
        }
    }
}
