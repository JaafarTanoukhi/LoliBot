using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LoliBotNew.Data
{
    [Serializable]
    public class User
    {
        
        public ulong id { get; }
 
        public string Loliname { get; set; }
  
        public bool isOwner { get; set; }
  
        public int Level { get; set; }
   
        public int xp { get; set; }
    
        public double money { get; set; }
   
        public int races_won { get; set; }
     
        public int roleNumber { get; set; }
      
        public int MaxXp => Level * 100;

        
        public User(ulong id,string Loliname, bool isOwner=false, int level = 1, int xp = 0, double money = 0, int races_won = 0, int roleNumber = 0)
        {
            this.Loliname = Loliname;
            this.id = id;
            this.isOwner = isOwner;
            Level = level;
            this.xp = xp;
            this.money = money;
            this.races_won = races_won;
            this.roleNumber = roleNumber;
        }

      
    }



}
