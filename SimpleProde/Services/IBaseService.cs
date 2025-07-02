using SimpleProde.DTOs;

namespace SimpleProde.Services
{
    public interface IBaseService<TDTO, TCreateDTO>
    {
        Task<List<TDTO>> Get(PaginationDTO pagination);
        Task Create(TCreateDTO dto);
        Task Update(int id, TCreateDTO dto);
        Task Delete(int id);
    }
}