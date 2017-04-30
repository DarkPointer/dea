﻿using DEA.Database.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DEA.Database.Repositories
{
    public class GuildRepository : BaseRepository<Guild>
    {
        public GuildRepository(IMongoCollection<Guild> guilds) : base(guilds) { }

        public async Task<Guild> FetchGuildAsync(ulong guildId)
        {
            var dbGuild = await FetchAsync(x => x.GuildId == guildId);
            if (dbGuild == default(Guild))
            {
                var createdGuild = new Guild(guildId);
                await InsertAsync(createdGuild);
                return createdGuild;
            }
            return dbGuild;
        }

    }
}