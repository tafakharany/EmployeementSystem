using AutoMapper;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmploymentSystem.Infrastructure.Services;

public class BaseService<T> where T : class
{
    protected IMapper Mapper { get; }
    protected ILogger<T> Logger { get; }
    protected string SerializedResponse = string.Empty;
    protected IApplicationDbContext Context { get; set; }

    protected BaseService(IMapper mapper, ILogger<T> logger,  IApplicationDbContext context
    )
    {
        Mapper = mapper;
        Logger = logger;
        Context = context;
    }

    protected ResponseDto LogException(Exception ex)
    {
        ResponseDto response = new();
        Logger.LogError(ex.Message, ex.StackTrace);
        response.ResponseMessage = Resource.GeneralError;
        response.ResponseCode = ResponseCodes.GeneralError;
        SerializedResponse = JsonConvert.SerializeObject(response);
        Logger.LogError(SerializedResponse);
        return response;
    }
}
