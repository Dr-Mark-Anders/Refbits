﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Encryption
{
    public class EncryptionClass
    {
        //private static string PasswordHash;

        public class SimpleAES
        {
            // Change these keys
            private readonly byte[] Key = { 121, 117, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 122, 219, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

            private readonly byte[] Vector = { 146, 74, 191, 111, 23, 3, 113, 119, 131, 121, 252, 112, 79, 32, 114, 156 };
            private readonly ICryptoTransform EncryptorTransform;
            private readonly ICryptoTransform DecryptorTransform;
            private readonly System.Text.UTF8Encoding UTFEncoder;

            public SimpleAES()
            {
                //This is our encryption method
                using (RijndaelManaged rm = new())
                {
                    //Create an encryptor and a decryptor using our encryption method, key, and vector.
                    EncryptorTransform = rm.CreateEncryptor(this.Key, this.Vector);
                    DecryptorTransform = rm.CreateDecryptor(this.Key, this.Vector);
                }

                //Used to translate bytes to text and vice versa
                UTFEncoder = new System.Text.UTF8Encoding();
            }

            /// -------------- Two Utility Methods (not used but may be useful) -----------
            /// Generates an encryption key.
            public static byte[] GenerateEncryptionKey()
            {
                //Generate a Key.
                RijndaelManaged rm = new();
                rm.GenerateKey();
                return rm.Key;
            }

            /// Generates a unique encryption vector
            public static byte[] GenerateEncryptionVector()
            {
                //Generate a Vector
                RijndaelManaged rm = new();
                rm.GenerateIV();
                return rm.IV;
            }

            /// ----------- The commonly used methods ------------------------------
            /// Encrypt some text and return a string suitable for passing in a URL.
            public string EncryptToString(string TextValue)
            {
                return ByteArrToString(Encrypt(TextValue));
            }

            /// Encrypt some text and return an encrypted byte array.
            public byte[] Encrypt(string TextValue)
            {
                //Translates our text value into a byte array.
                Byte[] bytes = UTFEncoder.GetBytes(TextValue);

                //Used to stream the data in and out of the CryptoStream.
                MemoryStream memoryStream = new();

                /*
                 * We will have to write the unencrypted bytes to the stream,
                 * then read the encrypted result back from the stream.
                 */

                #region Write the decrypted value to the encryption stream

                CryptoStream cs = new(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();

                #endregion Write the decrypted value to the encryption stream

                #region Read encrypted value back out of the stream

                memoryStream.Position = 0;
                byte[] encrypted = new byte[memoryStream.Length];
                memoryStream.Read(encrypted, 0, encrypted.Length);

                #endregion Read encrypted value back out of the stream

                //Clean up.
                cs.Close();
                memoryStream.Close();

                return encrypted;
            }

            /// The other side: Decryption methods
            public string DecryptString(string EncryptedString)
            {
                return Decrypt(StrToByteArray(EncryptedString));
            }

            /// Decryption when working with byte arrays.
            public string Decrypt(byte[] EncryptedValue)
            {
                if (EncryptedValue is null)
                    return null;
                else
                {
                    #region Write the encrypted value to the decryption stream

                    MemoryStream encryptedStream = new();
                    CryptoStream decryptStream = new(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
                    decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
                    decryptStream.FlushFinalBlock();

                    #endregion Write the encrypted value to the decryption stream

                    #region Read the decrypted value from the stream.

                    encryptedStream.Position = 0;
                    Byte[] decryptedBytes = new Byte[encryptedStream.Length];
                    encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    encryptedStream.Close();

                    #endregion Read the decrypted value from the stream.

                    return UTFEncoder.GetString(decryptedBytes);
                }
            }

            /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
            //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            //      return encoding.GetBytes(str);
            // However, this results in character values that cannot be passed in a URL.  So, instead, I just
            // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
            public static byte[] StrToByteArray(string str)
            {
                if (str.Length == 0)
                    return null;

                byte[] byteArr = new byte[str.Length / 3];
                int i = 0;
                int j = 0;
                do
                {
                    if (byte.TryParse(str.AsSpan(i, 3), out byte val))
                        byteArr[j++] = val;
                    else
                        return null;

                    i += 3;
                }
                while (i < str.Length);
                return byteArr;
            }

            // Same comment as above.  Normally the conversion would use an ASCII encoding in the other direction:
            //      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //      return enc.GetString(byteArr);
            public static string ByteArrToString(byte[] byteArr)
            {
                byte val;
                string tempStr = "";
                for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
                {
                    val = byteArr[i];
                    if (val < (byte)10)
                        tempStr += "00" + val.ToString();
                    else if (val < (byte)100)
                        tempStr += "0" + val.ToString();
                    else
                        tempStr += val.ToString();
                }
                return tempStr;
            }
        }
    }
}