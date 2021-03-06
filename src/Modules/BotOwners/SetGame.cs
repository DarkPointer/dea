﻿using Discord.Commands;
using System.Threading.Tasks;

namespace DEA.Modules.BotOwners
{
    public partial class BotOwners
    {
        [Command("SetGame")]
        [Remarks("boss froth")]
        [Summary("Sets the game of DEA.")]
        public async Task SetGame([Remainder] string game)
        {
            await Context.Client.SetGameAsync(game);
            await ReplyAsync($"Successfully set the game to {game}.");
        }
    }
}
