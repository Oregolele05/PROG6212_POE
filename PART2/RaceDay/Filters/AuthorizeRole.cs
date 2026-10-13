using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.Filters
{
    public class AuthorizeRole : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public AuthorizeRole(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. Check if user is logged in (has a session)
            var userId = context.HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                context.Result = new ObjectResult(new { message = "Unauthorized: You must be logged in." })
                {
                    StatusCode = 401 // Unauthorized
                };
                return;
            }

            // 2. Check if the role matches
            var userRole = context.HttpContext.Session.GetString("UserRole");
            if (userRole != _requiredRole)
            {
                context.Result = new ObjectResult(new { message = "Forbidden: You do not have permission to perform this action." })
                {
                    StatusCode = 403 // Forbidden
                };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}