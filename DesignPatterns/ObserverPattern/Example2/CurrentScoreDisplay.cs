using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public class CurrentScoreDisplay : IDisplay
    {
        private int score, wickets;
        private float over;
        public void Update(int score, int wickets, float over)
        {
            this.score = score;
            this.wickets = wickets;
            this.over = over;
            this.Display();
        }

        public void Display()
        {
            Console.WriteLine("Current score display:");
            Console.WriteLine("Score: {0}", this.score);
            Console.WriteLine("Wickets: {0}", this.wickets);
            Console.WriteLine("Over: {0}", this.over);
        }
    }
}
