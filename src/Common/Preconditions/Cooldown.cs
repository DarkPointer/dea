﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using DEA.Common.Extensions.DiscordExtensions;
using DEA.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using DEA.Services;

namespace DEA.Common.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal sealed class Cooldown : PreconditionAttribute
    {
        private IServiceProvider _serviceProvider;
        private CooldownService _cooldownService;
        private TimeSpan _cooldown;

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider serviceProvider)
        {
            return Task.Run(async () =>
            {
                _serviceProvider = serviceProvider;
                _cooldownService = _serviceProvider.GetService<CooldownService>();

                if (_cooldownService.TryGet(x => x.UserId == context.User.Id && x.GuildId == context.Guild.Id && x.CommandId == command.Name, out _cooldown))
                {
                    await context.Channel.SendErrorAsync($"Hours: {_cooldown.Hours}\nMinutes: {_cooldown.Minutes}\nSeconds: {_cooldown.Seconds}", $"{command.Name.UpperFirstChar()} cooldown for {context.User}");
                    return PreconditionResult.FromError(string.Empty);
                }

                return PreconditionResult.FromSuccess();
            });
        }
    }
}