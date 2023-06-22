using EmploymentSystem.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Response;

public class ResponseDto
{
    /// <summary>
    /// Default Constructor 
    /// </summary>
    public ResponseDto()
    {

    }
    public ResponseDto(string responseCode, string responseMessage)
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
    }
    public ResponseDto(string responseMessage)
    {
        ResponseCode = ResponseCodes.ProcessedSuccessfully;
        ResponseMessage = responseMessage;
    }

    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
}
