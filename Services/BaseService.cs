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
    public class BaseService<TEntity, TDTO, TCreateDTO> : IBaseService<TDTO, TCreateDTO>
        where TEntity : class, IId, new()
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        public readonly IStoreFiles storeFiles;
        public const string cacheTag = "team";
        public const string container = "team";
        private const bool IsNew = true;

        public BaseService(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IStoreFiles storeFiles)     
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.storeFiles = storeFiles;
        }

        public async Task<List<TDTO>> Get(PaginationDTO pagination)
        {
            var queryable = context.Set<TEntity>().AsQueryable();       
            return await queryable.Paginate(pagination)
                             .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                             .ToListAsync();
        }

        public async Task Create(TCreateDTO dto)
        {
            var entity = mapper.Map<TEntity>(dto);
            context.Add(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
        }

        public async Task Update(int id, TCreateDTO dto)
        {
            var exists = await context.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (!exists) throw new Exception("Not found");
            var entity = mapper.Map<TEntity>(dto);
            entity.Id = id;
            context.Update(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
        }

        public async Task Delete(int id)
        {
            var deleted = await context.Set<TEntity>().Where(g => g.Id == id).ExecuteDeleteAsync();
            if (deleted == 0)
            {
                throw new Exception("Not found");
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
        }

    }
}
