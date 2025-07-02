using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SimpleProde.DTOs;
using SimpleProde.Entities;

namespace SimpleProde.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            ConfigureUserMapping();
            ConfigureBetMapping();
            ConfigureChampionshipMapping();
            ConfigureMatchMapping();
            ConfigureScoreMapping();
            ConfigureTeamMapping();
        }

        private void ConfigureUserMapping()
        {
            CreateMap<IdentityUser, UserDTO>();
        }

        private void ConfigureBetMapping()
        {
            CreateMap<BetCreateDTO, Bet>();
            CreateMap<Bet, BetDTO>();
        }

        private void ConfigureChampionshipMapping()
        {
            CreateMap<ChampionshipCreateDTO, Championship>();
            CreateMap<Championship, ChampionshipDTO>();
        }

        private void ConfigureMatchMapping()
        {
            CreateMap<MatchCreateDTO, Match>();
            CreateMap<Match, MatchDTO>();
        }

        private void ConfigureScoreMapping()
        {
            CreateMap<ScoreCreateDTO, Score>();
            CreateMap<Score, ScoreDTO>();
        }

        private void ConfigureTeamMapping()
        {
            CreateMap<TeamCreateDTO, Team>();
            CreateMap<Team, TeamDTO>();
        }

    }
}
