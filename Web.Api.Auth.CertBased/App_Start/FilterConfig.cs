﻿using System.Web;
using System.Web.Mvc;

namespace Web.Api.Auth.CertBased
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
