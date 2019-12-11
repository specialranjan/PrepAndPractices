using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AdapterPattern
{
    public class PlasticToyDuck: IToyDuck
    {
        public void squeak()
        {
            Console.WriteLine("Squeak");
        }
    }
}
