//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using System.Security.Cryptography;
//using System.Windows.Forms;
////using ControlzEx.Standard;
////using Chilkat;

//namespace ClientSide
//{
//    public class Crypto
//    {

//        public static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

//        //how to get the private key
//        //  private  static System.Security.Cryptography.RSAParameters privKey = csp.ExportParameters(true);


//        //and the public key ...


//        public static string getPubKey()
//        {
//            System.Security.Cryptography.RSAParameters pubKey = csp.ExportParameters(false);

//            //we need some buffer
//            var sw = new System.IO.StringWriter();
//            //we need a serializer
//            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
//            //serialize the key into the stream
//            xs.Serialize(sw, pubKey);
//            //get the string from the stream
//            string pubKeyString = sw.ToString();
//            return pubKeyString;
//        }
//        public static string Encryption(string strText)
//        {
//            var publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
//            // string publicKey = getPubKey();
//            var testData = Encoding.UTF8.GetBytes(strText);

//            using (var rsa = new RSACryptoServiceProvider(1024))
//            {
//                try
//                {
//                    // client encrypting data with public key issued by server                    
//                    rsa.FromXmlString(publicKey.ToString());

//                    var encryptedData = rsa.Encrypt(testData, true);
//                    var base64Encrypted = Convert.ToBase64String(encryptedData);
//                    return base64Encrypted;
//                }
//                catch ( CryptographicException ex) {
//                    ShowErrorDialog("CryptographicException:\n" + ex);
//                    Console.WriteLine("CryptographicException:\n" + ex);
//                    //return EncryptionByParts(strText );
//                    return string.Empty;
//                }

//                finally
//                {
//                    rsa.PersistKeyInCsp = false;
//                }
//            }
//        }
//        private static void ShowErrorDialog(string message)
//        {
//            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }
//        private static string EncryptionByParts(string strText )
//        {
//            int maxSize = 200;
//            string subData = "";
//            string sumEncrypt = "";

//            if (strText.Length > maxSize)
//            {

//                for (int i = 0; i < strText.Length; i += maxSize)
//                {
//                    if ((i + maxSize) > strText.Length)
//                    {
//                        subData += strText.Substring(i, strText.Length);                      
//                    }                           
//                    else
//                    {
//                        subData += strText.Substring(i, i + maxSize);
//                    }
//                    sumEncrypt += Encryption(subData);
//                }


//            }
//            return sumEncrypt;
//        }

//        public static string Decryption(string strText)
//        {
//            var privateKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent><P>/aULPE6jd5IkwtWXmReyMUhmI/nfwfkQSyl7tsg2PKdpcxk4mpPZUdEQhHQLvE84w2DhTyYkPHCtq/mMKE3MHw==</P><Q>3WV46X9Arg2l9cxb67KVlNVXyCqc/w+LWt/tbhLJvV2xCF/0rWKPsBJ9MC6cquaqNPxWWEav8RAVbmmGrJt51Q==</Q><DP>8TuZFgBMpBoQcGUoS2goB4st6aVq1FcG0hVgHhUI0GMAfYFNPmbDV3cY2IBt8Oj/uYJYhyhlaj5YTqmGTYbATQ==</DP><DQ>FIoVbZQgrAUYIHWVEYi/187zFd7eMct/Yi7kGBImJStMATrluDAspGkStCWe4zwDDmdam1XzfKnBUzz3AYxrAQ==</DQ><InverseQ>QPU3Tmt8nznSgYZ+5jUo9E0SfjiTu435ihANiHqqjasaUNvOHKumqzuBZ8NRtkUhS6dsOEb8A2ODvy7KswUxyA==</InverseQ><D>cgoRoAUpSVfHMdYXW9nA3dfX75dIamZnwPtFHq80ttagbIe4ToYYCcyUz5NElhiNQSESgS5uCgNWqWXt5PnPu4XmCXx6utco1UVH8HGLahzbAnSy6Cj3iUIQ7Gj+9gQ7PkC434HTtHazmxVgIR5l56ZjoQ8yGNCPZnsdYEmhJWk=</D></RSAKeyValue>";
//            //string privateKey = getPriKey();
//            var testData = Encoding.UTF8.GetBytes(strText);

//            using (var rsa = new RSACryptoServiceProvider(1024))
//            {
//                try
//                {
//                    var base64Encrypted = strText;

//                    // server decrypting data with private key                    
//                    rsa.FromXmlString(privateKey);

//                    var resultBytes = Convert.FromBase64String(base64Encrypted);
//                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
//                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
//                    return decryptedData.ToString();
//                }
//                finally
//                {
//                    rsa.PersistKeyInCsp = false;
//                }
//            }
//        }

//        private static string getPriKey()
//        {
//            System.Security.Cryptography.RSAParameters privKey = csp.ExportParameters(true);
//            //we need some buffer
//            var sw = new System.IO.StringWriter();
//            //we need a serializer
//            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
//            //serialize the key into the stream
//            xs.Serialize(sw, privKey);
//            //get the string from the stream
//            string privKeyString = sw.ToString();
//            return privKeyString;
//        }
//    }
//}


using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClientSide
{
    class Crypto
    {
         
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }

}