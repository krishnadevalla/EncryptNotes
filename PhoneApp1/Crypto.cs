using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/**************************************************************************
 * Programmer : Leela Krishna Devalla
 * Date       : 11/18/2015
 * 
 * Class Name: Crypto
 * Fields    : None
 * 
 * Properties: None
 * 
 * Methods   : Encrypt, Decrypt, Hash, GenerateNewSalt, GetAlgorithm
 * 
 * 
 * This class implements cryptography methods like Encrypt, Decrypt, Hash, GenerateNewSalt, GetAlgorithm
 * This is a new enhancement to this app
 **************************************************************************/

namespace PhoneApp1
{
    //Crypto class
    public static class Crypto
    {
       
         // Function Encrypt has two parameters data of string type and password of type string
         // returns string
         // This function encrypts data using the password and symmetric algorithm
        public static string Encrypt(string data, string password)
        {
            using (SymmetricAlgorithm algo = GetAlgorithm(password)) // Using block including algorithm call
            using (MemoryStream ms = new MemoryStream())             // Using block creating memorystream
            using (CryptoStream cs = new CryptoStream(ms, algo.CreateEncryptor(), CryptoStreamMode.Write))  // Using block creating crytostream
            {
                byte[] plainbytes = Encoding.UTF8.GetBytes(data);  // Encoding data to UTF8 bytes 
                cs.Write(plainbytes, 0, plainbytes.Length);        // Writing the bytes to crytostream
                cs.FlushFinalBlock();                              // Cryptostream flusback
                return Convert.ToBase64String(ms.ToArray());       // returing the encrypted string of base 64
            }
        }

        // Function Decrypt has two parameters data of string type and password of type string
        // returns string
        // This function decrypts data using the password and symmetric algorithm
        public static string Decrypt(string data, string password)
        {
            using (SymmetricAlgorithm algo = GetAlgorithm(password)) // Using block including algorithm call
            using (MemoryStream ms = new MemoryStream())             // Using block creating memorystream
            using (CryptoStream cs = new CryptoStream(ms, algo.CreateDecryptor(), CryptoStreamMode.Write))  // Using block creating crytostream
            {
                byte[] cypherbytes = Convert.FromBase64String(data);                // Decoding data from base 64  to bytes 
                cs.Write(cypherbytes, 0, cypherbytes.Length);                       // Writing the bytes to crytostream
                cs.FlushFinalBlock();                                               // Cryptostream flusback
                cypherbytes = ms.ToArray();                                        
                return Encoding.UTF8.GetString(cypherbytes,0,cypherbytes.Length);   // returing the decrypted string of UTF8 format
            }
        }

        // Function Hash has one parameter password of type string
        // returns string
        // This function hashes password to base 64 string
        public static string Hash(string password)
        {
            byte[] databytes = Encoding.UTF8.GetBytes(password);             // Encoding data to UTF8 bytes 
            byte[] bytes = new byte[Settings.Salt.Length+databytes.Length];  // Creating byte array 
            Settings.Salt.CopyTo(bytes, 0);                                  // Coping salt value to bytes
            databytes.CopyTo(bytes, Settings.Salt.Length);                   // coping UTF8 bytes to bytes
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(bytes));  // returing hased value which is of base 64
        }

        // Function GenerateNewSalt has one parameter length of type int
        // returns byte array 
        // This function generates new salt value using the length given
        public static byte[] GenerateNewSalt(int length)
        {
            byte[] bytes = new byte[length];                 // creating byte array
            new RNGCryptoServiceProvider().GetBytes(bytes);  // instance of RNGCryptoServiceProvider to get bytes
            return bytes;
        }

        // Function GetAlgorithm has one parameter length of type int
        // returns SymmetricAlgorithm
        // This function generates new salt value using the length given
        static SymmetricAlgorithm GetAlgorithm(string password)
        {
            AesManaged algorithm = new AesManaged();                                   // new instance of AesManaged
            Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, Settings.Salt);// new instace of Rfc2898DeriveBytes
            algorithm.Key = bytes.GetBytes(algorithm.KeySize / 8);                     // setting key
            algorithm.IV = bytes.GetBytes(algorithm.BlockSize / 8);                    // setting IV
            return algorithm;
        }
    }
}
