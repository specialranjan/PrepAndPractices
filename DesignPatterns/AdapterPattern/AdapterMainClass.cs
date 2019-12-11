using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.AdapterPattern
{
    public class AdapterMainClass
    {
        public static void MainMethod()
        {
            Sparrow sparrow = new Sparrow();
            Console.WriteLine("Sparrow...");
            sparrow.fly();
            sparrow.makeSound();
            
            IToyDuck toyDuck = new PlasticToyDuck();
            Console.WriteLine("ToyDuck...");
            toyDuck.squeak();

            // Wrap a bird in a birdAdapter so that it  
            // behaves like toy duck 
            IToyDuck birdAdapter = new BirdAdapter(sparrow);

            // toy duck behaving like a bird  
            Console.WriteLine("BirdAdapter...");
            birdAdapter.squeak();
        }
    }
}
