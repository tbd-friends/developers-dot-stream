using System.Security.Cryptography;

namespace Developers.Stream.Shared_Kernel.Infrastructure;

public class RandomString(int length = 32)
{
    private const string Chars = "abcdefghijklmnopqrstuvwxyz0123456789";

    private readonly string _content = RandomNumberGenerator.GetString(Chars, length);

    public static implicit operator string(RandomString randomString) => randomString._content;
}