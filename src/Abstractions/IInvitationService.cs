using Harkh_backend.src.DTOs;

namespace Harkh_app_production.src.Abstractions;

public interface IInvitationService
{
    public Task<InvitationReadeDto?> CreateOne(InvitationDto newInvitation);
    public Task<IEnumerable<InvitationReadeDto>> FindAll();
    public Task<bool> ProcessInvitationAsync(InvitationDto Invitation);

}
