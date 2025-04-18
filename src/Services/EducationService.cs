using System.Collections;
using AutoMapper;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class EducationService : IEducationService


{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Education> _educationRepository;

    public EducationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = _unitOfWork.Users;
        _educationRepository = _unitOfWork.Educations;
    }

    public async Task<IEnumerable<EducationReadDto>> FindAll()
    {
        var educations = await _educationRepository.FindAll();
        var readEducations = _mapper.Map<IEnumerable<EducationReadDto>>(educations);
        return readEducations;
    }


    public async Task<IEnumerable?> FindWithUser(Guid userId)
    {
        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;
        var educations = await _educationRepository.FindAll();
        var users = await _userRepository.FindAll();
        var educationWithUser = from education in educations
                                join user in users
                                on education.UserId equals user.Id
                                where user.Id == userId
                                select new
                                {
                                    userName = user.Name,
                                    school = education.School,
                                    degree = education.Degree,
                                    fieldStudy = education.FieldStudy,
                                    grade = education.Grade,
                                    startDate = education.StartDate,
                                    endDate = education.EndDate

                                };
        return educationWithUser;
    }

    public async Task<EducationReadDto?> CreateOne(EducationCreateDto newEducation)
    {
        if (newEducation == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            var education = _mapper.Map<Education>(newEducation);
            await _educationRepository.CreateOne(education);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<EducationReadDto>(education);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }

    }

    public async Task<bool> DeleteOne(Guid id)
    {
        var findEducation = await _educationRepository.FindOne(id);
        if (findEducation == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _educationRepository.DeleteOne(findEducation);
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



    public async Task<EducationReadDto?> UpdateOne(Guid id, EducationUpdateDto updateEducation)
    {
        var education = await _educationRepository.FindOne(id);
        if (education == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            education.School = updateEducation.School;
            education.Degree = updateEducation.Degree;
            education.FieldStudy = updateEducation.FieldStudy;
            education.Grade = updateEducation.Grade;
            education.StartDate = updateEducation.StartDate;
            education.EndDate = updateEducation.EndDate;
            _educationRepository.UpdateOne(education);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<EducationReadDto>(education);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
}
