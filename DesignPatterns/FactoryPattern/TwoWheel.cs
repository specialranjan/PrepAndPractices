using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.FactoryPattern
{
    public class TwoWheel: Vehicle
    {
        private readonly string brand;
        private readonly string name;
        private readonly string model;

        public TwoWheel(string brand, string name, string model)
        {
            this.brand = brand;
            this.name = name;
            this.model = model;
        }
        public override string Brand { get { return this.brand; } }
        public override string Name { get { return this.name; } }
        public override string Model { get { return this.model; } }

        public override void Print()
        {
            Console.WriteLine("I am two Wheel Vehicle");
        }

    }
}
