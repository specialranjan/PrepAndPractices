using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example1
{
    public class ObserverMainClass
    {
        private CurrentScoreDisplay currentScoreDisplay;
        private AverageScoreDisplay averageScoreDisplay;

        public void MainMethod()
        {
            this.currentScoreDisplay = new CurrentScoreDisplay();
            this.averageScoreDisplay = new AverageScoreDisplay();

            CricketData cricketData = new CricketData(currentScoreDisplay, averageScoreDisplay);
            cricketData.DataChanged();
        }
    }
}
