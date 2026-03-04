using Microsoft.AspNetCore.Mvc;
using M8ScoreLibrary;
using M8ScoreTool.Extensions;
using M8ScoreTool.Models;

namespace M8ScoreTool.Controllers {
	public class HomeController : Controller {
		private const string MatchSessionKey = "Match";
		private readonly IWebHostEnvironment _environment;

		public HomeController(IWebHostEnvironment environment) {
			_environment = environment;
		}

		public ActionResult Index() {
			Match match = HttpContext.Session.GetObject<Match>(MatchSessionKey);
			MatchSettings settings = null;

			if(settings == null) {
				string settingsPath = Path.Combine(_environment.ContentRootPath, MatchSettings.PathString());
				settings = MatchSettings.LoadFromXml(settingsPath);
				if(settings == null) {
					//there was an error. clean this up later.
					settings = new MatchSettings(); //defaults
				}
			}

			if(match == null) {
				match = new Match(settings);
				//match.Sets[0].EnterSet(59, 41, 60, 43, true);
				//match.Sets[1].EnterSet(100, 73, 72, 77, false);
				//match.Sets[2].EnterSet(57, 40, 42, 50, false);
				//match.Sets[3].EnterSet(63, 55, 70, 32, true);
				//match.Sets[4].EnterSet(72, 70, 83, 77, true);
				HttpContext.Session.SetObject(MatchSessionKey, match);

			}

			return View(new MatchViewModel(match));
		}

		[HttpPost]
		public ActionResult Edit(MatchViewModel model) {
			//update match and refresh the model
			Match match = HttpContext.Session.GetObject<Match>(MatchSessionKey);
			if(match == null) {
				match = new Match();
				HttpContext.Session.SetObject(MatchSessionKey, match);
			}

			match.Sets[0].Rate = model.Set1Rate;
			match.Sets[0].Score = model.Set1Score;
			match.Sets[0].OpponentRate = model.Set1ORate;
			match.Sets[0].OpponentScore = model.Set1OScore;
			match.Sets[0].Win = model.Set1Win;

			match.Sets[1].Rate = model.Set2Rate;
			match.Sets[1].Score = model.Set2Score;
			match.Sets[1].OpponentRate = model.Set2ORate;
			match.Sets[1].OpponentScore = model.Set2OScore;
			match.Sets[1].Win = model.Set2Win;

			match.Sets[2].Rate = model.Set3Rate;
			match.Sets[2].Score = model.Set3Score;
			match.Sets[2].OpponentRate = model.Set3ORate;
			match.Sets[2].OpponentScore = model.Set3OScore;
			match.Sets[2].Win = model.Set3Win;

			match.Sets[3].Rate = model.Set4Rate;
			match.Sets[3].Score = model.Set4Score;
			match.Sets[3].OpponentRate = model.Set4ORate;
			match.Sets[3].OpponentScore = model.Set4OScore;
			match.Sets[3].Win = model.Set4Win;

			match.Sets[4].Rate = model.Set5Rate;
			match.Sets[4].Score = model.Set5Score;
			match.Sets[4].OpponentRate = model.Set5ORate;
			match.Sets[4].OpponentScore = model.Set5OScore;
			match.Sets[4].Win = model.Set5Win;

			HttpContext.Session.SetObject(MatchSessionKey, match);

			return RedirectToAction("Index");
		}

		public ActionResult ReportView() {
			Match model = HttpContext.Session.GetObject<Match>(MatchSessionKey);
			if(model == null) {
				return RedirectToAction("Index");
			}
			model.CalculateMatch();
			return View(model);
		}

		public ActionResult About() {
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}

	}
}