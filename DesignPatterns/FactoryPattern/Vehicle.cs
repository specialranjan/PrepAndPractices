namespace DesignPatterns.FactoryPattern
{
    public abstract class Vehicle
    {
        public abstract string Brand { get; }
        public abstract string Name { get; }
        public abstract string Model { get; }
        public abstract void Print();
    }    
}
