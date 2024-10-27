﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Data.DataAccess;
using MultiVerse_UI.Models;

namespace MultiVerse_UI.Extensions
{
    public class CheckSessionExpirationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsSessionExpired())
            {
                string CurrentURL = StaticPublicObjects.ado.GetRequestPath();
                // Redirect to a SSO login page or perform any other action for session expiration
                filterContext.Result = new RedirectResult("~/Account/Login?RedirectURL=" + Crypto.EncryptString(CurrentURL, true));
            }

            base.OnActionExecuting(filterContext);
        }

        private bool IsSessionExpired()
        {
            IHttpContextAccessor _HttpContextAccessor = StaticPublicObjects.ado.GetIHttpContextAccessor();
            var _publicClaimObjects = _HttpContextAccessor.HttpContext.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
            bool isExpired = true;
            string? CurrentUserAdmin = _publicClaimObjects?.username;

            if (string.IsNullOrEmpty(CurrentUserAdmin) == false)
            {
                isExpired = false;
            }
            else
            {
                UserLogic_Security.SignInAjax();
                _publicClaimObjects = _HttpContextAccessor.HttpContext.Session.GetObject<PublicClaimObjects>("PublicClaimObjects");
                CurrentUserAdmin = _publicClaimObjects?.username;

                if (string.IsNullOrEmpty(CurrentUserAdmin) == false)
                {
                    isExpired = false;
                }
            }

            return isExpired;
        }
    }
}
