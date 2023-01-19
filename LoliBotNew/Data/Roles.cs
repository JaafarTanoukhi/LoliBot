using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Net.WebRequestMethods;

namespace LoliBotNew.Data
{
    static class Roles
    {
        //embedded into code for simplicity
        public static List<Role> Rolelist = new List<Role>()
        {
            new Role("Master Lolicon",20000,"https://img-9gag-fun.9cache.com/photo/a3QqMQe_460s.jpg"),
            new Role("Average Loli Enjoyer",40000,"https://i.kym-cdn.com/entries/icons/original/000/026/152/gigachad.jpg "),
            new Role("LoliCon",10000,"https://img-9gag-fun.9cache.com/photo/aAdmPRg_460s.jpg"),
            new Role("LoliGod",100000,"https://d.newsweek.com/en/full/1310223/shaggy-god-meme-reddit.jpg?w=1600&h=1200&q=88&f=0a6b0597dc7717de9cc13b6721147a31")
        };
    }
}