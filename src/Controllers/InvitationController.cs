using Harkh_app_production.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Harkh_backend.src.Controllers;
public class InvitationController : CustomController
{
    private readonly IInvitationService _invitationService;

    public InvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DocumentReadDto>>> FindAll()
    {
        var invitations = await _invitationService.FindAll();
        return Ok(invitations);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InvitationDto Invitation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _invitationService.ProcessInvitationAsync(Invitation);

            if (result)
            {
                return Ok(new { message = "Message sent successfully" });
            }

            return StatusCode(500, "Error processing your request");
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
