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
    [Route("api/Score")]
    [ApiController]
    public class ScoreController : CustomBaseController
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public readonly IOutputCacheStore outputCacheStore;
        private readonly IUserService userservice;
        public readonly IStoreFiles storeFiles;
        public const string cacheTag = "Score";
        public const string container = "Score";


        public ScoreController(ApplicationDbContext context, IMapper mapper, IOutputCacheStore outputCacheStore, IUserService userservice)
            : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.userservice = userservice;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ScoreDTO>> Get([FromQuery] PaginationDTO pagination)
        {
            return await Get<Score, ScoreDTO>(pagination, orderBy: a => a.Points);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ScoreCreateDTO scoreCreateDTO)
        {
            GetUserId(scoreCreateDTO);
            return await Post<ScoreCreateDTO, Score>(scoreCreateDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] ScoreCreateDTO scoreCreateDTO)
        {
            GetUserId(scoreCreateDTO);
            return await Put<ScoreCreateDTO, Score>(id, scoreCreateDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Team>(id);
        }

        private async void GetUserId(ScoreCreateDTO scoreCreateDTO)
        {
            var userId = await userservice.GetUserId();
            scoreCreateDTO.UserId = userId;

        }
    }
}