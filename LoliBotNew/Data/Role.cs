using System;
using System.Collections.Generic;
using System.Text;

namespace LoliBotNew.Data
{
    public class Role
    {
        public string name { get; }
        public int price { get; }

        public string imageAdrs { get; }

        public Role(string name, int price, string imageAdrs)
        {
            this.name = name;
            this.price = price;
            this.imageAdrs = imageAdrs;
        }
    }
}
