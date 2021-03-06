﻿using DEA.Services.Static;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using Discord.Commands;
using DEA.Common.Utilities;

namespace DEA.Events
{
    internal sealed class Ready
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _client;
        private readonly Statistics _statistics;

        public Ready(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _commandService = _serviceProvider.GetService<CommandService>();
            _statistics = _serviceProvider.GetService<Statistics>();
            _client = _serviceProvider.GetService<DiscordSocketClient>();
            _client.Ready += HandleReady;
        }

        private Task HandleReady()
        {
            return Task.Run(async () =>
            {
                Logger.Log(LogSeverity.Debug, $"Event", "Ready");

                await _client.SetGameAsync("Type $support");

                Documentation.CreateAndSave(_commandService);

                foreach (var module in _commandService.Modules)
                {
                    foreach (var command in module.Commands)
                    {
                        _statistics.CommandUsage.TryAdd(command.Name, 0);
                    }
                }
            });
        }
    }
}
