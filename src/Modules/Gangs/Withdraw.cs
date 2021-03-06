﻿using DEA.Common.Extensions;
using DEA.Common.Preconditions;
using DEA.Common.Utilities;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DEA.Modules.Gangs
{
    public partial class Gangs
    {
        [Command("Withdraw")]
        [Require(Attributes.InGang)]
        [Cooldown]
        [Remarks("50")]
        [Summary("Withdraw cash from your gang's funds.")]
        public async Task Withdraw(decimal cash)
        {
            if (cash < Config.MinWithdraw)
            {
                ReplyError($"The minimum withdrawal is {Config.MinWithdraw.USD()}.");
            }
            else if (cash > Math.Round(Context.Gang.Wealth * Config.WithdrawCap, 2))
            {
                ReplyError($"You may only withdraw {Config.WithdrawCap.ToString("P")} of your gang's wealth, " +
                           $"that is {(Context.Gang.Wealth * Config.WithdrawCap).USD()}.");
            }

            await _gangRepo.ModifyAsync(Context.Gang, x => x.Wealth = Context.Gang.Wealth - cash);
            await _userRepo.EditCashAsync(Context, +cash);

            await ReplyAsync($"You have successfully withdrawn {cash.USD()}. " +
                             $"{Context.Gang.Name}'s Wealth: {Context.Gang.Wealth.USD()}.");

            await Context.Gang.LeaderId.TryDMAsync(Context.Client, $"{Context.User.Boldify()} has withdrawn {cash.USD()} from your gang's wealth.");
            _cooldownService.TryAdd(new CommandCooldown(Context.User.Id, Context.Guild.Id, "Withdraw", Config.WithdrawCooldown));
        }
    }
}
