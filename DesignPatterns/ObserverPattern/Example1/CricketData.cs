using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example1
{
    public class CricketData
    {
        private int score, wickets;
        private float over;

        private CurrentScoreDisplay currentScoreDisplay;
        private AverageScoreDisplay averageScoreDisplay;

        public CricketData(CurrentScoreDisplay currentScoreDisplay, AverageScoreDisplay averageScoreDisplay)
        {
            this.currentScoreDisplay = currentScoreDisplay;
            this.averageScoreDisplay = averageScoreDisplay;
        }

        private int GetCurrentScore()
        {
            return 50;
        }

        private int GetCurrentWickets()
        {
            return 5;
        }

        private float GetCurrentOver()
        {
            return 2.30f;
        }

        public void DataChanged()
        {
            this.score = this.GetCurrentScore();
            this.wickets = this.GetCurrentWickets();
            this.over = this.GetCurrentOver();
            this.currentScoreDisplay.Update(this.score, this.score, this.over);
            this.averageScoreDisplay.Update(this.score, this.score, this.over);
        }
    }
}
