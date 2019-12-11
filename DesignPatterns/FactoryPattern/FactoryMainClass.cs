using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.FactoryPattern
{
    public class FactoryMainClass
    {
        VehicleFactory vehicleFactory;

        public void MainMethod()
        {
            VehicleFactory vehicleFactory = new VehicleFactory(VehicleFactory.VehicleType.TwoWheeler, "Honda", "CB100", "Auto150CC");
            Vehicle vehicle = vehicleFactory.GetVehicle();
            vehicle.Print();
        }
    }
}
