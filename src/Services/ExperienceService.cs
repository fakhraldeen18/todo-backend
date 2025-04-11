using AutoMapper;
using Harkh_backend.src.UnitOfWork;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using System.Collections;

namespace Harkh_backend.src.Services;

public class ExperienceService : IExperienceService
{
    private readonly IBaseRepository<Experience> _experienceRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExperienceService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _experienceRepository = _unitOfWork.Experiences;
        _userRepository = _unitOfWork.Users;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ExperienceReadDto>> FindAll()
    {
        var experiences = await _experienceRepository.FindAll();
        var readExperiences = _mapper.Map<IEnumerable<ExperienceReadDto>>(experiences);
        return readExperiences;
    }
    public async Task<IEnumerable?> FindWithUser(Guid userId)
    {
        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;
        var experiences = await _experienceRepository.FindAll();
        var users = await _userRepository.FindAll();
        var experienceWithUser = from experience in experiences
                                 join user in users
                                 on experience.UserId equals user.Id
                                 where user.Id == userId
                                 select new
                                 {
                                     userName = user.Name,
                                     title = experience.Title,
                                     companyName = experience.CompanyName,
                                     description = experience.Description,
                                     employmentType = experience.EmploymentType,
                                     startDate = experience.StartDate,
                                     endDate = experience.EndDate
                                 };
        return experienceWithUser;
    }

    public async Task<ExperienceReadDto?> CreateOne(ExperienceCreateDto newExperience)
    {
        if (newExperience == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            var experience = _mapper.Map<Experience>(newExperience);
            await _experienceRepository.CreateOne(experience);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<ExperienceReadDto>(experience);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }


    public async Task<bool> DeleteOne(Guid id)
    {
        var findExperience = await _experienceRepository.FindOne(id);
        if (findExperience == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _experienceRepository.DeleteOne(findExperience);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return false;
        }
    }

    public async Task<ExperienceReadDto?> UpdateOne(Guid id, ExperienceUpdateDto updateExperience)
    {
        var experience = await _experienceRepository.FindOne(id);
        if (experience == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            experience.Title = updateExperience.Title;
            experience.CompanyName = updateExperience.CompanyName;
            experience.Description = updateExperience.Description;
            experience.EmploymentType = updateExperience.EmploymentType;
            experience.StartDate = updateExperience.StartDate;
            experience.EndDate = updateExperience.EndDate;
            _experienceRepository.UpdateOne(experience);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<ExperienceReadDto>(experience);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
}
