﻿using DEA.Common.Extensions;
using DEA.Common.Preconditions;
using DEA.Common.Utilities;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace DEA.Modules.Crime
{
    public partial class Crime
    {
        [Command("Collect")]
        [Cooldown]
        [Summary("Collect a portion from your slaves.")]
        public async Task Collect()
        {
            var collection = await _userRepo.AllAsync(x => x.SlaveOf == Context.User.Id && x.GuildId == Context.Guild.Id);

            if (collection.Count == 0)
            {
                ReplyError("You are not an owner of any slaves.");
            }

            var totalCashGain = 0m;
            var guildInterface = Context.Guild as IGuild;

            foreach (var slave in collection)
            {
                
                var slaveUser = await guildInterface.GetUserAsync(slave.UserId);
                if (slaveUser != null)
                {
                    totalCashGain += slave.Cash * 0.8m;
                    await _userRepo.EditCashAsync(await guildInterface.GetUserAsync(slave.UserId), Context.DbGuild, slave, -slave.Cash * 0.8m);
                }
            }

            await _userRepo.EditCashAsync(Context, totalCashGain);
            await ReplyAsync($"You have successfully collected {totalCashGain.USD()} in slave money.");
            _cooldownService.TryAdd(new CommandCooldown(Context.User.Id, Context.Guild.Id, "Collect", Config.CollectCooldown));
        }
    }
}
