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
            var diceCommand = new SlashCommandBuilder()
                .WithDescription("roll dice based on standard tabletop dice notation (eg. 1d4+1)")
                .WithName("roll-dice")
                .AddOption("roll", ApplicationCommandOptionType.String, "standard dice notation to roll", isRequired: true)
                .AddOption("private", ApplicationCommandOptionType.Boolean, "do you want others to see this roll", isRequired: true);

            var guildCommand = new Discord.SlashCommandBuilder()
                .WithName("list-roles")
                .WithDescription("Lists all roles of a user.")
                .AddOption("user", ApplicationCommandOptionType.User, "The users whos roles you want to be listed", isRequired: true);

            var settingsCommand = new Discord.SlashCommandBuilder()
                .WithName("settings")
                .WithDescription("Changes some settings within the bot.")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("field-a")
                    .WithDescription("Gets or sets the field A")
                    .WithType(ApplicationCommandOptionType.SubCommandGroup)
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithName("set")
                        .WithDescription("Sets the field A")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .AddOption("value", ApplicationCommandOptionType.String, "the value to set the field to", isRequired: true)
                    ).AddOption(new SlashCommandOptionBuilder()
                    .WithName("get")
                    .WithDescription("Gets the value of field A.")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    )
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithName("field-b")
                    .WithDescription("Gets or sets the field B")
                    .WithType(ApplicationCommandOptionType.SubCommandGroup)
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithName("set")
                        .WithDescription("Sets the field B")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .AddOption("value", ApplicationCommandOptionType.Integer, "the value to set the field to", isRequired: true)
                    ).AddOption(new SlashCommandOptionBuilder()
                        .WithName("get")
                        .WithDescription("Gets the value of field B.")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                    )
                ).AddOption(new SlashCommandOptionBuilder()
                    .WithName("field-c")
                    .WithDescription("Gets or sets the field C")
                    .WithType(ApplicationCommandOptionType.SubCommandGroup)
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithName("set")
                        .WithDescription("Sets the field C")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .AddOption("value", ApplicationCommandOptionType.Boolean, "the value to set the fie to.", isRequired: true)
                    ).AddOption(new SlashCommandOptionBuilder()
                        .WithName("get")
                        .WithDescription("Gets the value of field C.")
                        .WithType(ApplicationCommandOptionType.SubCommand)
                    )
                );

            try
            {
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(diceCommand.Build());
                await _client.CreateGlobalApplicationCommandAsync(settingsCommand.Build());
            }
            catch (HttpException e)
            {
                var json = JsonConvert.SerializeObject(e.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }


    }


}