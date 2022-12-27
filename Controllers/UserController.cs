using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.DTOs;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    DapperContext _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DapperContext(config);
        // Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
       string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET
                [FirstName] = '" + user.FirstName +
                "', [LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email +
                "', [Gender] = '" + user.Gender +
                "', [Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to update user.");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDTO user)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        ) VALUES (" +
            "'" + user.FirstName +
            "', '" + user.LastName +
            "', '" + user.Email +
            "', '" + user.Gender +
            "', '" + user.Active +
        "')";
        
        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to add user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users
        WHERE UserId = " + userId.ToString();

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to delete user.");
    }

    [HttpGet("UserSalary/{userId}")]

    public UserSalary GetUserSalary(int userId)
    {
        string sql = @"
            SELECT [UserId], 
                [Salary],
                [AvgSalary]
            FROM TutorialAppSchema.UserSalary
            WHERE UserId = " + userId.ToString();

    UserSalary salary = _dapper.LoadDataSingle<UserSalary>(sql);

    return salary;
    } 
    
    [HttpPost("UserSalary")]
    public IActionResult AddUserSalary(UserSalary userSalaryToAdd)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserSalary (
                UserId,
                Salary
            ) VALUES (" + userSalaryToAdd.UserId.ToString()
                + ", " + userSalaryToAdd.Salary
                + ")";

        if (_dapper.ExecuteSqlWithRowCount(sql) > 0)
        {
            return Ok(userSalaryToAdd);
        }
        throw new Exception("Adding user salary failed on save.");
    }

    [HttpPut("UserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalaryToEdit)
    {
        string sql = "UPDATE TutorialAppSchema.UserSalary SET Salary=" 
            + userSalaryToEdit.Salary
            + " WHERE UserId=" + userSalaryToEdit.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userSalaryToEdit);
        }
        throw new Exception("Editing user salary failed on save.");
    }

    [HttpDelete("UserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary WHERE UserId=" + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Deleting user salary failed on save.");
    }

    [HttpGet("UserJobInfo/{userId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"
            SELECT  UserJobInfo.UserId
                    , UserJobInfo.JobTitle
                    , UserJobInfo.Department
            FROM  TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId.ToString());
    }

    [HttpPost("UserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfoToAdd)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo (
                UserId,
                Department,
                JobTitle
            ) VALUES (" + userJobInfoToAdd.UserId
                + ", '" + userJobInfoToAdd.Department
                + "', '" + userJobInfoToAdd.JobTitle
                + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoToAdd);
        }
        throw new Exception("Adding user job info failed on save.");
    }

    [HttpPut("UserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfoToEdit)
    {
        string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department='" 
            + userJobInfoToEdit.Department
            + "', JobTitle='"
            + userJobInfoToEdit.JobTitle
            + "' WHERE UserId=" + userJobInfoToEdit.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoToEdit);
        }
        throw new Exception("Editing user job info failed on save.");
    }
    
    [HttpDelete("UserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserJobInfo 
                WHERE UserId = " + userId.ToString();
        
        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to delete user.");
    }
}
