using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using SimpleProde.Services;
using SimpleProde.Utilities;

namespace SimpleProde.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        public const string cacheTag = "team";
        private ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<List<TeamDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = await teamService.Get(pagination);
            HttpContext.InsertPaginationParametersInHeaderByCounter(queryable.Count); 
            
            return Ok(queryable);
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<List<TeamDTO>>> GetById(int id)
        {
            var team = await teamService.GetById(id);
            if (team is null) return NotFound();
            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TeamCreateDTO teamCreateDTO)
        {
            var id = await teamService.Create(teamCreateDTO);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] TeamCreateDTO teamCreateDTO)
        {
            var updated = await teamService.Update(id, teamCreateDTO);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await teamService.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}