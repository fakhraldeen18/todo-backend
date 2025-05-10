using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;


namespace Harkh_backend.src.Controllers;

public class ProjectsController : CustomController
{

    private Guid GetUserIdFromToken()
    {
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");

        return Guid.Parse(userIdClaim.Value);
    }
    private readonly IProjectService _projectService;
    private readonly IUserProjectService _userProjectService;

    public ProjectsController(IProjectService projectService, IUserProjectService userProjectService)
    {
        _projectService = projectService;
        _userProjectService = userProjectService;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "TeamMember,ProjectManager,Admin")]
    public async Task<ActionResult<IEnumerable>> GetUserProjects([FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
    {
        var userId = GetUserIdFromToken();
        var result = await _userProjectService.GetUserProjects(userId, limit, offset);
        if (result == null) return NotFound();
        return Ok(result);
    }


    [HttpGet("{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FindOne(Guid projectId)
    {
        var project = await _userProjectService.FindOne(projectId);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProjectReadDto>> CreateOne([FromBody] ProjectCreateDto newProject)
    {
        if (newProject == null) return BadRequest();
        ProjectReadDto? caretProject = await _projectService.CreateOne(newProject);
        return CreatedAtAction(nameof(CreateOne), caretProject);
    }

    [HttpPatch("{projectId}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectReadDto>> UpdateOne(Guid projectId, [FromBody] ProjectUpdateDto updateProject)
    {
        ProjectReadDto? findProject = await _projectService.FindOne(projectId);
        if (findProject == null) return NotFound();
        ProjectReadDto? updatedProject = await _projectService.UpdateOne(projectId, updateProject);
        return Accepted(updatedProject);
    }

    [HttpPost("document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DocumentReadDto>> CreteDocument([FromBody] DocumentCreateDto newDocument)
    {
        if (newDocument == null) return BadRequest();
        DocumentReadDto? createdDocument = await _projectService.CreateDocument(newDocument);
        return CreatedAtAction(nameof(CreteDocument), createdDocument);
    }

    [HttpGet("documents/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllDocuments(Guid projectId)
    {
        var findProject = await _projectService.FindOne(projectId);
        if (findProject == null) return NotFound();
        var documents = await _projectService.GetDocuments(projectId);
        return Ok(documents);
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "ProjectManager,Admin")]
    public async Task<ActionResult> DeleteOne(Guid projectId)
    {
        ProjectReadDto? findProject = await _projectService.FindOne(projectId);
        if (findProject == null) return NotFound();
        await _projectService.DeleteOne(projectId);
        return NoContent();
    }

     [HttpGet("NumberOfProjects/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> NumberOfProject(Guid userId)
        {
            var noProjects = await _userProjectService.NumberOfProject(userId);
            return Ok(noProjects);
        }

    [HttpDelete("member/{memberId}/{projectId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "ProjectManager,Admin")]
    public async Task<ActionResult> DeleteOne(Guid memberId, Guid projectId)
    {
        var findResult = await _userProjectService.DeleteOne(memberId, projectId);
        if (findResult == false) return NotFound();
        return NoContent();
    }
}
