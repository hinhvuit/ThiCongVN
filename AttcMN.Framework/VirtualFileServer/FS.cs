using Microsoft.AspNetCore.StaticFiles;

namespace AttcMN.Framework.VirtualFileServer;

/// <summary>
/// ???辣??掩
/// </summary>
[SuppressSniffer]
public static class FS
{
    /// <summary>
    /// ?寞?辣???隞嗥? ContentType ??MIME
    /// </summary>
    /// <param name="fileName">?辣??撣行?撅?</param>
    /// <param name="contentType">ContentType ??MIME</param>
    /// <returns></returns>
    public static bool TryGetContentType(string fileName, out string contentType)
    {
        return GetFileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
    }

    /// <summary>
    /// ????隞?ContentType ????
    /// </summary>
    /// <returns></returns>
    public static FileExtensionContentTypeProvider GetFileExtensionContentTypeProvider()
    {
        var fileExtensionProvider = new FileExtensionContentTypeProvider();
        fileExtensionProvider.Mappings[".iec"] = "application/octet-stream";
        fileExtensionProvider.Mappings[".patch"] = "application/octet-stream";
        fileExtensionProvider.Mappings[".apk"] = "application/vnd.android.package-archive";
        fileExtensionProvider.Mappings[".pem"] = "application/x-x509-user-cert";
        fileExtensionProvider.Mappings[".gzip"] = "application/x-gzip";
        fileExtensionProvider.Mappings[".7zip"] = "application/zip";
        fileExtensionProvider.Mappings[".jpg2"] = "image/jp2";
        fileExtensionProvider.Mappings[".et"] = "application/kset";
        fileExtensionProvider.Mappings[".dps"] = "application/ksdps";
        fileExtensionProvider.Mappings[".cdr"] = "application/x-coreldraw";
        fileExtensionProvider.Mappings[".shtml"] = "text/html";
        fileExtensionProvider.Mappings[".php"] = "application/x-httpd-php";
        fileExtensionProvider.Mappings[".php3"] = "application/x-httpd-php";
        fileExtensionProvider.Mappings[".php4"] = "application/x-httpd-php";
        fileExtensionProvider.Mappings[".phtml"] = "application/x-httpd-php";
        fileExtensionProvider.Mappings[".pcd"] = "image/x-photo-cd";
        fileExtensionProvider.Mappings[".bcmap"] = "application/octet-stream";
        fileExtensionProvider.Mappings[".properties"] = "application/octet-stream";
        fileExtensionProvider.Mappings[".m3u8"] = "application/x-mpegURL";
        return fileExtensionProvider;
    }
}
