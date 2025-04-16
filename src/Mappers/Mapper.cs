using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using AutoMapper;

namespace Harkh_backend.src.Mappers;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<User, UserReadDto>();
        CreateMap<UserCreateDto, User>();

        CreateMap<Entities.Task, TaskReadDto>();
        CreateMap<TaskCreteDto, Entities.Task>();

        CreateMap<Project, ProjectReadDto>();
        CreateMap<ProjectCreateDto, Project>();

        CreateMap<Milestone, MilestoneReadDto>();
        CreateMap<MilestoneCreateDto, Milestone>();

        CreateMap<Document, DocumentReadDto>();
        CreateMap<DocumentCreateDto, Document>();

        CreateMap<Experience, ExperienceReadDto>();
        CreateMap<ExperienceCreateDto, Experience>();

        CreateMap<EducationCreateDto, Education>();
        CreateMap<Education, EducationReadDto>();

        CreateMap<Skill, SkillReadDto>();
        CreateMap<SkillCreateDto, Skill>();

        CreateMap<UserSkillCreateDto, UserSkill>();
        CreateMap<UserSkill, UserSkillReadDto>();
        CreateMap<UserSkillCreateRangeDto, UserSkillCreateDto>();

        CreateMap<UsersProjectsCreateDto, UserProject>();

        CreateMap<InvitationDto, Invitation>();
        CreateMap<Invitation, InvitationReadeDto>();
    }
}
