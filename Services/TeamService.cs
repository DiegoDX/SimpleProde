using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using SimpleProde.Controllers;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Utilities;
using System.Linq;
using System.Linq.Expressions;

namespace SimpleProde.Services
{
    public class TeamService : ITeamService 
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        public readonly IStoreFiles storeFiles;
        public const string cacheTag = "team";
        public const string container = "team";
        private const bool IsNew = true;

        public TeamService(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IStoreFiles storeFiles)           
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.storeFiles = storeFiles;
        }

        public async Task<List<TeamDTO>> Get(PaginationDTO pagination)
        {
            return await context.Teams.AsQueryable()
                .OrderBy(t => t.Name)
                .Paginate(pagination)
                .ProjectTo<TeamDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TeamDTO?> GetById(int id)
        {
            return await context.Teams.Where(t=> t.Id == id)
                .ProjectTo<TeamDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<int> Create(TeamCreateDTO teamCreateDTO)
        {
            var entity = mapper.Map<Team>(teamCreateDTO);

            SaveImage(teamCreateDTO, entity, true);

            context.Teams.Add(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return entity.Id;
        }

        public async Task<bool> Update(int id, TeamCreateDTO teamCreateDTO)
        {
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team is null) return false;

            mapper.Map(teamCreateDTO, team);

            SaveImage(teamCreateDTO, team, false);

            //context.Teams.Update(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var team = await context.Teams.FindAsync(id);

            if (team is null)
            {
                return false;
            }

            context.Teams.Remove(team);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return true;
        }

        private void SaveImage(TeamCreateDTO teamCreateDTO, Team team, bool isNew)
        {
            ShirtColorImage(teamCreateDTO, team, isNew);
            FlagImage(teamCreateDTO, team, isNew);
        }

        private async void ShirtColorImage(TeamCreateDTO teamCreateDTO, Team team, bool isNew)
        {
            if (teamCreateDTO.ShirtColor is not null)
            {
                team.ShirtColor = isNew
                    ? await storeFiles.Edit(team.ShirtColor, container, teamCreateDTO.ShirtColor)
                    : await storeFiles.Store(container, teamCreateDTO.ShirtColor);
            }
        }

        private async void FlagImage(TeamCreateDTO teamCreateDTO, Team team, bool isNew)
        {
            if (teamCreateDTO.Flag is not null)
            {
                team.Flag = isNew
                    ? await storeFiles.Edit(team.Flag, container, teamCreateDTO.Flag)
                    : await storeFiles.Store(container, teamCreateDTO.Flag);
            }
        }
    }
}
