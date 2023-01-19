using System;
using System.Collections.Generic;
using System.Text;

namespace LoliBotNew.Data
{
    public class ShopItem
    {
        public int ItemId { get; }
        public string ItemName { get; }
        public int ItemPrice { get; }
        public int ItemXp { get; }
        public string message { get; }

        public ShopItem(int ItemId, string ItemName, int ItemPrice, int ItemXp, string message)
        {
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.message = message;
            this.ItemPrice = ItemPrice;
            this.ItemXp = ItemXp;
        }
    }
}
