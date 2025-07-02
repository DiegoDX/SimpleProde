using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Services;
using SimpleProde.Utilities;
using System.Linq.Expressions;

namespace SimpleProde.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly string cacheTag;
       

        public CustomBaseController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, string cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.cacheTag = cacheTag;            
        }

        protected async Task<List<TDTO>> Get<TEntiTy, TDTO>(PaginationDTO pagination,
         Expression<Func<TEntiTy, object>> orderBy)
         where TEntiTy : class
        {
            return await context.Set<TEntiTy>()
                .OrderBy(orderBy)
                .ProjectTo<TDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        //protected async Task<List<TDTO>> Get<TEntiTy, TDTO>(PaginationDTO pagination,
        //    Expression<Func<TEntiTy, object>> orderBy)
        //    where TEntiTy : class
        //{
        //    var queryable = context.Set<TEntiTy>().AsQueryable();
        //    await HttpContext.InsertPaginationParametersInHeader(queryable);
        //    return await queryable
        //        .OrderBy(orderBy)
        //        .Paginate(pagination)
        //        .ProjectTo<TDTO>(mapper.ConfigurationProvider).ToListAsync();
        //}

        protected async Task<ActionResult<TDTO>> Get<TEntiTy, TDTO>(int id)
            where TEntiTy : class, IId
            where TDTO : IId
        {
            var entidad = await context.Set<TEntiTy>()
                .ProjectTo<TDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entidad is null)
            {
                return NotFound();
            }

            return entidad;
        }

        protected async Task<IActionResult> Post<TCreationDTO, TEntiTy, TDTO>
            (TCreationDTO creationDTO, string pathname)
            where TEntiTy : class, IId
        {
            var entity = mapper.Map<TEntiTy>(creationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var entityDTO = mapper.Map<TDTO>(entity);
            return CreatedAtRoute(pathname, new { id = entity.Id }, entityDTO);
        }

        protected async Task<IActionResult> Post<TCreationDTO, TEntiTy>
           (TCreationDTO creationDTO)
           where TEntiTy : class, IId
        {
            var entity = mapper.Map<TEntiTy>(creationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return NoContent();
        }

        protected async Task<IActionResult> Put<TCreationDTO, TEntiTy>(int id, TCreationDTO creationDTO)
            where TEntiTy : class, IId
        {
            var entityExists = await context.Set<TEntiTy>().AnyAsync(g => g.Id == id);

            if (!entityExists)
            {
                return NotFound();
            }

            var entity = mapper.Map<TEntiTy>(creationDTO);
            entity.Id = id;

            context.Update(entity);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return NoContent();
        }

        protected async Task<IActionResult> Delete<TEntity>(int id)
            where TEntity : class, IId
        {
            var deletedRegister = await context.Set<TEntity>().Where(g => g.Id == id).ExecuteDeleteAsync();
            if (deletedRegister == 0)
            {
                return NotFound();
            }

            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
        
    }
}
