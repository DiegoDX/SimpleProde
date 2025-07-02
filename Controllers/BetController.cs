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
    [Route("api/Bet")]
    [ApiController]
    public class BetController : CustomBaseController
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        public readonly IStoreFiles storeFiles;
        public const string cacheTag = "Bet";
        public const string container = "Bet";


        public BetController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore)
            : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<BetDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Bet, BetDTO>(pagination, orderBy: a => a.MatchId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BetCreateDTO betCreateDTO)
        {
            return await Post<BetCreateDTO, Bet>(betCreateDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] BetCreateDTO betCreateDTO)
        {
            return await Put<BetCreateDTO, Bet>(id, betCreateDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Bet>(id);
        }
    }
}