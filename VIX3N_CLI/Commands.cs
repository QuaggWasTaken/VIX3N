using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIX3N_CLI
{
    internal class Commands
    {
        public string FieldA { get; set; }
        public int FieldB { get; set; }
        public bool FieldC { get; set; }
        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "list-roles":
                    await HandleListRoleCommand(command);
                    break;
                case "roll-dice":
                    await HandleDiceRollCommand(command);
                    break;
                case "settings":
                    await HandleSettingsCommand(command);
                    break;
            }
        }
        public async Task HandleListRoleCommand(SocketSlashCommand command)
        {
            var guildUser = (SocketGuildUser)command.Data.Options.First().Value;
            var roleList = string.Join(",\n", guildUser.Roles.Where(x => !x.IsEveryone).Select(x => x.Mention));

            var embedBuilder = new EmbedBuilder()
                .WithAuthor(guildUser.ToString(), guildUser.GetAvatarUrl() ?? guildUser.GetDefaultAvatarUrl())
                .WithTitle("Roles")
                .WithDescription(roleList)
                .WithColor(Color.Green)
                .WithCurrentTimestamp();

            await command.RespondAsync(embed: embedBuilder.Build());
        }

        public async Task HandleDiceRollCommand(SocketSlashCommand command)
        {
            await command.RespondAsync(embed: DiceRoll(command), ephemeral: (bool)command.Data.Options.ToArray()[1].Value);
        }
        public async Task HandleSettingsCommand(SocketSlashCommand command)
        {
            // First lets extract our variables
            var fieldName = command.Data.Options.First().Name;
            var getOrSet = command.Data.Options.First().Options.First().Name;
            // Since there is no value on a get command, we use the ? operator because "Options" can be null.
            var value = command.Data.Options.First().Options.First().Options?.FirstOrDefault().Value;

            switch (fieldName)
            {
                case "field-a":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-a` is `{FieldA}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldA = (string)value;
                            await command.RespondAsync($"`field-a` has been set to `{FieldA}`");
                        }
                    }
                    break;
                case "field-b":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-b` is `{FieldB}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldB = (int)value;
                            await command.RespondAsync($"`field-b` has been set to `{FieldB}`");
                        }
                    }
                    break;
                case "field-c":
                    {
                        if (getOrSet == "get")
                        {
                            await command.RespondAsync($"The value of `field-c` is `{FieldC}`");
                        }
                        else if (getOrSet == "set")
                        {
                            this.FieldC = (bool)value;
                            await command.RespondAsync($"`field-c` has been set to `{FieldC}`");
                        }
                    }
                    break;
            }
        }


        public Embed DiceRoll(SocketSlashCommand cmd)
        {
            var embedBuilder = new EmbedBuilder()
                .WithAuthor($"{cmd.User.Username}")
                .WithDescription($"Rolled {cmd.Data.Options.First().Value}");

            return embedBuilder.Build();
        }
    }
}
