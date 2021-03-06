﻿using DEA.Common;
using DEA.Common.Preconditions;
using DEA.Database.Repositories;
using Discord.Commands;

namespace DEA.Modules.BotOwners
{
    [Require(Attributes.BotOwner)]
    [Summary("These commands may only be used by the bot owners provided by the Owner Ids in the Credentials.json file.")]
    public partial class BotOwners : Module
    {
        private readonly GuildRepository _guildRepo;
        private readonly BlacklistRepository _blacklistRepo;

        public BotOwners(GuildRepository guildRepo, BlacklistRepository blacklistRepo)
        {
            _guildRepo = guildRepo;
            _blacklistRepo = blacklistRepo;
        }
    }
}
