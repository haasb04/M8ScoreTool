using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace M8ScoreLibrary
{
    [Serializable]
    public class Match
    {
        public int WinBonusValue { get; set; }
        public int Multiplier { get; set; }
        public int PenaltyMultiplier { get; set; }
        public int OverUnderLimit { get; set; }
        public int TeamBonusOrPenalty { get; set; }
        public int OpponentTeamBonusOrPenalty { get; set; }
        public int TotalRate { get; set; }
        public int OpponentTotalRate { get; set; }
        public List<Set> Sets { get; set; }
        private bool mIsRegularSeason;
        public bool IsRegularSeason
        {
            get { return mIsRegularSeason; }
            set
            {
                if (mIsRegularSeason != value)
                {
                    mIsRegularSeason = value;
                    if (Sets != null)
                    {
                        foreach (Set s in Sets)
                        {
                            s.IsRegularSeason = mIsRegularSeason;
                        }
                    }
                }
            }
        }

        private int mMatchScore;
        private int mOpponentMatchScore;

        public Match()
            : this(new MatchSettings()) { }

        public Match(MatchSettings settings)
        {
            ChangeSettings(settings);

            Sets = new List<Set>();

            for (int i = 0; i < 5; i++)
            {
                Sets.Add(new Set(i + 1, IsRegularSeason));
            }
        }

        //Managing settings this way is hokey.
        //Should be refactored at some point.

        public void ChangeSettings(MatchSettings settings)
        {
            Multiplier = settings.WinMultiplier;
            OverUnderLimit = settings.OverUnderLimit;
            PenaltyMultiplier = settings.PenaltyMultiplier;
            IsRegularSeason = settings.IsRegularSeason;
        }

        public MatchSettings GetSettings()
        {
            MatchSettings settings = new MatchSettings();
            settings.WinMultiplier = Multiplier;
            settings.OverUnderLimit = OverUnderLimit;
            settings.PenaltyMultiplier = PenaltyMultiplier;
            settings.IsRegularSeason = IsRegularSeason;

            return settings;
        }

        public void CalculateMatch()
        {
            //Sum all sets
            int totalPlayerRatings = 0;
            int totalOpponentPlayerRatings = 0;
            mMatchScore = mOpponentMatchScore = 0;
            bool weForfeit = false;
            bool theyForfeit = false;
            bool unplayedSet = false;

            foreach (Set s in Sets)
            {
                SetResult results = s.CalculateScore(Multiplier);
                weForfeit = weForfeit || s.Score == -1;
                theyForfeit = theyForfeit || s.OpponentScore == -1;
                mMatchScore += results.Score;
                mOpponentMatchScore += results.OpponentScore;
                unplayedSet = unplayedSet || (s.Rate == 0 && s.OpponentRate == 0);
                totalPlayerRatings += (s.Rate == -1 ? 50 : s.Rate);
                totalOpponentPlayerRatings += (s.OpponentRate == -1 ? 50 : s.OpponentRate);
            }

            //calculatae over/under
            //no bonus awarded to either team if a set is unplayed.
            TotalRate = totalPlayerRatings;
            OpponentTotalRate = totalOpponentPlayerRatings;
            if (!unplayedSet)
            {
                if (totalPlayerRatings > OverUnderLimit)
                {
                    TeamBonusOrPenalty = -(totalPlayerRatings - OverUnderLimit) * PenaltyMultiplier;
                }
                else
                {
                    TeamBonusOrPenalty = weForfeit ? 0 : (OverUnderLimit - totalPlayerRatings);
                }

                mMatchScore += TeamBonusOrPenalty;

                if (totalOpponentPlayerRatings > OverUnderLimit)
                {
                    OpponentTeamBonusOrPenalty =
                        -(totalOpponentPlayerRatings - OverUnderLimit) * PenaltyMultiplier;
                }
                else
                {
                    OpponentTeamBonusOrPenalty = theyForfeit
                        ? 0
                        : (OverUnderLimit - totalOpponentPlayerRatings);
                }

                mOpponentMatchScore += OpponentTeamBonusOrPenalty;
            }
        }

        public int MatchScore
        {
            get { return mMatchScore; }
        }

        public int OpponentMatchScore
        {
            get { return mOpponentMatchScore; }
        }

        public ScenarioResult DetermineMustGet()
        {
            //clone the match
            //set fifth match score to rate and set the win flag
            //for opponent score = 0; oppponent score < their rate +14
            //if their match score > our match score
            //we lose.  Must hold opponent to one less than this ball.
            ScenarioResult retVal = new ScenarioResult();
            int mustGetScore = 0;

            Match testMatch = Clone();

            int effectiveRate;
            int effectiveOpponentRate;

            if (testMatch.Sets[4].Rate == -1 || testMatch.Sets[4].OpponentRate == -1)
            {
                effectiveRate = effectiveOpponentRate = 50;
            }
            else
            {
                effectiveRate = testMatch.Sets[4].Rate;
                effectiveOpponentRate = testMatch.Sets[4].OpponentRate;
            }

            retVal.Rate = effectiveOpponentRate;
            for (int i = 0; i < 15; i++)
            {
                testMatch.Sets[4].OpponentScore = effectiveOpponentRate + i;

                testMatch.Sets[4].Win = false;
                mustGetScore = 0;
                for (int myScore = 0; myScore < effectiveRate + 15; myScore++)
                {
                    testMatch.Sets[4].Score = myScore;
                    testMatch.CalculateMatch();
                    if (testMatch.OpponentMatchScore < testMatch.MatchScore)
                    {
                        mustGetScore = myScore;
                        break;
                    }
                }
                switch (i)
                {
                    case 0:
                        retVal.RatePlusNone = mustGetScore;
                        break;
                    case 1:
                        retVal.RatePlusOne = mustGetScore;
                        break;
                    case 2:
                        retVal.RatePlusTwo = mustGetScore;
                        break;
                    case 3:
                        retVal.RatePlusThree = mustGetScore;
                        break;
                    case 4:
                        retVal.RatePlusFour = mustGetScore;
                        break;
                    case 5:
                        retVal.RatePlusFive = mustGetScore;
                        break;
                    case 6:
                        retVal.RatePlusSix = mustGetScore;
                        break;
                    case 7:
                        retVal.RatePlusSeven = mustGetScore;
                        break;
                    case 8:
                        retVal.RatePlusEight = mustGetScore;
                        break;
                    case 9:
                        retVal.RatePlusNine = mustGetScore;
                        break;
                    case 10:
                        retVal.RatePlusTen = mustGetScore;
                        break;
                    case 11:
                        retVal.RatePlusEleven = mustGetScore;
                        break;
                    case 12:
                        retVal.RatePlusTwelve = mustGetScore;
                        break;
                    case 13:
                        retVal.RatePlusThirteen = mustGetScore;
                        break;
                    case 14:
                        retVal.RatePlusFourteen = mustGetScore;
                        break;
                }
                Debug.WriteLine(
                    string.Format(
                        "Opponent Score: {0}, must get to: {1}",
                        testMatch.Sets[4].OpponentScore,
                        mustGetScore
                    )
                );
            }

            return retVal;
        }

        public ScenarioResult DetermineHoldThemTo()
        {
            //clone the match
            //set fifth match score to rate and set the win flag
            //for opponent score = 0; oppponent score < their rate +14
            //if their match score > our match score
            //we lose.  Must hold opponent to one less than this ball.
            ScenarioResult retVal = new ScenarioResult();
            int maxOpponentScore = 0;

            //Match testMatch = (Match)this.MemberwiseClone();
            Match testMatch = Clone();
            int effectiveRate;
            int effectiveOpponentRate;

            if (testMatch.Sets[4].Rate == -1 || testMatch.Sets[4].OpponentRate == -1)
            {
                effectiveRate = effectiveOpponentRate = 50;
            }
            else
            {
                effectiveRate = testMatch.Sets[4].Rate;
                effectiveOpponentRate = testMatch.Sets[4].OpponentRate;
            }

            retVal.Rate = effectiveRate;
            for (int i = 0; i < 15; i++)
            {
                testMatch.Sets[4].Score = effectiveRate + i;

                testMatch.Sets[4].Win = true;
                maxOpponentScore = 0;
                for (int oScore = 0; oScore < effectiveOpponentRate + 14; oScore++)
                {
                    testMatch.Sets[4].OpponentScore = oScore;
                    testMatch.CalculateMatch();
                    if (testMatch.OpponentMatchScore >= testMatch.MatchScore)
                    {
                        maxOpponentScore = oScore - 1;
                        break;
                    }
                }

                switch (i)
                {
                    case 0:
                        retVal.RatePlusNone = maxOpponentScore;
                        break;
                    case 1:
                        retVal.RatePlusOne = maxOpponentScore;
                        break;
                    case 2:
                        retVal.RatePlusTwo = maxOpponentScore;
                        break;
                    case 3:
                        retVal.RatePlusThree = maxOpponentScore;
                        break;
                    case 4:
                        retVal.RatePlusFour = maxOpponentScore;
                        break;
                    case 5:
                        retVal.RatePlusFive = maxOpponentScore;
                        break;
                    case 6:
                        retVal.RatePlusSix = maxOpponentScore;
                        break;
                    case 7:
                        retVal.RatePlusSeven = maxOpponentScore;
                        break;
                    case 8:
                        retVal.RatePlusEight = maxOpponentScore;
                        break;
                    case 9:
                        retVal.RatePlusNine = maxOpponentScore;
                        break;
                    case 10:
                        retVal.RatePlusTen = maxOpponentScore;
                        break;
                    case 11:
                        retVal.RatePlusEleven = maxOpponentScore;
                        break;
                    case 12:
                        retVal.RatePlusTwelve = maxOpponentScore;
                        break;
                    case 13:
                        retVal.RatePlusThirteen = maxOpponentScore;
                        break;
                    case 14:
                        retVal.RatePlusFourteen = maxOpponentScore;
                        break;
                }
                Debug.WriteLine(
                    string.Format(
                        "Score: {0}, must hold opponent to: {1}",
                        testMatch.Sets[4].Score,
                        maxOpponentScore
                    )
                );
            }

            return retVal;
        }

        private Match Clone()
        {
            MatchSettings settings = GetSettings();
            Match clone = new Match(settings);
            clone.TeamBonusOrPenalty = TeamBonusOrPenalty;
            clone.OpponentTeamBonusOrPenalty = OpponentTeamBonusOrPenalty;
            clone.TotalRate = TotalRate;
            clone.OpponentTotalRate = OpponentTotalRate;

            for (int i = 0; i < Sets.Count && i < clone.Sets.Count; i++)
            {
                clone.Sets[i].Rate = Sets[i].Rate;
                clone.Sets[i].OpponentRate = Sets[i].OpponentRate;
                clone.Sets[i].Score = Sets[i].Score;
                clone.Sets[i].OpponentScore = Sets[i].OpponentScore;
                clone.Sets[i].Win = Sets[i].Win;
            }

            clone.CalculateMatch();
            return clone;
        }
    }
}
