using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;

namespace LoliBotNew.Data
{
    public static class JsonMaster
    {
        private const string jsonPath = "E:\\vs\\LoliBotNew\\LoliBotNew\\Data\\Users.Json";
        private static List<User> AllUsers;

        static JsonMaster()
        {
           
            AllUsers = ReadUserDataFromFile();
            StartWriteTimer();
        }
        private static List<User> ReadUserDataFromFile()
        {
            
            List<User> Result;
            //no need for try clause, application should fail if something is wrong with the json path...
            StreamReader str=new StreamReader(jsonPath);
            string JsonString = str.ReadToEnd();
            str.Close();
            if (JsonString == "") return new List<User>();
            Result=JsonSerializer.Deserialize<List<User>>(JsonString);

            return Result;
        }

      

        public static User GetUser(ulong id)
        {
            try
            {
                return AllUsers.First(x => x.id == id);
            }
            catch
            {
                return null;
            }
        }

        public static void CreateUser(ulong id,string loliname)
        {
            if (exists(id))
                AllUsers.First(x => x.id == id).Loliname = loliname;
            else
                AllUsers.Add(new User(id, loliname));

        }

        public static bool exists(ulong id)
        {
            return AllUsers.Any(x=>x.id == id);
        }


        public  static void AddXp(ulong id, int XPtoAdd)
        {
            try
            {
                AllUsers.First(x => x.id == id).xp += XPtoAdd;
            }
            catch
            {

            }

        }
        public  static void RoleIncrement(ulong id)
        {
                User user = GetUser(id);
                if(user.roleNumber<Roles.Rolelist.Count-1) user.roleNumber += 1;
        }

        //this is called "change" money not "add" money because it can be used for deducting and adding
        //for adding give a positive value for money 
        //for deducting money give a negative value for money
        public static void ChangeMoney(ulong id, double money)
        {
            AllUsers.First(x => x.id == id).money += money;
        }
        private static void StartWriteTimer()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(10);

            var timer = new System.Threading.Timer((e) =>
            {
                WriteToFile();
            }, null, startTimeSpan, periodTimeSpan);
        }
        public static void AssignOwnerShip(ulong id)
        {
            try
            {
                AllUsers.First(x => x.id == id).isOwner = true;
            }
            catch
            {

            }
        }
        public static void RemoveOwnerShip(ulong id)
        {
            try
            {
                AllUsers.First(x => x.id == id).isOwner = false;
            }
            catch
            {

            }
        }
        private static void WriteToFile()
        {
            Console.WriteLine("Writing To File");
            string JsonString=JsonSerializer.Serialize<List<User>>(AllUsers);
            StreamWriter writer=new StreamWriter(jsonPath);
            writer.Write(JsonString);
            writer.Close();
            Console.WriteLine("Writing Was Succesfull, Last Updated "+DateTime.Now);
        }

        }

    }
