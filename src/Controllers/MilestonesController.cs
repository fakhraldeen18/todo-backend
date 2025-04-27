using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Harkh_backend.src.Controllers;
public class MilestonesController : CustomController
{
    private readonly IMilestoneService _milestoneService;
    private readonly IDocumentService _documentService;

    public MilestonesController(IMilestoneService milestoneService, IDocumentService documentService)
    {
        _milestoneService = milestoneService;
        _documentService = documentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MilestoneReadDto>>> FindAll()
    {
        var milestones = await _milestoneService.FindAll();
        return Ok(milestones);
    }

    [HttpGet("full-data/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MilestoneFullDataDto>> FindAllFullData(Guid id)
    {
        MilestoneFullDataDto milestone = await _milestoneService.FindAllFullData(id);
        if (milestone == null) return NotFound();
        return Ok(milestone);
    }
    [HttpGet("project-milestones/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MilestoneFullDataDto>> FindAllProjectMilestonesData(Guid projectId)
    {
        var milestone = await _milestoneService.FindAllProjectMilestonesData(projectId);
        if (milestone == null) return NotFound();
        return Ok(milestone);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MilestoneReadDto>> FindOne(Guid id)
    {
        var findMilestone = await _milestoneService.FindOne(id);
        if (findMilestone == null) return NotFound();
        return Ok(findMilestone);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MilestoneReadDto>> CreateOne([FromBody] MilestoneCreateDto newMilestone)
    {
        if (newMilestone == null) return BadRequest();
        var createMilestone = await _milestoneService.CreateOne(newMilestone);
        return CreatedAtAction(nameof(CreateOne), createMilestone);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOne(Guid id)
    {
        var findMilestone = await _milestoneService.FindOne(id);
        if (findMilestone == null) return NotFound();
        await _milestoneService.DeleteOne(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MilestoneReadDto>> UpdateOne(Guid id, [FromBody] MilestoneUpdateDto updateMilestone)
    {
        var milestone = await _milestoneService.FindOne(id);
        if (milestone == null) return NotFound();
        var updatedMilestone = _milestoneService.UpdateOne(id, updateMilestone);
        return Accepted(await updatedMilestone);
    }
    [HttpPost("document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DocumentReadDto>> CreteDocument([FromBody] DocumentCreateDto newDocument)
    {
        if (newDocument == null) return BadRequest();
        DocumentReadDto? createdDocument = await _documentService.CreateOne(newDocument);
        return CreatedAtAction(nameof(CreteDocument), createdDocument);
    }

    [HttpGet("tasks/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MilestoneJoinTaskDto>>> GetTasks(Guid id)
    {
        var findMilestone = await _milestoneService.FindOne(id);
        if (findMilestone == null) return NotFound();
        var milestoneJoinTask = await _milestoneService.GetTasks(id);
        return Ok(milestoneJoinTask);
    }

    [HttpGet("documents/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAllDocuments(Guid id)
    {
        var findMilestone = await _milestoneService.FindOne(id);
        if (findMilestone == null) return NotFound();
        var documents = await _milestoneService.GetDocuments(id);
        return Ok(documents);
    }
}
