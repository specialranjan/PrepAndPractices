using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.FactoryPattern
{
    public class VehicleFactory
    {
        public enum VehicleType
        {
            TwoWheeler, 
            FourWheel 
        }

        private Vehicle vehicle;

        public VehicleFactory(VehicleType vehicleType, string brand, string name, string model)
        {
            if (vehicleType == VehicleType.TwoWheeler)
            {
                vehicle = new TwoWheel(brand, name, model);
            }
            else
            {
                vehicle = new FourWheel(brand, name, model);
            }
        }
        public Vehicle GetVehicle()
        {
            return this.vehicle;
        }
    }
}
