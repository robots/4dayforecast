// Decompiled with JetBrains decompiler
// Type: Forecast.Crypto
// Assembly: 4-Day Forecast, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C6EF029-44CF-4261-A2A3-D6C268534CDA
// Assembly location: C:\Program Files (x86)\4-Day Forecast\4-Day Forecast\4-Day Forecast.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Forecast
{
  public class Crypto
  {
    public static string encrypt3des(string input, string key_seed)
    {
      string s = Crypto.MD5SUM(Encoding.ASCII.GetBytes(key_seed)).Substring(0, 24);
      input = input.Trim();
      ASCIIEncoding asciiEncoding = new ASCIIEncoding();
      TripleDES tripleDes = TripleDES.Create();
      tripleDes.Padding = PaddingMode.Zeros;
      tripleDes.Mode = CipherMode.ECB;
      tripleDes.Key = asciiEncoding.GetBytes(s);
      tripleDes.GenerateIV();
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, tripleDes.CreateEncryptor(), CryptoStreamMode.Write);
      cryptoStream.Write(asciiEncoding.GetBytes(input), 0, asciiEncoding.GetByteCount(input));
      cryptoStream.FlushFinalBlock();
      byte[] inArray = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return Convert.ToBase64String(inArray, 0, inArray.GetLength(0)).Trim();
    }

    public static string decrypt3des(string input, string key_seed)
    {
      try
      {
        string s = Crypto.MD5SUM(Encoding.ASCII.GetBytes(key_seed)).Substring(0, 24);
        ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        TripleDES tripleDes = TripleDES.Create();
        tripleDes.Padding = PaddingMode.Zeros;
        tripleDes.Mode = CipherMode.ECB;
        tripleDes.Key = asciiEncoding.GetBytes(s);
        byte[] buffer = Convert.FromBase64String(input);
        return new StreamReader((Stream) new CryptoStream((Stream) new MemoryStream(buffer, 0, buffer.Length), tripleDes.CreateDecryptor(), CryptoStreamMode.Read)).ReadToEnd();
      }
      catch
      {
        return "";
      }
    }

    private static string MD5SUM(byte[] FileOrText)
    {
      return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(FileOrText)).Replace("-", "").ToLower();
    }
  }
}
