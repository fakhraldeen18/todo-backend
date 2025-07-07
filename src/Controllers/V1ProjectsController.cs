using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Harkh_backend.src.Controllers;

public class V1ProjectsController : CustomController
{

    private readonly IProjectService _projectService;
    private readonly IDocumentService _documentService;

    public V1ProjectsController(IProjectService projectService, IDocumentService documentService)
    {
        _projectService = projectService;
        _documentService = documentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectReadDto>>> FindAll()
    {
        IEnumerable<ProjectReadDto> projects = await _projectService.FindAll();
        return Ok(projects);
    }

    [HttpGet("full-data/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProjectFullDataDto>>> FindAllFullData(Guid id)
    {
        ProjectFullDataDto projects = await _projectService.FindAllFullData(id);
        if (projects == null) return NotFound();
        return Ok(projects);
    }
    [HttpGet("pagination")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProjectReadDto>>> FindAll([FromQuery(Name = "limit")] int limit, [FromQuery(Name = "offset")] int offset)
    {
        IEnumerable<ProjectReadDto> projects = await _projectService.FindAll(limit, offset);
        return Ok(projects);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateOneProject([FromBody] ProjectCreateDto newProject)
    {
        if (newProject == null) return BadRequest();
        var caretProject = await _projectService.CreateOneProject(newProject);
        return CreatedAtAction(nameof(CreateOneProject), caretProject);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectReadDto>> FindOne(Guid id)
    {
        ProjectReadDto? project = await _projectService.FindOne(id);
        if (project == null) return NotFound();
        return Ok(project);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOne(Guid id)
    {
        ProjectReadDto? findProject = await _projectService.FindOne(id);
        if (findProject == null) return NotFound();
        await _projectService.DeleteOne(id);
        return NoContent();
    }


    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectReadDto>> UpdateOne(Guid id, [FromBody] ProjectUpdateDto updateProject)
    {
        ProjectReadDto? findProject = await _projectService.FindOne(id);
        if (findProject == null) return NotFound();
        ProjectReadDto? updatedProject = await _projectService.UpdateOne(id, updateProject);
        return Accepted(updatedProject);
    }

    [HttpPatch("status/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectReadDto>> UpdateStatus(Guid id, [FromBody] ProjectUpdateStatusDto updateProjectStatus)
    {
        var findProject = _projectService.FindOne(id);
        if (findProject == null) return NotFound();
        ProjectReadDto? updatedProject = await _projectService.UpdateStatus(id, updateProjectStatus);
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

    [HttpGet("milestones/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ProjectJoinMilestoneDto>>> GetAllMilestones(Guid id)
    {
        var findProject = await _projectService.FindOne(id);
        if (findProject == null) return NotFound();
        var milestones = await _projectService.GetMilestones(id);
        return Ok(milestones);
    }
    [HttpGet("documents/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllDocuments(Guid id)
    {
        var findProject = await _projectService.FindOne(id);
        if (findProject == null) return NotFound();
        var documents = await _projectService.GetDocuments(id);
        return Ok(documents);
    }
}
