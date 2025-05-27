using System.Collections;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Harkh_backend.src.Controllers
{
    public class UsersProjectsController : CustomController
    {
        private Guid GetUserIdFromToken()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return Guid.Parse(userIdClaim.Value);
        }
        private readonly IUserProjectService _userProjectService;

        public UsersProjectsController(IUserProjectService userProjectService)
        {
            _userProjectService = userProjectService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsersProjectsReadDto>>> FindAll()
        {
            return Ok(await _userProjectService.FindAll());
        }

        [HttpGet("/api/v1/projectUsers/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable>> GetProjectUsers(Guid projectId)
        {
            var result = await _userProjectService.GetProjectUsers(projectId);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpGet("/api/v1/GetProjects/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable>> GetUserProjects(Guid userId, [FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
        {
            var result = await _userProjectService.GetUserProjects(userId, limit, offset);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsersProjectsReadDto>> CreateOne([FromBody] UsersProjectsCreateDto newUserProject)
        {
            if (newUserProject == null) return BadRequest();
            var createNewUserProject = await _userProjectService.CreateOne(newUserProject);
            if (createNewUserProject == null) return BadRequest();
            return CreatedAtAction(nameof(CreateOne), createNewUserProject);
        }

        [HttpDelete("{id}/{projectId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOne(Guid id, Guid projectId)
        {
            var findResult = await _userProjectService.DeleteOne(id, projectId);
            if (findResult == false) return NotFound();
            return NoContent();
        }



        [HttpGet("InsightCards/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> InsightCards(Guid projectId)
        {
            var cards = await _userProjectService.InsightsCards(projectId);
            if (cards == null) return NotFound();
            return Ok(cards);
        }
        [HttpGet("NumberOfProjects/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> NumberOfProject(Guid userId)
        {
            var noProjects = await _userProjectService.NumberOfProject(userId);
            if (noProjects == null) return NotFound();
            return Ok(noProjects);
        }

        [HttpGet("FindManager/{projectId}/{managerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> FindOne(Guid projectId, Guid? managerId)
        {
            var isManager = await _userProjectService.FindManager(projectId, managerId);
            if (isManager == false) return NotFound();
            return Ok(isManager);
        }
    }
}