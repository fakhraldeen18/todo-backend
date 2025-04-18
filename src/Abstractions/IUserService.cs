using Harkh_backend.src.DTOs;

namespace Harkh_backend.src.Abstractions;

public interface IUserService
{


    public Task<IEnumerable<UserReadDto>> FindAll();
    public Task<UserReadDto?> FindOne(Guid id);
    public Task<string?> Login(UserLogInDto user);
    public Task<UserReadDto?> SignUp(UserCreateDto user);
    public Task<UserReadDto?> CreateInviteUser(string inviteUserEmail);
    public Task<bool> DeleteOne(Guid id);
    public Task<UserReadDto?> UpdateProfile(Guid id, UserUpdateProfileDto updatedUser);
    public Task<UserReadDto?> UpdatePersonalInfo(Guid id, UserUpdatePersonalInfoDto updatedUser);
    public Task<UserReadDto?> UpdateRole(Guid id, UserUpdateRoleDto updatedUser);
    public Task<UserReadDto?> UpdateUserInvitePassWord(Guid id, UserInviteUpdatePassWordDto updatedUser);
}
