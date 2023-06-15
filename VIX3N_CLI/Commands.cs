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
            await command.RespondAsync(embed: DiceRoll(command), ephemeral: (bool)command.Data.Options.ToArray()[1].Value );
        }

        public Embed DiceRoll(SocketSlashCommand cmd)
        {
            var embedBuilder = new EmbedBuilder()
                .WithAuthor($"{cmd.User.ToString}")
                .WithDescription($"Rolled {cmd.Data.Options.First().Value}");

            return embedBuilder.Build();
        }
    }
}
