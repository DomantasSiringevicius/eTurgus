using Backend.Application.Interfaces;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> hasher = new PasswordHasher<User>();

    public string Hash(User user, string password)
    {
        return hasher.HashPassword(user, password);
    }

    public bool Verify(User user, string password)
    {
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        return result != PasswordVerificationResult.Failed;
    }
}