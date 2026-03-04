using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using M8ScoreLibrary;

namespace M8ScoreTool.Models {
	public class MatchViewModel {
		public MatchViewModel() {

		}
		public MatchViewModel(Match match) {
			match.CalculateMatch();

			Set1Rate = match.Sets[0].Rate;
			Set1Score = match.Sets[0].Score;
			Set1ORate = match.Sets[0].OpponentRate;
			Set1OScore = match.Sets[0].OpponentScore;
			Set1Win = match.Sets[0].Win;

			Set2Rate = match.Sets[1].Rate;
			Set2Score = match.Sets[1].Score;
			Set2ORate = match.Sets[1].OpponentRate;
			Set2OScore = match.Sets[1].OpponentScore;
			Set2Win = match.Sets[1].Win;

			Set3Rate = match.Sets[2].Rate;
			Set3Score = match.Sets[2].Score;
			Set3ORate = match.Sets[2].OpponentRate;
			Set3OScore = match.Sets[2].OpponentScore;
			Set3Win = match.Sets[2].Win;

			Set4Rate = match.Sets[3].Rate;
			Set4Score = match.Sets[3].Score;
			Set4ORate = match.Sets[3].OpponentRate;
			Set4OScore = match.Sets[3].OpponentScore;
			Set4Win = match.Sets[3].Win;

			Set5Rate = match.Sets[4].Rate;
			Set5Score = match.Sets[4].Score;
			Set5ORate = match.Sets[4].OpponentRate;
			Set5OScore = match.Sets[4].OpponentScore;
			Set5Win = match.Sets[4].Win;

			TotalRate = match.TotalRate;
			TotalORate = match.OpponentTotalRate;

			MatchOverUnder = match.TeamBonusOrPenalty;
			MatchOOverUnder = match.OpponentTeamBonusOrPenalty;

			MatchScore = match.MatchScore;
			MatchOScore = match.OpponentMatchScore;

			MatchFiveWin = match.DetermineHoldThemTo();
			MatchFiveLose = match.DetermineMustGet();
		}

		public int Set1Rate { get; set; }
		public int Set1Score { get; set; }
		public int Set1ORate { get; set; }
		public int Set1OScore { get; set; }
		public bool Set1Win { get; set; }

		public int Set2Rate { get; set; }
		public int Set2Score { get; set; }
		public int Set2ORate { get; set; }
		public int Set2OScore { get; set; }
		public bool Set2Win { get; set; }

		public int Set3Rate { get; set; }
		public int Set3Score { get; set; }
		public int Set3ORate { get; set; }
		public int Set3OScore { get; set; }
		public bool Set3Win { get; set; }

		public int Set4Rate { get; set; }
		public int Set4Score { get; set; }
		public int Set4ORate { get; set; }
		public int Set4OScore { get; set; }
		public bool Set4Win { get; set; }

		public int Set5Rate { get; set; }
		public int Set5Score { get; set; }
		public int Set5ORate { get; set; }
		public int Set5OScore { get; set; }
		public bool Set5Win { get; set; }

		public int TotalRate { get; set; }
		public int TotalORate { get; set; }

		public int MatchOverUnder { get; set; }
		public int MatchOOverUnder { get; set; }

		public int MatchScore { get; set; }
		public int MatchOScore { get; set; }

		public ScenarioResult MatchFiveWin { get; set; }
		public ScenarioResult MatchFiveLose { get; set; }

	}
}