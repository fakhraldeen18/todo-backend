using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Harkh_backend.src.Controllers;
public class UsersSkillsController : CustomController
{
    private readonly IUserSkillService _userSkillService;
    private readonly IUserService _userService;

    public UsersSkillsController(IUserSkillService userSkillService, IUserService userService)
    {
        _userSkillService = userSkillService;
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserSkillReadDto>>> FindAll()
    {
        return Ok(await _userSkillService.FindAll());
    }
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserSkill(Guid userId)
    {
        var result = await _userSkillService.GetUserSkills(userId);
        if (result == null) return NotFound();
        return Ok(result);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserSkillReadDto>> CreateOne([FromBody] UserSkillCreateDto newUserSkill)
    {
        if (newUserSkill == null) return BadRequest();
        var createdUserSkill = await _userSkillService.CreateOne(newUserSkill);
        return CreatedAtAction(nameof(CreateOne), createdUserSkill);
    }

    [HttpPost("range/{userId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserSkillReadDto>> CreateRange(Guid userId,[FromBody] IEnumerable<UserSkillCreateRangeDto> userTasks)
    {
        var findUsers = await _userService.FindOne(userId);
        if (findUsers == null) return NotFound();

        if (userTasks == null) return BadRequest();
        var creatRange = await _userSkillService.CreateRange(userId, userTasks);
        if (creatRange == null) return BadRequest();
        return CreatedAtAction(nameof(CreateRange), creatRange);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOne(Guid id)
    {
        var result = await _userSkillService.DeleteOne(id);
        if (result == false) return NotFound();
        return NoContent();
    }

    [HttpDelete("range/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteRange(Guid userId, [FromBody] IEnumerable<UserSkillCreateRangeDto> userSkills)
    {
        var findUsers = await _userService.FindOne(userId);
        if (findUsers == null) return NotFound();

        if (userSkills == null) return BadRequest();
        var result = await _userSkillService.DeleteRange(userId, userSkills);
        if (result == false) return NotFound();
        return NoContent();
    }
}
