using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.ObserverPattern.Example2
{
    public interface IDisplay
    {
        void Update(int score, int wickets, float over);
    }
}
