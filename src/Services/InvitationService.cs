using AutoMapper;
using Harkh_app_production.src.Abstractions;
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

    public InvitationService(IMapper mapper, IUnitOfWork unitOfWork, IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _invitationRepository = _unitOfWork.Invitations;
        _mapper = mapper;
        _emailSenderService = emailSenderService;
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

        // Save to database
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

        return await _emailSenderService.SendEmailAsync(emailRequest);
    }
}
