using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public class ObserverMainClass
    {
        public void MainMethod()
        {
            var currentScoreDisplay = new CurrentScoreDisplay();
            var averageScoreDisplay = new AverageScoreDisplay();
            Example2.CricketData cricketData = new Example2.CricketData();
            cricketData.RegisterDisplay(currentScoreDisplay);
            cricketData.RegisterDisplay(averageScoreDisplay);
            cricketData.DataChanged();
            cricketData.UnRegisterDisplay(averageScoreDisplay);
            cricketData.DataChanged();
        }
    }
}
