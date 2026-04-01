using ResidentialExpenses.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace ResidentialExpenses.Infrastructure.Security.Cryptography;

internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);

        return passwordHash;
    }

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}