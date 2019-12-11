using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AdapterPattern
{
    public class BirdAdapter: IToyDuck
    {
        IBird bird;
        public BirdAdapter(IBird bird)
        {
            this.bird = bird;
        }

        public void squeak()
        {
            bird.makeSound();
        }
    }
}
