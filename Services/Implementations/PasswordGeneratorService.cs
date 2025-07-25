using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementations;

public class PasswordGeneratorService
{
    private readonly PasswordOptions _options;

    public PasswordGeneratorService()
    {
        _options = new PasswordOptions
        {
            RequiredLength = 12,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true
        };
    }

    public PasswordGeneratorService(PasswordOptions options)
    {
        _options = options;   
    }

    public string GeneratePassword()
    {
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string nonAlphanumeric = "!@$?_-%^&*";

        var rand = RandomNumberGenerator.Create();
        var result = new StringBuilder();
        var chars = new List<char>();

        // Ensure all required types are included
        if (_options.RequireLowercase)
            chars.Add(GetRandomChar(lowercase, rand));
        if (_options.RequireUppercase)
            chars.Add(GetRandomChar(uppercase, rand));
        if (_options.RequireDigit)
            chars.Add(GetRandomChar(digits, rand));
        if (_options.RequireNonAlphanumeric)
            chars.Add(GetRandomChar(nonAlphanumeric, rand));

        // Fill the rest with random mix
        string allChars = "";
        if (_options.RequireLowercase) allChars += lowercase;
        if (_options.RequireUppercase) allChars += uppercase;
        if (_options.RequireDigit) allChars += digits;
        if (_options.RequireNonAlphanumeric) allChars += nonAlphanumeric;

        while (chars.Count < _options.RequiredLength)
            chars.Add(GetRandomChar(allChars, rand));

        // Shuffle for randomness
        return new string(chars.OrderBy(x => RandomNumber(rand)).ToArray());
    }

    private char GetRandomChar(string charset, RandomNumberGenerator rng)
    {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        int idx = BitConverter.ToInt32(data, 0) % charset.Length;
        return charset[Math.Abs(idx)];
    }

    private int RandomNumber(RandomNumberGenerator rng)
    {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        return Math.Abs(BitConverter.ToInt32(data, 0));
    }
}
