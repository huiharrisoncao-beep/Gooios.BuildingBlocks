using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.Security;

namespace Gooios.BuildingBlocks.Extensions;

public static class StringExtension
{
    public static SecureString ToSecureString(this string content)
    {
        var result = new SecureString();
        var contentArr = content.ToCharArray();
        foreach (var c in contentArr)
        {
            result.AppendChar(c);
        }

        return result;
    }

    public static Bitmap ToImageFromBase64(this string base64Str)
    {
        string[] imgStr = base64Str.Split(',');
        var arr = Convert.FromBase64String(imgStr[1].Replace(" ", "+"));
        using (var ms = new MemoryStream(arr))
        {
            var bmp = new Bitmap(ms);
            return bmp;
        }

    }

    public static bool SaveImage(this string base64Str, string savePath)
    {
        var bitmap = base64Str.ToImageFromBase64();
        if (bitmap == null) return false;

        var floderPath = savePath.Substring(0, savePath.LastIndexOf('/'));
        if (!Directory.Exists(floderPath))
            Directory.CreateDirectory(floderPath);

        var suffix = savePath.Substring(savePath.LastIndexOf('.') + 1, savePath.Length - savePath.LastIndexOf('.') - 1).ToLower();
        var suffixName = suffix == "png" ? ImageFormat.Png :
                         suffix == "jpg" || suffix == "jpeg" ? ImageFormat.Jpeg :
                         suffix == "bmp" ? ImageFormat.Bmp :
                         suffix == "gif" ? ImageFormat.Gif : ImageFormat.Jpeg;

        var bmpNew = new Bitmap(bitmap);
        bmpNew.Save(savePath, suffixName);
        bmpNew.Dispose();

        bitmap.Dispose();

        return true;
    }
}