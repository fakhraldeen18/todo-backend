using Microsoft.AspNetCore.Mvc;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using System.Collections;

namespace Harkh_backend.src.Controllers;

public class TasksController : CustomController
{
    private readonly ITaskService _TaskService;
    private readonly IDocumentService _documentService;

    public TasksController(ITaskService TaskService, IDocumentService documentService)
    {
        _TaskService = TaskService;
        _documentService = documentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> FindAll()
    {
        IEnumerable<TaskReadDto>? tasks = await _TaskService.FindAll();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> FindOne(Guid id)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        return Ok(findTask);
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable>> GetUserTasks(Guid userId)
    {
        var findTask = await _TaskService.GetUserTasks(userId);
        if (findTask == null) return NotFound();
        return Ok(findTask);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskReadDto>> CreateOne([FromBody] TaskCreteDto newTask)
    {
        if (newTask == null) return BadRequest();
        TaskReadDto? createdTask = await _TaskService.CreateOne(newTask);
        return CreatedAtAction(nameof(CreateOne), createdTask);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOne(Guid id)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        await _TaskService.DeleteOne(id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> UpdateOne(Guid id, [FromBody] TaskUpdateDto updateTask)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        TaskReadDto? updatedTask = await _TaskService.UpdateOne(id, updateTask);
        return Accepted(updatedTask);
    }

    [HttpPatch("status/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> UpdateStatus(Guid id, [FromBody] TaskUpdateStatusDto updateStatus)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        TaskReadDto? updatedTask = await _TaskService.UpdateStatus(id, updateStatus);
        return Accepted(updatedTask);
    }
    [HttpPatch("priority/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> UpdatePriority(Guid id, [FromBody] TaskUpdatePriorityDto updateProgress)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        TaskReadDto? updatedTask = await _TaskService.UpdatePriority(id, updateProgress);
        return Accepted(updatedTask);
    }

    [HttpPatch("milestone/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskReadDto>> UpdateMilestone(Guid id, [FromBody] TaskUpdateMilestoneDto updateMilestone)
    {
        var findTask = await _TaskService.FindOne(id);
        if (findTask == null) return NotFound();
        TaskReadDto? updatedTask = await _TaskService.UpdateMilestone(id, updateMilestone);
        return Accepted(updatedTask);
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
}
