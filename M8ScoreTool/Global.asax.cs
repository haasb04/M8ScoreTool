using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using M8ScoreLibrary;

namespace M8ScoreTool
{
	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			//load matchsettings. Create settings file if doesnt exist
			if(!File.Exists(Server.MapPath(MatchSettings.PathString()))) {
				//create one
				MatchSettings settings = new MatchSettings();
				settings.Save(Server.MapPath(MatchSettings.PathString()));
			}
		}
	}
}
