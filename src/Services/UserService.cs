using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.Utils;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<User> _userRepository;
    private IConfiguration _config;
    private readonly IMapper _mapper;


    public UserService(IConfiguration config, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.Users;
        _config = config;
        _mapper = mapper;
    }

    public async Task<bool> DeleteOne(Guid id)
    {
        User? findUser = await _userRepository.FindOne(id);
        if (findUser == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _userRepository.DeleteOne(findUser);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return false;
        }
    }

    public async Task<IEnumerable<UserReadDto>> FindAll()
    {
        IEnumerable<User> users = await _userRepository.FindAll();
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<UserReadDto?> FindOne(Guid id)
    {
        User? findUser = await _userRepository.FindOne(id);
        if (findUser == null) return null;
        return _mapper.Map<UserReadDto>(findUser);
    }

    public async Task<string?> Login(UserLogInDto user)
    {
        IEnumerable<User>? users = await _userRepository.FindAll();
        User? isUser = users.FirstOrDefault(u => u.Email == user.Email);
        if (isUser == null) return null;
        byte[] pepper = Encoding.UTF8.GetBytes(_config["Jwt_Pepper"]!);
        bool isCorrect = PasswordUtils.VerifyPassword(user.Password, isUser.Password, pepper);
        if (!isCorrect) return null;

        //Create Token 
        var claims = new[]
        {
                // new Claim(ClaimTypes.Name, isUser.Name),
                new Claim(ClaimTypes.Role, isUser.Role.ToString()),
                new Claim(ClaimTypes.Email, isUser.Email),
                new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()),
            };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt_SigningKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt_Issuer"]!,
            audience: _config["Jwt_Audience"]!,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
            );
        var tokenSettings = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenSettings;
    }

    public async Task<UserReadDto?> SignUp(UserCreateDto user)
    {
        await _unitOfWork.BeginTransaction();
        try
        {
            // Convert email to lowercase
            user.Email = user.Email.ToLower();
            byte[] pepper = Encoding.UTF8.GetBytes(_config["Jwt_Pepper"]!);
            PasswordUtils.HashPassword(user.Password, out string hashedPassword, pepper);
            user.Password = hashedPassword;
            User mappedUser = _mapper.Map<User>(user);
            User newUser = await _userRepository.CreateOne(mappedUser);
            UserReadDto readerUser = _mapper.Map<UserReadDto>(newUser);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return readerUser;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<UserReadDto?> CreateInviteUser(string inviteUserEmail)
    {
        await _unitOfWork.BeginTransaction();
        try
        {
            // Convert email to lowercase
            inviteUserEmail = inviteUserEmail.ToLower();
            UserInviteCreateDto createUser = new()
            {
                Email = inviteUserEmail
            };
            User mappedUser = _mapper.Map<User>(createUser);
            User newUser = await _userRepository.CreateOne(mappedUser);
            UserReadDto readerUser = _mapper.Map<UserReadDto>(newUser);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return readerUser;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<UserReadDto?> UpdateProfile(Guid id, UserUpdateProfileDto updatedUser)
    {
        User? user = await _userRepository.FindOne(id);
        if (user == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            user.Name = updatedUser.Name;
            user.Position = updatedUser.Position;
            user.ProfileImage = updatedUser.ProfileImage;
            _userRepository.UpdateOne(user);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<UserReadDto>(user);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<UserReadDto?> UpdatePersonalInfo(Guid id, UserUpdatePersonalInfoDto updatedUser)
    {
        User? user = await _userRepository.FindOne(id);
        if (user == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Nationality = updatedUser.Nationality;
            user.BirthDate = updatedUser.BirthDate;
            _userRepository.UpdateOne(user);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<UserReadDto>(user);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<UserReadDto?> UpdateRole(Guid id, UserUpdateRoleDto updatedUser)
    {
        User? user = await _userRepository.FindOne(id);
        if (user == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            user.Role = updatedUser.Role;
            _userRepository.UpdateOne(user);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<UserReadDto>(user);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<UserReadDto?> UpdateUserInvitePassWord(Guid id, UserInviteUpdatePassWordDto updatedUser)
    {
        User? user = await _userRepository.FindOne(id);
        if (user == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            byte[] pepper = Encoding.UTF8.GetBytes(_config["Jwt_Pepper"]!);
            PasswordUtils.HashPassword(updatedUser.Password, out string hashedPassword, pepper);
            user.Password = hashedPassword;
            _userRepository.UpdateOne(user);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<UserReadDto>(user);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
}
