using AutoMapper;
using Harkh_app_production.src.Abstractions;
using Harkh_app_production.src.Utils;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class InvitationService : IInvitationService
{
    private IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Invitation> _invitationRepository;
    private readonly IMapper _mapper;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IUserService _userService;
    private readonly IUserProjectService _userProjectRepository;

    public InvitationService(IMapper mapper, IUnitOfWork unitOfWork, IEmailSenderService emailSenderService, IUserService userService, IUserProjectService userProjectRepository)
    {
        _unitOfWork = unitOfWork;
        _invitationRepository = _unitOfWork.Invitations;
        _mapper = mapper;
        _emailSenderService = emailSenderService;
        _userService = userService;
        _userProjectRepository = userProjectRepository;
    }

    public async Task<InvitationReadeDto?> CreateOne(InvitationDto newInvitation)
    {
        if (newInvitation == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            Invitation sendInvitation = _mapper.Map<Invitation>(newInvitation);
            await _invitationRepository.CreateOne(sendInvitation);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<InvitationReadeDto>(sendInvitation);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }


    public async Task<IEnumerable<InvitationReadeDto>> FindAll()
    {
        IEnumerable<Invitation> invitations = await _invitationRepository.FindAll();
        return _mapper.Map<IEnumerable<InvitationReadeDto>>(invitations);
    }



    public async Task<bool> ProcessInvitationAsync(InvitationDto Invitation)
    {

        var users = await _userService.FindAll();
        if (string.IsNullOrWhiteSpace(Invitation.ToEmail))
        {
            throw CustomException.BadRequest("Email is required");
        }
        if (users.Any(u => u.Email.Equals(Invitation.ToEmail, StringComparison.OrdinalIgnoreCase)))
        {
            throw CustomException.BadRequest("Email already register please try another one");
        }
        await _unitOfWork.BeginTransaction();
        try
        {
            var newUser = await _userService.CreateInviteUser(Invitation.ToEmail);
            // string guidString = Invitation.ProjectLink.Split('/').Last();
            // Guid invitationGuid;
            // if (!Guid.TryParse(guidString, out invitationGuid))
            // {
            //     throw new ArgumentException("Invalid invitation link format");
            // }
            var newProjectUser = new UsersProjectsCreateDto
            {
                ProjectId = Invitation.ProjectId,
                UserId = newUser!.Id
            };
            await _userProjectRepository.CreateOne(newProjectUser);

            // Save to database
            Invitation.ProjectLink = $"http://localhost:3000/invitation/{newUser.Id}";
            await CreateOne(Invitation);

            // Send email
            var emailRequest = new EmailSender
            {
                SenderID = Invitation.UserId,
                ToEmail = $"{Invitation.ToEmail}", // Replace with recipient's email
                Subject = $"You're invited to join {Invitation.ProjectName} on Haraka! 🚀",
                PlainTextContent = $"Hi {Invitation.Name},\n\nYou’ve been added to \"{Invitation.ProjectName}\" on Haraka!\n\n" +
                          $"🔸 Access the project here: {Invitation.ProjectLink}\n" +
                          $"🔸 Get started in seconds.\n\n" +
                          $"Cheers,\n{Invitation.YourName}",
                HtmlContent = $@"
        <p>Hi {Invitation.ToEmail},</p>
        <p>You’ve been added to <strong>{Invitation.ProjectName}</strong> on Haraka!</p>
        <ul>
            <li>🔸 <a href='{Invitation.ProjectLink}'>Access the project here</a></li>
            <li>🔸 Get started in seconds.</li>
        </ul>
        <p>Cheers,<br/>{Invitation.YourName}</p>"
            };

            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return await _emailSenderService.SendEmailAsync(emailRequest);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return false;
        }
    }
}
