using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace LoliBotNew.Modules
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        static void Main(string[] args)
        {
          HTTPGenerator.Start();
            new Program().RunBotAsync().GetAwaiter().GetResult();

        }

        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection().AddSingleton(client).AddSingleton(commands).BuildServiceProvider();

            string token = "OTAwNzU1MTY3MTExMTUxNzI3.G9XP_T.LMYIl5nCq_ShZwsoA3s7HCOg8K0l_tFYGT3Dgw";

            client.Log += client_Log;

            await RegisterCommandAsync();

            await client.LoginAsync(TokenType.Bot, token);

            await client.StartAsync();

            await Task.Delay(-1);
        }

        private Task client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (client == null || message == null) return;

            var context = new SocketCommandContext(client, message);
            if (message.Author.IsBot) return;

            int argpos = 0;
            if ((message.HasStringPrefix("!", ref argpos) || message.HasStringPrefix("l/", ref argpos)) || message.HasStringPrefix("L/",ref argpos))
            {
                var result = await commands.ExecuteAsync(context, argpos, services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
            }
        }

        




    }
}



