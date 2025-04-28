using System.Collections;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Harkh_backend.src.Controllers
{
    public class UsersProjectsController : CustomController
    {
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
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable>> GetUserProjects(Guid userId,[FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
        {
            var result = await _userProjectService.GetUserProjects(userId,limit, offset);
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
            return CreatedAtAction(nameof(CreateOne), createNewUserProject);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOne(Guid id)
        {
            var findResult = await _userProjectService.DeleteOne(id);
            if (findResult == false) return NotFound();
            return NoContent();
        }
    }
}