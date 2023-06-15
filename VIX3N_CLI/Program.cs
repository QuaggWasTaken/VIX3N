using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Reflection;

namespace VIX3N_CLI
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private ulong guildId = 562973571094282247;
        private Commands _commands = new Commands();

        public static Task Main(string[] args) => new Program().MainAsync(args);

        public async Task MainAsync(string[] args)
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            _client.Ready += Client_Ready;

            _client.SlashCommandExecuted += _commands.SlashCommandHandler;


            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter your bot token as an argument.");
                return;
            }

            var token = args[0];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();


            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
        public async Task Client_Ready()
        {
            var guild = _client.GetGuild(guildId);

            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("first-global-command");
            globalCommand.WithDescription("babys first global slash command");

            var guildCommand = new Discord.SlashCommandBuilder()
                .WithName("list-roles")
                .WithDescription("Lists all roles of a user.")
                .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true);

            try
            {
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException e)
            {
                var json = JsonConvert.SerializeObject(e.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        
    }

    
}