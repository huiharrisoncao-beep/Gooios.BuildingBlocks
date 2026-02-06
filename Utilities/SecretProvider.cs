using System.Security.Cryptography;
using System.Text;

namespace Gooios.BuildingBlocks.Utilities;

public class SecretProvider
{
    public string ToMD5(string input)
    {
        using var md5 = MD5.Create();
        byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(data).ToLowerInvariant();
    }

    public string Sign(string content, string privateKey)
    {
        byte[] bt = Encoding.UTF8.GetBytes(content);
        using var sha256 = SHA256.Create();
        byte[] rgbHash = sha256.ComputeHash(bt);

        using var key = new RSACryptoServiceProvider();
        key.ImportFromPem(privateKey);
        var formatter = new RSAPKCS1SignatureFormatter(key);
        formatter.SetHashAlgorithm("SHA256");
        byte[] inArray = formatter.CreateSignature(rgbHash);
        return Convert.ToBase64String(inArray);
    }

    public bool CheckSign(string content, string sign, string publicKey)
    {
        byte[] bt = Encoding.UTF8.GetBytes(content);
        using var sha256 = SHA256.Create();
        byte[] rgbHash = sha256.ComputeHash(bt);

        using var key = new RSACryptoServiceProvider();
        key.ImportFromPem(publicKey);
        var deformatter = new RSAPKCS1SignatureDeformatter(key);
        deformatter.SetHashAlgorithm("SHA256");
        var rgbSignature = Convert.FromBase64String(sign);
        return deformatter.VerifySignature(rgbHash, rgbSignature);
    }
}