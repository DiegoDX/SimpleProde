using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Services;

namespace SimpleProde.Controllers
{
    [Route("api/match")]
    [ApiController]
    public class ChampionshipController : CustomBaseController
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        public const string cacheTag = "Championship";
        public const string container = "Championship";
        private const bool IsNew = true;
        public readonly IStoreFiles storeFiles;

        public ChampionshipController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IStoreFiles storeFiles)
            : base(context, mapper, outputCacheStore, cacheTag)
        {
            context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.storeFiles = storeFiles;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ChampionshipDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Championship, ChampionshipDTO>(pagination, orderBy: a => a.Name);
        }

        //[HttpGet("{id:int}", Name = "ObtenerTeamPorId")]
        //[OutputCache(Tags = [cacheTag])]
        //public async Task<ActionResult<team>>()
        //{

        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ChampionshipCreateDTO championshipCreateDTO)
        {
            Championship championship = mapper.Map<Championship>(championshipCreateDTO);
            SaveImage(championshipCreateDTO, championship, IsNew);
            context.Add(championship);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] ChampionshipCreateDTO championshipCreateDTO)
        {
            Championship championship = await context.Championships.FirstOrDefaultAsync(t => t.Id == id);
            if (championship is null)
            {
                return NotFound();
            }

            championship = mapper.Map<Championship>(championshipCreateDTO);
            SaveImage(championshipCreateDTO, championship, !IsNew);


            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Team>(id);
        }

        private async void SaveImage(ChampionshipCreateDTO championshipCreateDTO, Championship championship, bool isNew)
        {
            if (championshipCreateDTO.Icon is not null)
            {
                championship.Icon = isNew
                    ? await storeFiles.Edit(championship.Icon, container, championshipCreateDTO.Icon)
                    : await storeFiles.Store(container, championshipCreateDTO.Icon);
            }
        }
    }
}