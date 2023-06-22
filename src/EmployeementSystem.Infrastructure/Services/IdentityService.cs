using AutoMapper;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Infrastructure.Persistence;
using EmploymentSystem.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmploymentSystem.Application.DTOs.Common;
using Microsoft.EntityFrameworkCore.Storage;
using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;

namespace EmploymentSystem.Infrastructure.Services;


public class IdentityService : BaseService<IdentityService>, IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Injecting Services needed to Register new user and Login 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="context"></param>
    public IdentityService(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        ILogger<IdentityService> logger,
        IConfiguration configuration, IApplicationDbContext context
        ) : base(mapper, logger, context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;

        _roleManager = roleManager;
    }

    /// <summary>
    /// Endpoint for Login 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<LoginResponseDto> Login(LoginRequestDto request)
    {
        LoginResponseDto response = new()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.WrongData
        };
        try
        {
            User? user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.ResponseMessage = Resource.UserNotFound;
                response.ResponseCode = ResponseCodes.NotFound;
                return response;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var token = await GenerateTokenAsync(user);
                response.ExpiresIn = 3600;
                response.AccessToken = token;
                response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
                response.ResponseMessage = Resource.Sucess;
            }

        }
        catch (Exception ex)
        {
            return (LoginResponseDto)LogException(ex);
        }
        return response;
    }

    /// <summary>
    /// Endpoint for registering new User
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request)
    {

        var user = Mapper.Map<User>(request);
        var role = request.UserType.ToString();
        return await RegisterUserAsync(user, request.Password, role);

    }


    #region Utils
    private IEnumerable<string> SetErrorMessage(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
            yield return error.Description;
    }

    private async Task<string> GenerateTokenAsync(User? user)
    {

        var role = await _userManager.GetRolesAsync(user);

        var claims = new[]
        {
                    new Claim(ClaimTypes.Email,user?.Email),
                    new Claim(ClaimTypes.Actor, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role?.FirstOrDefault())
                };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }


    private async Task<RegisterResponseDto> RegisterUserAsync(User user,
        string password,
        string role)
    {
        RegisterResponseDto response = new()
        {
            ResponseMessage = Resource.Sucess,
            ResponseCode = ResponseCodes.ProcessedSuccessfully
        };
        await using var transaction = await Context.Transaction;
        try
        {
            var registered = await _userManager.CreateAsync(user, password);

            if (!registered.Succeeded)
            {
                await transaction.RollbackAsync();
                response.ResponseMessage = "Error While Registering the user" +
                                           string.Join(',', SetErrorMessage(registered.Errors));
                SerializedResponse = JsonConvert.SerializeObject(response);
                Logger.LogError(SerializedResponse);
                return response;
            }

            return await AssignRoleAsync(user, role, transaction);

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (RegisterResponseDto)LogException(ex);
        }
        return response;
    }


    private async Task<RegisterResponseDto> AssignRoleAsync(User user, string role, IDbContextTransaction transaction)
    {
        RegisterResponseDto response = new()
        {
            ResponseMessage = Resource.Sucess,
            ResponseCode = ResponseCodes.ProcessedSuccessfully
        };
        try
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                response.ResponseCode = ResponseCodes.FailedToProcess;
                response.ResponseMessage = Resource.GeneralError;
                Logger.LogError("Role is not existing");
                return response;
            }
            await _userManager.AddToRoleAsync(user, role);
            response.ResponseMessage = Resource.Sucess;
            response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (RegisterResponseDto)LogException(ex);
        }
        return response;
    }


    #endregion
}
