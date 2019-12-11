using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.SingletonPattern
{
    public class AzureTableRepository
    {
        private static AzureTableRepository obj = new AzureTableRepository();

        public static AzureTableRepository GetInstance()
        {
            //if (obj == null)
            //{
            //    obj = new AzureTableRepository();
            //}

            return obj;
        }

        public IEnumerable GetItems(string tableName)
        {
            return new List<String>() { "Item1", "Item2", "Item3", "Item4" };
        }
    }
}
