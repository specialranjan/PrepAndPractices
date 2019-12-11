using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SingletonPattern
{
    public class SingletonMainClass
    {
        public void MainMethod()
        {
            var tableRepository = AzureTableRepository.GetInstance();
            var items = tableRepository.GetItems("Brands");
            Console.Write("Items are: ");
            foreach (var item in items)
                Console.Write("{0}, ", item);
        }
    }
}
