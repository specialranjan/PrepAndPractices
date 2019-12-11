using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public class AverageScoreDisplay : IDisplay
    {
        private float runRate;
        private int predictedScore;

        public void Update(int score, int wickets, float over)
        {
            this.runRate = (float)score / over;
            this.predictedScore = (int)this.runRate * 50;
            this.Display();
        }

        public void Display()
        {
            Console.WriteLine("Average score display:");
            Console.WriteLine("Run Rate: {0}", this.runRate);
            Console.WriteLine("Predicted Score: {0}", this.predictedScore);
        }
    }
}
