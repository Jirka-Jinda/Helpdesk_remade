using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementations;

public class PasswordGeneratorService
{
    public static string GeneratePassword(PasswordOptions? opts = null)
    {
        if (opts is null) opts = new PasswordOptions
        {
            RequiredLength = 12,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true
        };

        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string nonAlphanumeric = "!@$?_-%^&*";

        var rand = RandomNumberGenerator.Create();
        var result = new StringBuilder();
        var chars = new List<char>();

        // Ensure all required types are included
        if (opts.RequireLowercase)
            chars.Add(GetRandomChar(lowercase, rand));
        if (opts.RequireUppercase)
            chars.Add(GetRandomChar(uppercase, rand));
        if (opts.RequireDigit)
            chars.Add(GetRandomChar(digits, rand));
        if (opts.RequireNonAlphanumeric)
            chars.Add(GetRandomChar(nonAlphanumeric, rand));

        // Fill the rest with random mix
        string allChars = "";
        if (opts.RequireLowercase) allChars += lowercase;
        if (opts.RequireUppercase) allChars += uppercase;
        if (opts.RequireDigit) allChars += digits;
        if (opts.RequireNonAlphanumeric) allChars += nonAlphanumeric;

        while (chars.Count < opts.RequiredLength)
            chars.Add(GetRandomChar(allChars, rand));

        // Shuffle for randomness
        return new string(chars.OrderBy(x => RandomNumber(rand)).ToArray());
    }

    private static char GetRandomChar(string charset, RandomNumberGenerator rng)
    {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        int idx = BitConverter.ToInt32(data, 0) % charset.Length;
        return charset[Math.Abs(idx)];
    }

    private static int RandomNumber(RandomNumberGenerator rng)
    {
        byte[] data = new byte[4];
        rng.GetBytes(data);
        return Math.Abs(BitConverter.ToInt32(data, 0));
    }
}
