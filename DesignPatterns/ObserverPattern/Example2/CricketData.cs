using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public class CricketData : IData
    {
        private int score, wickets;
        private float over;
        List<IDisplay> scoreDisplays;

        public CricketData()
        {
            this.scoreDisplays = new List<IDisplay>();
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
            this.NotifyDisplay();
        }

        public void RegisterDisplay(IDisplay display)
        {
            this.scoreDisplays.Add(display);
        }

        public void UnRegisterDisplay(IDisplay display)
        {
            this.scoreDisplays.Remove(display);
        }

        public void NotifyDisplay()
        {
            foreach (var scoreDisplay in this.scoreDisplays)
            {
                scoreDisplay.Update(this.score, this.score, this.over);
            }
        }
    }
}
