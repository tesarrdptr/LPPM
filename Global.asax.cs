﻿using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProposalPolmanAstra
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}