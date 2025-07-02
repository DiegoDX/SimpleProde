using AutoMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Services;

namespace SimpleProde.Controllers
{
    [Route("api/Match")]
    [ApiController]
    public class MatchController : CustomBaseController
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        public readonly IStoreFiles storeFiles;
        public const string cacheTag = "Match";
        public const string container = "Match";


        public MatchController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore)
            : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<MatchDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Match, MatchDTO>(pagination, orderBy: a => a.ChampionshipId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MatchCreateDTO matchCreateDTO)
        {
            return await Post<MatchCreateDTO, Match>(matchCreateDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] MatchCreateDTO matchCreateDTO)
        {
            return await Put<MatchCreateDTO, Match>(id, matchCreateDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Match>(id);
        }
    }
}