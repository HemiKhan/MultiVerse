using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace Data.DataAccess
{
    public class Crypto
    {
        public Crypto() { }
        public static string FormatFileSize(long bytes)
        {
            string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB" };
            int counter = 0;
            decimal number = bytes;

            while (Math.Round(number / 1024) >= 1 && counter < suffixes.Length - 1)
            {
                number /= 1024;
                counter++;
            }

            return string.Format("{0:0.##} {1}", number, suffixes[counter]);
        }
        public static string NewEncodePassword(byte passFormat, string passtext, string passwordSalt)
        {
            if (passFormat == 0)
            {
                // No encoding required
                return passtext;
            }
            else
            {
                // Convert password and salt to bytes
                byte[] bytePASS = Encoding.Unicode.GetBytes(passtext);
                byte[] byteSALT = Convert.FromBase64String(passwordSalt);

                // Concatenate salt and password bytes
                byte[] combinedBytes = new byte[bytePASS.Length + byteSALT.Length];
                Buffer.BlockCopy(bytePASS, 0, combinedBytes, 0, bytePASS.Length);
                Buffer.BlockCopy(byteSALT, 0, combinedBytes, bytePASS.Length, byteSALT.Length);

                // Use AES encryption
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;

                    // Generate key and IV from password
                    Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(passtext, byteSALT, 10000);
                    aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                    aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

                    // Encrypt the combined bytes
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(combinedBytes, 0, combinedBytes.Length);
                            cs.FlushFinalBlock();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        public static string NewDecodePassword(byte passFormat, string encodedPassword, string passwordSalt)
        {
            if (passFormat == 0)
            {
                // No decoding required
                return encodedPassword;
            }
            else
            {
                // Convert encoded password to bytes
                byte[] encryptedBytes = Convert.FromBase64String(encodedPassword);

                // Convert salt to bytes
                byte[] byteSALT = Convert.FromBase64String(passwordSalt);

                // Use AES decryption
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;

                    // Generate key and IV from password
                    Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(encodedPassword, byteSALT, 10000);
                    aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                    aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);

                    // Decrypt the encrypted bytes
                    using (MemoryStream ms = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] decryptedBytes = new byte[encryptedBytes.Length];
                            int decryptedByteCount = cs.Read(decryptedBytes, 0, decryptedBytes.Length);
                            return Encoding.Unicode.GetString(decryptedBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }
        public static string EncodePassword(byte passFormat, string passtext, string passwordSalt)
        {
            if ((passFormat.Equals(0)))
                return passtext;
            else
            {
                byte[] Key;
                byte[] IV;
                byte[] bytePASS = Encoding.Unicode.GetBytes(passtext);
                byte[] byteSALT = Convert.FromBase64String(passwordSalt);
                var Lenarr = (byteSALT.Length + bytePASS.Length) - 1;
                byte[] byteRESULT = new byte[Lenarr + 1];

                System.Buffer.BlockCopy(byteSALT, 0, byteRESULT, 0, byteSALT.Length);
                System.Buffer.BlockCopy(bytePASS, 0, byteRESULT, byteSALT.Length, bytePASS.Length);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.KeySize = 128;
                Key = UTF8Encoding.UTF8.GetBytes("0123456789ABCDEF");
                IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
                tdes.Key = Key;
                tdes.IV = IV;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(byteRESULT, 0, byteRESULT.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

        }
        public static string DecodePassword(string Password, string PW_Salt)
        {
            byte[] PasswordDecode;
            byte[] Salt;
            string RetVal;
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.KeySize = 128;
            tdes.Key = UTF8Encoding.UTF8.GetBytes("0123456789ABCDEF");
            tdes.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
            byte[] toEncryptArray = Convert.FromBase64String(Password);
            ICryptoTransform cTransform1 = tdes.CreateDecryptor();
            PasswordDecode = cTransform1.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            Salt = Convert.FromBase64String(PW_Salt);
            RetVal = Encoding.Unicode.GetString(PasswordDecode).Remove(0, Encoding.Unicode.GetString(Salt).Length);
            return RetVal;
        }
        public static byte[] GenerateSalt(byte[] saltSize)
        {
            byte[] saltBytes = saltSize;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }
        #region DES Encrypt/Decrypt
        public static string EncryptWithExpiry(string plainText, string encryptionKey, TimeSpan validityPeriod)
        {
            // Ensure encryptionKey is at least 16 characters long
            if (encryptionKey.Length < 16)
                throw new ArgumentException("Encryption key must be at least 16 characters long.");

            // Convert plaintext to bytes
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // Add current timestamp to plain bytes
            byte[] timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            byte[] combinedBytes = new byte[plainBytes.Length + timestampBytes.Length];
            Array.Copy(plainBytes, combinedBytes, plainBytes.Length);
            Array.Copy(timestampBytes, 0, combinedBytes, plainBytes.Length, timestampBytes.Length);

            // Encrypt combined bytes
            byte[] encryptedBytes = EncryptBytes(combinedBytes, encryptionKey);

            // Convert result to hex string
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
        public static string DecryptWithExpiry(string hexString, string encryptionKey, TimeSpan validityPeriod)
        {
            // Convert hex string to bytes
            byte[] encryptedBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                encryptedBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            // Decrypt bytes
            byte[] decryptedBytes = DecryptBytes(encryptedBytes, encryptionKey);

            // Extract plaintext and timestamp
            byte[] plainBytes = new byte[decryptedBytes.Length - sizeof(long)];
            byte[] timestampBytes = new byte[sizeof(long)];
            Array.Copy(decryptedBytes, plainBytes, plainBytes.Length);
            Array.Copy(decryptedBytes, plainBytes.Length, timestampBytes, 0, timestampBytes.Length);

            // Check timestamp validity
            long ticks = BitConverter.ToInt64(timestampBytes, 0);
            DateTime timestamp = new DateTime(ticks, DateTimeKind.Utc);
            if (DateTime.UtcNow - timestamp > validityPeriod)
                throw new InvalidOperationException("Data has expired.");

            // Convert plaintext bytes to string
            return Encoding.UTF8.GetString(plainBytes);
        }
        private static byte[] EncryptBytes(byte[] inputBytes, string encryptionKey)
        {
            // Convert encryptionKey to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 16));

            // Encrypt bytes using AES
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(inputBytes, 0, inputBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }

                    byte[] ivAndEncryptedBytes = new byte[aesAlg.IV.Length + msEncrypt.Length];
                    Array.Copy(aesAlg.IV, ivAndEncryptedBytes, aesAlg.IV.Length);
                    Array.Copy(msEncrypt.ToArray(), 0, ivAndEncryptedBytes, aesAlg.IV.Length, msEncrypt.Length);
                    return ivAndEncryptedBytes;
                }
            }
        }
        private static byte[] DecryptBytes(byte[] encryptedBytes, string encryptionKey)
        {
            // Convert encryptionKey to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 16));

            // Extract IV and encrypted data
            byte[] iv = new byte[16];
            byte[] encryptedData = new byte[encryptedBytes.Length - 16];
            Array.Copy(encryptedBytes, iv, 16);
            Array.Copy(encryptedBytes, 16, encryptedData, 0, encryptedData.Length);

            // Decrypt bytes using AES
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = iv;

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (MemoryStream msPlain = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            return msPlain.ToArray();
                        }
                    }
                }
            }
        }
        public static string EncryptToHex(string plainText, string encryptionKey, bool isfixedIV = false, TimeSpan? validityPeriod = null)
        {
            if (plainText == "")
                return plainText;

            // Ensure encryptionKey is at least 16 characters long
            if (encryptionKey.Length < 16)
                throw new ArgumentException("Encryption key must be at least 16 characters long.");

            // Convert stringToEncrypt and encryptionKey to bytes
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 16));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                if (isfixedIV)
                    aesAlg.Mode = CipherMode.ECB; // Use ECB mode
                else
                    aesAlg.GenerateIV();

                // Encrypt the data
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }

                    if (isfixedIV == false)
                    {
                        // Combine IV and encrypted data
                        byte[] encryptedBytes = msEncrypt.ToArray();
                        byte[] resultBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
                        Array.Copy(aesAlg.IV, resultBytes, aesAlg.IV.Length);
                        Array.Copy(encryptedBytes, 0, resultBytes, aesAlg.IV.Length, encryptedBytes.Length);
                        return BitConverter.ToString(resultBytes).Replace("-", "");
                    }
                    else
                    {
                        // Convert result to hex string
                        return BitConverter.ToString(msEncrypt.ToArray()).Replace("-", "");
                    }
                }
            }
        }
        public static string DecryptFromHex(string hexString, string encryptionKey, bool isfixedIV = false)
        {
            if (hexString == "")
                return hexString;

            // Ensure encryptionKey is at least 16 characters long
            if (encryptionKey.Length < 16)
                throw new ArgumentException("Encryption key must be at least 16 characters long.");

            // Convert hex string to bytes
            byte[] inputBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                inputBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            byte[] iv = new byte[16];
            byte[] encryptedBytes = new byte[inputBytes.Length - iv.Length];
            if (isfixedIV == false)
            {
                // Extract IV and encrypted data
                Array.Copy(inputBytes, iv, iv.Length);
                Array.Copy(inputBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);
            }
            // Convert encryptionKey to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 16));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                if (isfixedIV)
                    aesAlg.Mode = CipherMode.ECB; // Use ECB mode
                else
                    aesAlg.IV = iv;

                // Decrypt the data
                using (MemoryStream msDecrypt = new MemoryStream((isfixedIV ? inputBytes : encryptedBytes)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion
        public static string EncryptString(String qs, bool isfixedIV = false)
        {
            qs = (qs ?? "");
            if (qs == "")
                return qs;
            return Crypto.EncryptToHex(qs, "MetroCryptoUSA08861-POMS#?+", isfixedIV);
        }
        public static string DecryptString(String qs, bool isfixedIV = false)
        {
            qs = (qs ?? "");
            if (qs == "")
                return qs;
            return Crypto.DecryptFromHex(qs, "MetroCryptoUSA08861-POMS#?+", isfixedIV);
        }
        public static string? EncryptNumericToString(int? qs, bool isfixedIV = false)
        {
            if (qs == null)
                return null;
            return Crypto.EncryptToHex(qs.ToString(), "MetroCryptoUSA08861-POMS#?+", isfixedIV);
        }
        public static int? DecryptNumericToString(string? qs, bool IsNullForEmptyString = false, bool isfixedIV = false)
        {
            if (qs == null)
                return null;
            else if (qs == "" && IsNullForEmptyString)
                return null;
            else if (qs == "" && IsNullForEmptyString == false)
                return 0;
            return Convert.ToInt32(Crypto.DecryptFromHex(qs, "MetroCryptoUSA08861-POMS#?+", isfixedIV));
        }
        public static string EncryptNumericToStringWithOutNull(int? qs, bool isfixedIV = false)
        {
            if (qs == null)
                return "";
            return Crypto.EncryptToHex(qs.ToString(), "MetroCryptoUSA08861-POMS#?+", isfixedIV);
        }
        public static int DecryptNumericToStringWithOutNull(string? qs, bool isfixedIV = false)
        {
            if (qs == null)
                return 0;
            return (int)DecryptNumericToString(qs, false, isfixedIV);
        }
        public static string EncryptOrderSource(string qs)
        {
            if (qs == null || qs.Length == 0)
                return "0";
            else
                return Crypto.EncryptToHex(qs.ToString(), "MetroCryptoUSA08861-POMS#?+");
        }
        public static string DecryptOrderSource(String qs)
        {
            string Retqs = "0";
            if (qs == null || qs.Length == 0)
                return Retqs;
            else
            {
                Retqs = Crypto.DecryptFromHex(qs, "MetroCryptoUSA08861-POMS#?+");
                if (Information.IsNumeric(Retqs) == false)
                    Retqs = "0";
            }
            return Retqs;
        }
        public static string EncryptUserName(String qs)
        {
            if (qs == null || qs.Length == 0)
                return qs;
            else
                return Crypto.EncryptToHex(qs.ToUpper(), "MetroCryptoUSA08861-POMS#?+");
        }
        public static string DecryptUserName(String qs)
        {
            if (qs == null || qs.Length == 0)
                return qs;
            else
                return Crypto.DecryptFromHex(qs, "MetroCryptoUSA08861-POMS#?+").ToUpper();
        }
        public static string EncryptPasswordHashSalt(String ph, String ps)
        {
            return Crypto.EncryptToHex($"{ph}{ps}", "MetroCryptoUSA08861-POMS#?+", true);
        }
        public static string DecryptPasswordHashSalt(String qs)
        {
            return Crypto.DecryptFromHex(qs, "MetroCryptoUSA08861-POMS#?+", true);
        }
        public static string EncryptPassword(String password)
        {
            return Crypto.EncryptToHex($"{password}", "MetroCryptoUSA08861-POMS#?+", true);
        }
        public static string DecryptPassword(String password)
        {
            return Crypto.DecryptFromHex(password, "MetroCryptoUSA08861-POMS#?+", true);
        }
        public static NameValueCollection ConvertNVC(string qs)
        {
            NameValueCollection nvc = new NameValueCollection();
            string[] nameValuePairs = qs.Split('&');

            for (int i = 0; i < nameValuePairs.Length; i++)
            {
                string[] nameValue = nameValuePairs[i].Split('=');

                if (nameValue.Length == 2)
                    nvc.Add(nameValue[0], nameValue[1]);
            }

            return nvc;
        }
    }
}
