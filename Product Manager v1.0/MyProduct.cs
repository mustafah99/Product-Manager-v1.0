using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Manager_v1._0
{
    public class MyProduct
    {
        public MyProduct(string articleNumber, string name, string description, int price)
        {
            this.articleNumber = articleNumber;
            this.name = name;
            this.description = description;
            this.price = price;
        }

        public MyProduct(string articleNumber, string name, string description, string price1)
        {
            this.articleNumber = articleNumber;
            this.name = name;
            this.description = description;
        }

        public string articleNumber { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
    }
}
