using System.Collections;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;

namespace Harkh_backend.src.Abstractions;

public interface IUserSkillService
{

    public Task<IEnumerable<UserSkillReadDto>> FindAll();
    public Task<UserSkillReadDto?> CreateOne(UserSkillCreateDto newUserSkill);
    public Task<IEnumerable<UserSkillReadDto>?> CreateRange(Guid userId, IEnumerable<UserSkillCreateRangeDto> newTasks);
    public Task<IEnumerable?> GetUserSkills(Guid userId);
    public Task<bool> DeleteOne(Guid id);
    public Task<bool> DeleteRange(Guid userId, IEnumerable<UserSkillCreateRangeDto> newSkills);

}
