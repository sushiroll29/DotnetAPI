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

    IMapper _mapper;

    public UserEFController(IConfiguration config)
    {
        _entityFramework = new EFContext(config);
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

        if(_entityFramework.SaveChanges() > 0)
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
        
        _entityFramework.Add(userDb);

        if(_entityFramework.SaveChanges() > 0)
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
            _entityFramework.Users.Remove(userDb);
            if(_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
        throw new Exception("Failed to delete user.");
        }

        throw new Exception("Failed to get user.");
    }
    
}
