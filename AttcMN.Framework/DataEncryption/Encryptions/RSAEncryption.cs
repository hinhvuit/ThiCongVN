using System.Security.Cryptography;
using System.Text;

namespace AttcMN.Framework.DataEncryption;

/// <summary>
/// RSA ??
/// </summary>
[SuppressSniffer]
public static class RSAEncryption
{
    /// <summary>
    /// ?? RSA 蝘
    /// </summary>
    /// <param name="keySize">憭批?敹◆銝?2048 ??16384 銋嚗?敹◆?質◤ 8 ?湧</param>
    /// <returns></returns>
    public static (string publicKey, string privateKey) GenerateSecretKey(int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }

    /// <summary>
    /// ??
    /// </summary>
    /// <param name="text">???捆</param>
    /// <param name="publicKey">?祇</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public static string Encrypt(string text, string publicKey, int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        rsa.FromXmlString(publicKey);

        var originalData = Encoding.Default.GetBytes(text);
        byte[] encryptedData;

        // 撖?臬?撖?桅摨?
        var bufferSize = (rsa.KeySize / 8) - 11;

        // RSA 蝞?閫?
        // 敺?撖?摮??唬??質?餈??亦??踹漲?潮隞?8 ????11嚗嚗SACryptoServiceProvider.KeySize / 8 - 11嚗?
        // ??撖?敺撖????嚗迤憟賣撖?摨血潮隞?8嚗嚗SACryptoServiceProvider.KeySize / 8嚗?
        if (originalData.Length > bufferSize)
        {
            // ?挾??
            var buffer = new byte[bufferSize];
            using var input = new MemoryStream(originalData);
            using var output = new MemoryStream();

            while (true)
            {
                var readLine = input.Read(buffer, 0, bufferSize);
                if (readLine <= 0)
                {
                    break;
                }

                var temp = new byte[readLine];
                Array.Copy(buffer, 0, temp, 0, readLine);

                var encrypt = rsa.Encrypt(temp, false);
                output.Write(encrypt, 0, encrypt.Length);
            }
            encryptedData = output.ToArray();
        }
        else encryptedData = rsa.Encrypt(originalData, false);

        return Convert.ToBase64String(encryptedData);
    }

    /// <summary>
    /// 閫??
    /// </summary>
    /// <param name="text">撖??捆</param>
    /// <param name="privateKey">蝘</param>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public static string Decrypt(string text, string privateKey, int keySize = 2048)
    {
        CheckRSAKeySize(keySize);

        using var rsa = new RSACryptoServiceProvider(keySize);
        rsa.FromXmlString(privateKey);

        var encryptData = Convert.FromBase64String(text);
        byte[] decryptedData;

        // ?航圾撖???憭折摨?
        var bufferSize = rsa.KeySize / 8;

        // RSA 蝞?閫?
        // 敺?撖?摮??唬??質?餈??亦??踹漲?潮隞?8 ????11嚗嚗SACryptoServiceProvider.KeySize / 8 - 11嚗?
        // ??撖?敺撖????嚗迤憟賣撖?摨血潮隞?8嚗嚗SACryptoServiceProvider.KeySize / 8嚗?
        if (encryptData.Length > bufferSize)
        {
            // ?挾閫??
            var buffer = new byte[bufferSize];
            using var input = new MemoryStream(encryptData);
            using var output = new MemoryStream();

            while (true)
            {
                var readLine = input.Read(buffer, 0, bufferSize);
                if (readLine <= 0)
                {
                    break;
                }

                var temp = new byte[readLine];
                Array.Copy(buffer, 0, temp, 0, readLine);

                var decrypt = rsa.Decrypt(temp, false);
                output.Write(decrypt, 0, decrypt.Length);
            }
            decryptedData = output.ToArray();
        }
        else decryptedData = rsa.Decrypt(encryptData, false);

        return Encoding.Default.GetString(decryptedData);
    }

    /// <summary>
    /// 璉??RSA ?踹漲
    /// </summary>
    /// <param name="keySize"></param>
    private static void CheckRSAKeySize(int keySize)
    {
        if (keySize < 2048 || keySize > 16384 || keySize % 8 != 0)
            throw new ArgumentException("The keySize must be between 2048 and 16384 in size and must be divisible by 8.", nameof(keySize));
    }
}
