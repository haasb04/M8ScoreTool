using Microsoft.AspNetCore.Mvc;
using M8ScoreLibrary;
using M8ScoreTool.Extensions;

namespace M8ScoreTool.Controllers {
	public class MatchSettingsController : Controller {
		private const string MatchSessionKey = "Match";
		private readonly IWebHostEnvironment _environment;

		public MatchSettingsController(IWebHostEnvironment environment) {
			_environment = environment;
		}

		// GET: MatchSettings
		public ActionResult Index() {
			Match currentMatch = HttpContext.Session.GetObject<Match>(MatchSessionKey);
			MatchSettings settings = null;
			if(currentMatch != null) {
				settings = currentMatch.GetSettings();
			} else {
				string settingsPath = Path.Combine(_environment.ContentRootPath, MatchSettings.PathString());
				settings = MatchSettings.LoadFromXml(settingsPath);
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
			string settingsPath = Path.Combine(_environment.ContentRootPath, MatchSettings.PathString());
			model.Save(settingsPath);
			//update session settings
			Match currentMatch = HttpContext.Session.GetObject<Match>(MatchSessionKey);
			if(currentMatch != null) {
				currentMatch.ChangeSettings(model);
				HttpContext.Session.SetObject(MatchSessionKey, currentMatch);
			}
			
			return RedirectToAction("Index", "Home");
		}
	}
}