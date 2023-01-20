using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Discord;
using LoliBotNew.Data;

namespace LoliBotNew.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        Random rand = new Random();
        [Command("Loli")]
        public async Task loli()
        {
          User user=null;
          try{
             user = JsonMaster.GetUser(Context.User.Id);
            }
          catch(Exception e){
            Console.WriteLine(e.Message);
          }
            if (user!=null  &&  user.isOwner)
            {
                await Context.Channel.SendMessageAsync("You already have a loli");
                return;
            }

            string filename = Lists.ImageLinks.ElementAt(rand.Next(Lists.ImageLinks.Count));
            JsonMaster.CreateUser(Context.User.Id, filename);

            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                ImageUrl = filename

            }.Build();

            await Context.Channel.SendMessageAsync("Hey, That's me!!", false, embed);

        }

        [Command("Adopt")]
        public async Task Adopt()
        {
            User user = JsonMaster.GetUser(Context.User.Id);

            if (user == null)
            {
                await Context.Channel.SendMessageAsync("You need to use the loli command first");
                return;
            }
            if (!user.isOwner)
            {
                JsonMaster.AssignOwnerShip(user.id);
                await Context.Channel.SendMessageAsync("You are now the proud owner of this loli");
            }
            else 
                 await Context.Channel.SendMessageAsync("You already Have a loli");
            
           
            

            var efb = new EmbedFooterBuilder() { Text = $"LoliPoints:{user.xp}/{user.MaxXp}{Environment.NewLine}Level: {user.Level}" };

            var field1 = new EmbedFieldBuilder()
            {
                Name = "Money",
                IsInline = true,
                Value = user.money + "$",
            };


            var field2 = new EmbedFieldBuilder()
            {
                Name = "Races Won",
                IsInline = true,
                Value = user.races_won,
            };

            var fields = new List<EmbedFieldBuilder>() { field1, field2 };

            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                Title = Context.User.Username + "'s Loli",
                ThumbnailUrl = Context.User.GetAvatarUrl(),
                ImageUrl = user.Loliname,
                Fields = fields,
                Footer = efb,
            };

            var mr = new MessageReference(Context.Message.Id);

            await Context.Channel.SendMessageAsync(embed: embed.Build(), messageReference: mr);


        }

        [Command("lolilewd")]
        public async Task question()
        {
            MessageReference mr = new MessageReference(Context.Message.Id);

            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                ImageUrl = Lists.lewdImageLinks.ElementAt(rand.Next(Lists.lewdImageLinks.Count))
            }.Build();

            await Context.Channel.SendMessageAsync(embed: embed, messageReference: mr);
        }

        [Command("storytime")]
        public async Task storytime()
        {
            await Context.Channel.SendMessageAsync(Lists.StoryTimeLinks.ElementAt(rand.Next(Lists.StoryTimeLinks.Count)));
        }

        [Command("getmilk")]
        public async Task GetMilk()
        {
            MessageReference mr = new MessageReference(Context.Message.Id);
            
            if (JsonMaster.exists(Context.User.Id))
            {
                JsonMaster.RemoveOwnerShip(Context.User.Id);
                await Context.Channel.SendMessageAsync("your were never seen again", messageReference: mr);
                return;
            }
            await Context.Channel.SendMessageAsync("You need a Loli to get milk");
        }

        [Command("Pat", true)]
        public async Task pat()
        {
            MessageReference mr = new MessageReference(Context.Message.Id);
            string PatString = Lists.PatStringLinks.ElementAt(rand.Next(Lists.PatStringLinks.Count));
            string mentions = "";
            string names = "";
            string description_message = "";
            string footer_message = "You got 1 LoliPoint and 10$";

            User user = JsonMaster.GetUser(Context.User.Id);
            if (user == null)
            {
                await Context.Channel.SendMessageAsync("You don't own a loli to pat....sad");
                return;
            }
            if (Context.Message.MentionedUsers.Count >= 1)
            {
                int i = 0;
                foreach (var u in Context.Message.MentionedUsers)
                {
                    if (i == Context.Message.MentionedUsers.Count - 1)
                    {
                        mentions += u.Mention + " ";
                        names += u.Username + " ";
                        break;
                    }

                    mentions += u.Mention + " ";
                    names += u.Username + " and ";
                    i++;
                    if (user.isOwner)
                    {
                        JsonMaster.AddXp(user.id, 1);
                        JsonMaster.ChangeMoney(user.id, 10);
                    }

                }

                description_message = "You patted " + mentions;
                footer_message = "You and " + names + "get 1 LoliPoint and 10$ each";
            }

            var embedfooter = new EmbedFooterBuilder() { Text = footer_message };

            var embed = new EmbedBuilder()
            {
                Description = description_message,
                ImageUrl = PatString,
                Color = Color.Purple,
            };

            if (user.isOwner)
            {
                JsonMaster.AddXp(user.id, 1);
                JsonMaster.ChangeMoney(user.id, 10);
                embed.WithFooter(embedfooter);
            }

            await Context.Channel.SendMessageAsync(embed: embed.Build(), messageReference: mr);
        }

        [Command("profile")]
        public async Task profile()
        {
            User user = JsonMaster.GetUser(Context.User.Id);
            var efb = new EmbedFooterBuilder() { Text = $"LoliPoints: { user.xp}/{user.MaxXp}{Environment.NewLine}Level: {user.Level}" };

            var field1 = new EmbedFieldBuilder()
            {
                Name = "Money",
                IsInline = true,
                Value = user.money + "$",
            };


            var field2 = new EmbedFieldBuilder()
            {
                Name = "Races Won",
                IsInline = true,
                Value = user.races_won,
            };

            var fields = new List<EmbedFieldBuilder>() { field1, field2 };

            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                Title = Context.User.Username + "'s Loli",
                ThumbnailUrl = Context.User.GetAvatarUrl(),
                ImageUrl = user.Loliname,
                Fields = fields,
                Footer = efb,
            };

            var mr = new MessageReference(Context.Message.Id);

            await Context.Channel.SendMessageAsync(embed: embed.Build(), messageReference: mr);
        }

        [Command("work")]
        public async Task work()
        {
            MessageReference mr = new MessageReference();
            Emoji emoji = new Emoji("🙄");
            User user = JsonMaster.GetUser(Context.User.Id);
            if (!user.isOwner)
            {
                await Context.Channel.SendMessageAsync("You need a loli first to work....isn't it obvious" + emoji, messageReference: mr);
                return;
            }
            
            if (!Worker.exists(Context.User.Id)) 
            {
                Worker worker= new Worker(Context.User.Id);
                JsonMaster.AddXp(Context.User.Id, 10);
                JsonMaster.ChangeMoney(Context.User.Id, 100);
                await Context.Channel.SendMessageAsync("Wow, you got to work on a useless digital bot, here you go 100 useless dolars and 10 LoliPoints");
                return;
            }
            await ReplyAsync("You got to wait a little bit more before working...pat your loli to pass the time!");
           
        }

        [Command("race", true,RunMode=RunMode.Async)]
        public async Task race()
        {
            User user = JsonMaster.GetUser(Context.User.Id);
            if(!user.isOwner)
            {
               await ReplyAsync("You need to own a loli");
                return;
            }
            var message = Context.Message.ToString().Split(" ");
            double bet;
            try
            {
                bet = double.Parse(message[1]);
            }catch 
            {
               await ReplyAsync("The command should be in the format: !race bet @user1@user2...");
                return;
            }
            if(bet<=0)
            {
                Embed e = new EmbedBuilder()
                {
                    ImageUrl= "https://i.redd.it/rsxdejqw8w8x.png",
                }.Build();
                await ReplyAsync("Ha...You can't fool me!! That is an invalid amount.", embed: e);
                return;
            }
           
            if (user.money < bet)
            {
                await ReplyAsync("you don't have enough money dum dum, try betting lower");
                return;
            } 

            if (Context.Message.MentionedUsers.Count >= 1)
            {
                await ReplyAsync("All mentioned users must reply with race for the race to begin");
                var players = await GetPlayers(Context);
                if (players==null) return;
                foreach (var player in players)
                {
                    var p= JsonMaster.GetUser(player.Id);
                    if (p.isOwner)
                    {
                        await ReplyAsync($"{player.Mention} does not own a loli");
                        return;
                    }
                    if (p.money < bet)
                    {
                       await ReplyAsync("Why are you accepting?!Your'e broke...");
                        players.Remove(player);
                    }
                }
                var winner = players[rand.Next(players.Count)];
                await ReplyAsync("The winner is " + winner+"\n Congrats you get "+bet+"$ and 5 LoliPoints");
                JsonMaster.ChangeMoney(winner.Id, bet);
                JsonMaster.AddXp(winner.Id, 5);
                return;
            }

            int num = rand.Next(2);
            switch (num)
            {
                case 0: await ReplyAsync("You Won!!\nCongrats You get "+bet+"$ and 5 LoliPoints");
                    JsonMaster.ChangeMoney(user.id, bet);
                    JsonMaster.AddXp(user.id, 5);
                    break;
                case 1: await ReplyAsync("I Won!!");break;
            }
           
        }
        [Command("help")]
        public async Task Help()
        {
            var field1 = new EmbedFieldBuilder()
            {
                Name = "!loli",
                IsInline = false,
                Value="Get a loli to potentially adopt"

            };

            var field2 = new EmbedFieldBuilder()
            {
                Name = "!adopt",
                IsInline = false,
                Value="Adopt a loli"
            };

            var field3 = new EmbedFieldBuilder()
            {
                Name = "!lolilewd",
                IsInline = false,
                Value = "Get something hot😉"
            };

            var field4 = new EmbedFieldBuilder()
            {
                Name = "!storytime",
                IsInline = false,
                Value = "Get something"
            };
            var field5 = new EmbedFieldBuilder()
            {
                Name = "!getmilk",
                IsInline = false,
                Value = "Just an quick trip to the grocery store"
            };
            var field6 = new EmbedFieldBuilder()
            {
                Name = "!pat",
                IsInline = false,
                Value = "Pat someone's loli or just a random loli"
            };
            var field7 = new EmbedFieldBuilder()
            {
                Name = "!profile",
                IsInline = false,
                Value = "Look at your majestic loli"
            };
            var field8 = new EmbedFieldBuilder()
            {
                Name = "!work",
                IsInline = false,
                Value = "Go to work!"
            };
            var field9 = new EmbedFieldBuilder()
            {
                Name = "!race",
                IsInline = false,
                Value = "Start a race with a friend or with me if you're lonely"
            };
            var field10 = new EmbedFieldBuilder()
            {
                Name = "!shop",
                IsInline = false,
                Value = "Browse the shop"
            };
            var field11 = new EmbedFieldBuilder()
            {
                Name = "!buy",
                IsInline = false,
                Value="Buy something for your loli or for you..."
                
            };
            var field12 = new EmbedFieldBuilder()
            {
                Name = "!roles",
                IsInline = false,
                Value = "View roles that you can buy"
            };
            var field13 = new EmbedFieldBuilder()
            {
                Name = "!buyrole",
                IsInline = false,
                Value = "Buy a role",
            };
            var fields = new List<EmbedFieldBuilder>() { field1, field2,field3,field4,field5,field6,field7,field8,field9,field10,field11,field12,field13};

            //finish up the help and all you have left is the roles buying command
            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                Title = "Help",
                Description="All Commands that you can use",
                Fields=fields
            }.Build();

            var mr = new MessageReference(Context.Message.Id);
            await Context.Channel.SendMessageAsync(embed: embed, messageReference: mr);
        }

        [Command("Roles")]
        public async Task roles()
        {
            var mr = new MessageReference(Context.Message.Id);
            var fields = new List<EmbedFieldBuilder>();

            foreach(var role in Roles.Rolelist)
            {
                EmbedFieldBuilder eb = new EmbedFieldBuilder()
                {
                    Name = role.name,
                    IsInline = false,
                    Value = $"Price: {role.price}$",
                };
                fields.Add(eb);
            }

            var embed = new EmbedBuilder()
            {
                Color = Color.Purple,
                Title = "Roles",
                Description = "All Roles That You Can Buy",
                Fields = fields
            }.Build();

            await Context.Channel.SendMessageAsync(embed: embed, messageReference: mr);
        }

        [Command("shop")]
        public async Task openshop()
        {
            string message = "";

            for (int i = 0; i < Shop.Count(); i++)
            {
                var item = Shop.getItem(i);
                message += $"{item.ItemId}. {item.ItemName} Price: {item.ItemPrice}${Environment.NewLine}";
            }
            message += $"{Environment.NewLine}**(Totally not an idleRpg Ripoff)**";
            await Context.Channel.SendMessageAsync(message);
        }
        [Command("buy",true)]
        public async Task buy()
        {
            User user = JsonMaster.GetUser(Context.User.Id);
            if (!user.isOwner)
            {
                await ReplyAsync("You need to have a loli cz your'e broke without one");
                return;
            }
            var message = Context.Message.ToString().Split(" ");
            int itemId;
            try
            {
                itemId = int.Parse(message[1]);
            }
            catch
            {
                await ReplyAsync("The command should be in the format: !buy itemId");
                return;
            }
            if(itemId>Shop.Count()||itemId<=0)
            {
                await ReplyAsync("Are you sure your'e using a valid itemId");
                return;
            }
            ShopItem item = Shop.getItem(itemId);
            if (user.money >= item.ItemPrice)
            {   
                JsonMaster.ChangeMoney(Context.User.Id, item.ItemPrice);
                JsonMaster.AddXp(Context.User.Id, item.ItemXp);
                await ReplyAsync(item.message);
                return;
            }
            await ReplyAsync("Sorry but Your'e broke *Giggles* ");

        }
        [Command("Buyrole")]
        public async Task buyRole()
        {
            User user = JsonMaster.GetUser(Context.User.Id);
           
            if (user.isOwner)
            {
               await ReplyAsync("You need to own a loli");
                return;
            }
            var r = Roles.Rolelist[user.roleNumber];
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == r.name);
            if (role==null)  role=await (user as IGuildUser).Guild.CreateRoleAsync(r.name, null, Color.Purple, false, null);
            Embed e = new EmbedBuilder()
            {
                ImageUrl=r.imageAdrs
            }.Build();
            if (user.money >= r.price)
            {
                await (user as IGuildUser).AddRoleAsync(role);
                JsonMaster.ChangeMoney(user.id, -r.price);
                JsonMaster.AddXp(user.id, 100);
                JsonMaster.RoleIncrement(user.id);
                await ReplyAsync("Congrats... You have now the " + MentionUtils.MentionRole(role.Id) + " role",embed: e);
                return;
            }
            await ReplyAsync("Your'e broke...Go away!");

        }

        private async Task<List<SocketUser>> GetPlayers(SocketCommandContext context)
        {
            var mentioned_users=context.Message.MentionedUsers;
            
            var players = new List<SocketUser>();
            players.Add(context.Message.Author);

            Stopwatch timer=new Stopwatch();
            timer.Start();
            
            while (true)
            {
                var messages = Context.Channel.GetMessagesAsync(limit: 1);
                var Messages =await AsyncEnumerableExtensions.FlattenAsync(messages);
                var msg = Messages.ElementAt(0);

                        if (msg.ToString() == "race")
                        {
                            for (int i = 0; i <= mentioned_users.Count; i++)
                            {
                                if (mentioned_users.ElementAt(i).ToString()==msg.Author.ToString())
                                {
                                    players.Add(mentioned_users.ElementAt(i));
                                    break;
                                }
                            }

                        }
               
                if (players.Count == mentioned_users.Count + 1)
                    return players;
                if (timer.ElapsedMilliseconds>10000)
                {
                    timer.Stop();
                    await ReplyAsync("You guys took too long to start...so bye!");
                    return null;
                }
                
            }
        }
    }
}
