using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M8ScoreLibrary {
	internal class SetResult {
		public int Score { get; set; }
		public int OpponentScore { get; set; }

		public SetResult(int score, int opponentScore) {
			Score = score;
			OpponentScore = opponentScore;
		}
	}
}
