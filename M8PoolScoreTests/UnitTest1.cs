using System;
using System.Diagnostics;
using M8ScoreLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M8PoolScoreTests {
	[TestClass]
	public class MatchTests {
		private Match mTestMatch;

		public MatchTests() {
			mTestMatch = new Match();
		}

		[TestMethod]
		public void UnplayedSet() {
			populateSets();

			mTestMatch.Sets[4].EnterSet(0, 0, 0, 0, false);
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();
			Assert.AreEqual(513, mTestMatch.MatchScore);
			Assert.AreEqual(531, mTestMatch.OpponentMatchScore);
		}

		[TestMethod]
		public void ForfeitSetTest() {
			//Regular Season
			mTestMatch.IsRegularSeason = true;
			//Scores to make the assert true
			mTestMatch.Sets[0].EnterSet(59, 41, 60, 43, true);
			mTestMatch.Sets[1].EnterSet(100, 73, 72, 77, false);
			mTestMatch.Sets[2].EnterSet(57, 40, 42, 50, false);
			mTestMatch.Sets[3].EnterSet(63, 55, 70, 32, true);
			mTestMatch.Sets[4].EnterSet(72, 70, 0, -1, true);
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();

			Assert.AreEqual(533, mTestMatch.MatchScore);
			Assert.AreEqual(531, mTestMatch.OpponentMatchScore);

			//Post Season
			mTestMatch.IsRegularSeason = false;
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();

			Assert.AreEqual(633, mTestMatch.MatchScore);
			Assert.AreEqual(531, mTestMatch.OpponentMatchScore);

			//We forfeit (post season)
			mTestMatch.Sets[4].EnterSet(72, 70, -1, 0, false);
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();

			Assert.AreEqual(383, mTestMatch.MatchScore);
			Assert.AreEqual(827, mTestMatch.OpponentMatchScore);

			//they forfeit, over the penalty (regular season)
			mTestMatch.Sets[4].EnterSet(72, 121, 0, -1, true);
			mTestMatch.IsRegularSeason = true;
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();

			Assert.AreEqual(533, mTestMatch.MatchScore);
			Assert.AreEqual(506, mTestMatch.OpponentMatchScore);

		}

		[TestMethod]
		public void NoHandicapScoreTest() {
			//Scores to make the assert true
			mTestMatch.Sets[0].EnterSet(59, 41, 60, 43, true);
			mTestMatch.Sets[1].EnterSet(100, 73, 72, 77, false);
			mTestMatch.Sets[2].EnterSet(57, 40, 42, 50, false);
			mTestMatch.Sets[3].EnterSet(63, -1, 55, 32, true);
			mTestMatch.Sets[4].EnterSet(-1, 70, 52, 47, true);
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();

			Assert.AreEqual(624, mTestMatch.MatchScore);
			Assert.AreEqual(629, mTestMatch.OpponentMatchScore);
		}

		[TestMethod]
		public void MatchTest() {
			populateSets();
			mTestMatch.CalculateMatch();
			mTestMatch.DetermineHoldThemTo();
			mTestMatch.DetermineMustGet();


			Debug.WriteLine(string.Format("Our Score: {0}", mTestMatch.MatchScore));
			Debug.WriteLine(string.Format("Oppponent Score: {0}", mTestMatch.OpponentMatchScore));

			ScenarioResult holdThemTo = mTestMatch.DetermineHoldThemTo();
			ScenarioResult mustGet = mTestMatch.DetermineMustGet();			
			
			Assert.AreEqual(566, mTestMatch.MatchScore);
			Assert.AreEqual(654, mTestMatch.OpponentMatchScore);
		}

		private void populateSets() {
			 
			//Scores to make the assert true
			mTestMatch.Sets[0].EnterSet(59, 41, 60, 43, true);
			mTestMatch.Sets[1].EnterSet(100, 73, 72, 77, false);
			mTestMatch.Sets[2].EnterSet(57, 40, 42, 50, false);
			mTestMatch.Sets[3].EnterSet(63, 55, 70, 32, true);
			mTestMatch.Sets[4].EnterSet(72, 70, 83, 77, true);



			//mTestMatch.Sets[0].EnterSet(59, 41, 60, 43, true);
			//mTestMatch.Sets[1].EnterSet(100, 73, 101, 62, true);
			//mTestMatch.Sets[2].EnterSet(57, 40, 65, 35, true);
			//mTestMatch.Sets[3].EnterSet(63, 55, 55, 63, false);
			//mTestMatch.Sets[4].EnterSet(45, 40, 60, 40, false);

			
		}
	}
}
