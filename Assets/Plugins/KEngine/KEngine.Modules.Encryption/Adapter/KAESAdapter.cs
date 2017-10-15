﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace KEngine
{


/// <summary>
/// Aes加解密
/// </summary>
public sealed class AesEncrypter
{
    /// <summary>
    /// RijndaelManaged
    /// </summary>
    private readonly RijndaelManaged rijndaelManaged;

    /// <summary>
    /// Aes加解密
    /// </summary>
    /// <param name="rijndaelManaged">RijndaelManaged</param>
    public AesEncrypter(RijndaelManaged rijndaelManaged)
    {
        this.rijndaelManaged = rijndaelManaged;
    }

    /// <summary>
    /// Aes加解密
    /// </summary>
    /// <param name="size">密钥长度</param>
    /// <param name="mode">密码模式</param>
    /// <param name="padding">填充模式</param>
    public AesEncrypter(int size = 128, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
    {
        rijndaelManaged = new RijndaelManaged
        {
            KeySize = size,
            BlockSize = size,
            Padding = padding,
            Mode = mode,
        };
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="content">加密内容</param>
    /// <param name="key">密钥</param>
    /// <returns></returns>
    public string Encrypt(byte[] content, byte[] key)
    {
        rijndaelManaged.GenerateIV();

        var aesEncrypt = rijndaelManaged.CreateEncryptor(key, rijndaelManaged.IV);
        var aesBuffer = aesEncrypt.TransformFinalBlock(content, 0, content.Length);
        var ivAesBuffer = KArray.Merge(rijndaelManaged.IV, aesBuffer);

        return Encode(Convert.ToBase64String(rijndaelManaged.IV),
            Convert.ToBase64String(aesBuffer),
            Convert.ToBase64String(HMac(ivAesBuffer, key)));
    }

    /// <summary>
    /// 解密被加密的内容
    /// </summary>
    /// <param name="str">需要解密的字符串</param>
    /// <param name="key">密钥</param>
    /// <returns>解密后的值</returns>
    public byte[] Decrypt(string str, byte[] key)
    {
        string iv, value, hmac;
        Decode(str, out iv, out value, out hmac);

        var aesBuffer = Convert.FromBase64String(value);
        var ivBuffer = Convert.FromBase64String(iv);
        var ivAesBuffer = KArray.Merge(rijndaelManaged.IV, aesBuffer);

        if (Convert.ToBase64String(HMac(ivAesBuffer, key)) != hmac)
        {
            throw new Exception("HMac validation failed");
        }

        var aesDecrypt = rijndaelManaged.CreateDecryptor(key, ivBuffer);
        aesBuffer = aesDecrypt.TransformFinalBlock(aesBuffer, 0, aesBuffer.Length);

        return aesBuffer;
    }

    /// <summary>
    /// 计算HMac
    /// </summary>
    /// <param name="content">需要计算HMac的数据</param>
    /// <param name="key">密钥</param>
    /// <returns>HMac值</returns>
    private byte[] HMac(byte[] content, byte[] key)
    {
        var hmacSha256 = new HMACSHA256(key);
        return hmacSha256.ComputeHash(content);
    }

    /// <summary>
    /// 加码
    /// </summary>
    /// <param name="iv">iv</param>
    /// <param name="value">加密值</param>
    /// <param name="hmac">验证码</param>
    private string Encode(string iv, string value, string hmac)
    {
        var templete = "{\"iv\":\"" + iv + "\",\"value\":\"" + value + "\",\"hmac\":\"" + hmac + "\"}";
        return templete;
    }

    /// <summary>
    /// 解码
    /// </summary>
    /// <param name="str">传入数据</param>
    /// <param name="iv">iv</param>
    /// <param name="value">加密至</param>
    /// <param name="hmac"></param>
    private void Decode(string str, out string iv, out string value, out string hmac)
    {
        str = str.Replace("\"", string.Empty);
        str = str.Replace("{", string.Empty);
        str = str.Replace("}", string.Empty);

        var arr = str.Split(',');

        if (arr.Length < 3)
        {
            throw new Exception("Invalid encrypted data");
        }

        var result = new Dictionary<string, string>();

        foreach (var segment in arr)
        {
            var fragment = segment.Split(':');
            if (fragment.Length != 2)
            {
                throw new Exception("Invalid encrypted data");
            }

            result[fragment[0]] = fragment[1];
        }

        if (!result.TryGetValue("iv", out iv) ||
            !result.TryGetValue("value", out value) ||
            !result.TryGetValue("hmac", out hmac))
        {
            throw new Exception("Invalid encrypted data");
        }
    }
}
}