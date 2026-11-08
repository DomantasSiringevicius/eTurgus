using Backend.Application.Dtos.Auth;
using MediatR;

namespace Backend.Application.UseCases.Auth.Register;

public class RegisterCommand : IRequest<UserDto>
{
    public required string Email { get; set; }
    
    public required string UserName { get; set; }
    
    public required string Password { get; set; }
    
    public required string Role { get; set; }
}