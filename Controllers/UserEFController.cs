using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using AutoMapper;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{

    EFContext _entityFramework;
    IUserRepository _userRepository;

    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new EFContext(config);

        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.CreateMap<UserToAddDTO, User>();
        }));
        // Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if(user != null)
        {
            return user;
        }

        throw new Exception("Failed to get user.");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityFramework.Users
        .Where(u => u.UserId == user.UserId)
        .FirstOrDefault<User>();

        if(userDb != null)
        {
            userDb.Active = user.Active;
            userDb.Email = user.Email;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Gender = user.Gender;

        if(_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to update user.");
        }

        throw new Exception("Failed to get user.");
        
        
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDTO user)
    { 
        User userDb = _mapper.Map<User>(user);
        
        _userRepository.AddEntity<User>(userDb);

        if(_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to add user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if(userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }
        throw new Exception("Failed to delete user.");
        }

        throw new Exception("Failed to get user.");
    }

    [HttpGet("UserSalary/{userId}")]
    public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
    {
        return _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserSalary")]
    public IActionResult AddUserSalaryEF(UserSalary userToAdd)
    {
        _userRepository.AddEntity<UserSalary>(userToAdd);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding user salary failed on save.");
    }


    [HttpPut("UserSalary")]
    public IActionResult EditUserSalaryEF(UserSalary userToEdit)
    {
        UserSalary? userToUpdate = _entityFramework.UserSalary
            .Where(u => u.UserId == userToEdit.UserId)
            .FirstOrDefault();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userToEdit);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Editing user salary failed on save.");
        }
        throw new Exception("Failed to find a user salary to edit.");
    }


    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalaryEF(int userId)
    {
        UserSalary? userToDelete = _entityFramework.UserSalary
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserSalary>(userToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting user salary failed on save.");
        }
        throw new Exception("Failed to find a user salary to delete.");
    }


    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfoEF(int userId)
    {
        return _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .ToList();
    }

    [HttpPost("UserJobInfo")]
    public IActionResult AddUserJobInfoEF(UserJobInfo userToAdd)
    {
        _userRepository.AddEntity<UserJobInfo>(userToAdd);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding user job info failed on save.");
    }


    [HttpPut("UserJobInfo")]
    public IActionResult EditUserJobInfoEF(UserJobInfo userToEdit)
    {
        UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userToEdit.UserId)
            .FirstOrDefault();

        if (userToUpdate != null)
        {
            _mapper.Map(userToUpdate, userToEdit);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Updating user job info failed on save.");
        }
        throw new Exception("Failed to find a user job info to edit.");
    }


    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfoEF(int userId)
    {
        UserJobInfo? userToDelete = _entityFramework.UserJobInfo
            .Where(u => u.UserId == userId)
            .FirstOrDefault();

        if (userToDelete != null)
        {
            _userRepository.RemoveEntity<UserJobInfo>(userToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting user job info failed on save.");
        }
        throw new Exception("Failed to find a user job info to delete.");
    }
    
}
