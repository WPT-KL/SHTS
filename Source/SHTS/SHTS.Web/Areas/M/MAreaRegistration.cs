﻿using System.Web.Mvc;

namespace Witbird.SHTS.Web.Areas.M
{
    public class MAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "M";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "M_default",
                "M/{controller}/{action}/{id}",
                new { controller = "Index", action = "Index", id = UrlParameter.Optional },
                constraints: null,
                namespaces: new string[] { "Witbird.SHTS.Web.Areas.M.Controllers" }
            );
        }
    }
}
