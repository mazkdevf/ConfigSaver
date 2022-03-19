using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConfigSaver
{
	public static class CF
    {
		private static readonly string configpath = ".\\ConfigSaver.json";

		private static string decryptedtext = null; 

		public static string js(string userorpass)
        {
			ReadConfig(); // Read Config to make sure its actually existing

			string value = null;

			if (decryptedtext == "Cfnotexist")
            {
				return "Config File Not Found....";
            }

			var userdetails = JsonConvert.DeserializeObject<configdata>(decryptedtext);


			if (userorpass == "user") 
			{
				if (decryptedtext == "Cfnotexist") 
				{
					value = "Config File Not Found....";
				} else {
					value = userdetails.username;
				}
			} else if (userorpass == "pass") {
				if (decryptedtext == "Cfnotexist")
				{
					value = "Config File Not Found....";
				}
				else
				{
					value = userdetails.password;
				}
			} else {
				value = "Method Not Found..";
            }

			return value;
        }

		public static string readConfigPath(string key)
        {
			string returnvalue = "";

			if (key == "eUe8vgM3ivJDe")
            {
				returnvalue = configpath;
            } else {
				returnvalue = "Key is wrong..";
            }
			return returnvalue;
        }

		public static void ReadConfig()
		{
			if (File.Exists(configpath))
			{
				string content = File.ReadAllText(configpath);
				decryptedtext = enc.Decrypt(content);
			}
			else
			{
				decryptedtext = "Cfnotexist";
			}

		}

		public static void CreateConfig(string username, string password)
		{
			configdata configdata = new configdata
			{
				username = username.ToString(),
				password = password.ToString()
			};
			JObject jobject = (JObject)JToken.FromObject(configdata);

			string encryptedver = enc.Encrypt(jobject.ToString());

			if (File.Exists(configpath)) { File.Delete(configpath); }

			File.WriteAllText(configpath, encryptedver);
			File.SetAttributes(configpath, FileAttributes.Hidden);
			ReadConfig(); // Read Config to make sure its actually existing
		}
	}

	public static class enc
	{
		// Credits to Eramake ECrypto


		// Great Password Generator - https://passwordsgenerator.net/
		// Create #8 Digit Password
		private static readonly string PasswordHash = "ZFDUx9aY";

		// Create #8 Digit Password
		private static readonly string SaltKey = "E27fwNbX";

		// Create #16 Digit Password
		private static readonly string VIKey = "B3jt8s6bntaEb4wn";

		public static string Encrypt(string plainText)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			byte[] bytes2 = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.Zeros
			};
			ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes2, Encoding.ASCII.GetBytes(VIKey));
			byte[] inArray;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					inArray = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(inArray);
		}

		public static string Decrypt(string encryptedText)
		{
			byte[] array = Convert.FromBase64String(encryptedText);
			byte[] bytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(32);
			RijndaelManaged rijndaelManaged = new RijndaelManaged
			{
				Mode = CipherMode.CBC,
				Padding = PaddingMode.None
			};
			ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(VIKey));
			MemoryStream memoryStream = new MemoryStream(array);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array2 = new byte[array.Length];
			int count = cryptoStream.Read(array2, 0, array2.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array2, 0, count).TrimEnd("\0".ToCharArray());
		}
	}

	public class configdata
	{
		public string username { get; set; }
		public string password { get; set; }
	}
}
