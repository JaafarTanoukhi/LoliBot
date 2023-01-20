using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LoliBotNew.Data
{
    public static class Shop
    {
        private static List<ShopItem> itemList = new List<ShopItem>();
        private static string filepath = "Shop.txt";

        static Shop()
        {
            ReadFromFile();
        }
        public static int Count()
        {
            return itemList.Count;
        }
        public static ShopItem getItem(int index)
        {
            return itemList[index + 1];
        }

        private static void ReadFromFile()
        {
            if (!File.Exists(filepath)) File.Create(filepath);

            try
            {
                StreamReader str = new StreamReader(filepath);
                while (!str.EndOfStream)
                {
                    string[] line = str.ReadLine().Split(" ");
                    var item = new ShopItem(int.Parse(line[0]), line[1], int.Parse(line[2]), int.Parse(line[3]), str.ReadLine());
                    itemList.Add(item);
                }
                str.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }






    }
}

