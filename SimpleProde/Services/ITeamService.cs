using SimpleProde.DTOs;

namespace SimpleProde.Services
{
    public interface ITeamService
    {
        Task<List<TeamDTO>> Get(PaginationDTO pagination);
        Task<TeamDTO?> GetById(int id);
        Task<int> Create(TeamCreateDTO teamCreateDTO);
        Task<bool> Update(int id, TeamCreateDTO teamCreateDTO);
        Task<bool> Delete(int id);
    }
}
