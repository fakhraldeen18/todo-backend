using Todo_backend.src.DTOs;
using Todo_backend.src.Entities;

namespace Todo_backend.src.Abstractions;

public interface IUserService
{


    public IEnumerable<UserReadDto> FindAll();
    public UserReadDto? FindOne(Guid id);
    public string? Login(UserLogInDto user);
    public UserReadDto? SignUp(UserCreateDto user);
    public bool DeleteOne(Guid id);
    public UserReadDto? UpdateOne(Guid id, UserUpdateDto updatedUser);

}
