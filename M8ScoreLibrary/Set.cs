using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M8ScoreLibrary {
	[Serializable]
	public class Set {
		public int SetNumber { get; private set; }
		public int Rate { get; set; }
		public int OpponentRate { get; set; }
		public int Score { get; set; }
		//public int OpponentScore { get; set; }
		public int WinBonus { get; set; }
		public int OpponentWinBonus { get; set; }
		public bool Win { get; set; }
		public bool IsRegularSeason { get; set; }
		private int mOpponentScore;
		public int OpponentScore {
			get { return mOpponentScore; }
			set { mOpponentScore = value; }
		}

		public Set() { }
		internal Set(int setNumber, bool isRegularSeason) {
			SetNumber = setNumber;
			IsRegularSeason = isRegularSeason;
		}
		public void EnterSet(int rate, int opponentRate, int score, int opponentScore, bool win) {
			Rate = rate;
			OpponentRate = opponentRate;
			Score = score;
			OpponentScore = opponentScore;
			Win = win;
		}

		internal SetResult CalculateScore(int multiplier) {
			int score = Score;
			int opponentScore = OpponentScore;
			int effectiveRate = Rate;
			int effectiveOpponentRate = OpponentRate;
			bool noRate = false;
			bool noOpponentRate = false;
			mMultiplier = multiplier;

			//Forfeited match.
			bool forfeit = false;
			if(Score == -1) {
				//we forfeit
				//regular season - 150 Points, postseason - 250 Points
				opponentScore = (IsRegularSeason ? 150 : 250);
				score = 0;
				forfeit = true;
			}
			if(OpponentScore == -1) {
				//they forfeit
				//regular season - 150 Points, postseason - 250 Points
				score = (IsRegularSeason ? 150 : 250);
				opponentScore = 0;
				forfeit = true;
			}
			if(forfeit) {
				return new SetResult(score, opponentScore);
			}

			//Unrated players. Race to 50.
			if(effectiveRate == -1) {
				effectiveRate = 50;
				effectiveOpponentRate = 50;
				noRate = true;
			}
			if(effectiveOpponentRate == -1) {
				effectiveOpponentRate = 50;
				effectiveRate = 50;
				noOpponentRate = true;
			}
			mMargin = 0;
			mOpponentMargin = 0;
			WinBonus = OpponentWinBonus = 0;
			if(Win) {
				if(Score > 0) {
					score += 100;
					WinBonus = 100;
				}
				int delta;
				if(noOpponentRate) {
					delta = 50 - OpponentScore;
				} else {
					delta = effectiveOpponentRate - OpponentScore;
				}
				if(delta > 0) {
					score += (delta * multiplier);
					mMargin = delta;
				}
			} else {
				if(OpponentScore > 0) {
					opponentScore += 100;
					OpponentWinBonus = 100;
				}
				int delta;
				if(noRate) {
					delta = 50 - Score;
				} else {
					delta = effectiveRate - Score;
				}
				if(delta > 0) {
					opponentScore += (delta * multiplier);
					mOpponentMargin = delta;
				}
			}
			return new SetResult(score, opponentScore);
		}
		private int mMargin;
		private int mMultiplier;
		private int mOpponentMargin;


		public int Margin {
			get {
				return mMargin;
			}
		}

		public int OpponentMargin {
			get { return mOpponentMargin; }
		}
	
		public int AddOn {
			get {
				return mMargin * mMultiplier;
			}
		}

		public int OpponentAddOn {
			get {
				return mOpponentMargin * mMultiplier;
			}
		}

		public int SetTotal {
			get {
				return Score + AddOn + WinBonus;
			}
		}

		public int OpponentSetTotal {
			get {
				return OpponentScore + OpponentAddOn + OpponentWinBonus;
			}
		}
		
	}
}
