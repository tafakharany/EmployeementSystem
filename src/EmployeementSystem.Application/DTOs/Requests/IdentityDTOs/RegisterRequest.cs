using EmploymentSystem.Domain.lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;

public class RegisterRequestDto : BaseIdentityRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public UserType UserType { get; set; }
}
