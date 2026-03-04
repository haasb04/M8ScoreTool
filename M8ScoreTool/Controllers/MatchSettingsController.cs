using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M8ScoreLibrary;

namespace M8ScoreTool.Controllers {
	public class MatchSettingsController : Controller {
		// GET: MatchSettings
		public ActionResult Index() {
			Match currentMatch = (Match)Session["Match"];
			MatchSettings settings = null;
			if(currentMatch != null) {
				settings = currentMatch.GetSettings();
			} else {
				settings = MatchSettings.LoadFromXml(MatchSettings.PathString());
			}
			if(settings == null) {
				settings = new MatchSettings();
			}

			return View(settings);
		}

		public ActionResult Edit(MatchSettings model) {
			//validate the model
			if(model == null) {
				throw new ArgumentNullException("model");
			}
			//save new settings
			model.Save(Server.MapPath(MatchSettings.PathString()));
			//update session settings
			Match currentMatch =(Match)Session["Match"];
			if(currentMatch != null) {
				currentMatch.ChangeSettings(model);
			}
			
			return RedirectToAction("Index", "Home");
		}
	}
}