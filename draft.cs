using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace SecureLibrary
{
    [ComVisible(true)]
    [Guid("9E506401-739E-402D-A11F-C77E7768362B")]
    [ClassInterface(ClassInterfaceType.None)]
    public class EncryptionHelper
    {
        //// this section for Symmetric Encryption with AES GCM mode
        /// AES-GCM not suported in .NET Framework 4.8.1 (will implement in .NET version DLL)
        //public static byte[] EncryptAesGcm(string plainText, byte[] key, byte[] nonce)
        //{
        //    using (AesGcm aesGcm = new AesGcm(key))
        //    {
        //        byte[] encryptedData = new byte[plainText.Length];
        //        byte[] tag = new byte[32]; // 256-bit tag
        //        aesGcm.Encrypt(nonce, Encoding.UTF8.GetBytes(plainText), encryptedData, tag);
        //        return Combine(encryptedData, tag);
        //    }
        //}

        //public static string DecryptAesGcm(byte[] cipherText, byte[] key, byte[] nonce)
        //{
        //    using (AesGcm aesGcm = new AesGcm(key))
        //    {
        //        byte[] tag = new byte[32];
        //        byte[] encryptedData = new byte[cipherText.Length - 32];
        //        Array.Copy(cipherText, encryptedData, encryptedData.Length);
        //        Array.Copy(cipherText, encryptedData.Length, tag, 0, tag.Length);

        //        byte[] decryptedData = new byte[encryptedData.Length];
        //        aesGcm.Decrypt(nonce, encryptedData, tag, decryptedData);

        //        return Encoding.UTF8.GetString(decryptedData);
        //    }
        //}
        public static (byte[] cipherText, byte[] iv) EncryptAesCbcWithIv(string plainText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // IV »ý¼º
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] cipherText = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return (cipherText, aes.IV);
                }
            }
        }

        public static string DecryptAesCbcWithIv(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        // this section related about diffie hellman
        public static (byte[] publicKey, byte[] privateKey) GenerateDiffieHellmanKeys()
        {
            using (ECDiffieHellmanCng dh = new ECDiffieHellmanCng())
            {
                dh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                dh.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] publicKey = dh.PublicKey.ToByteArray();
                byte[] privateKey = dh.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
                return (publicKey, privateKey);
            }
        }

        public static byte[] DeriveSharedKey(byte[] otherPartyPublicKey, byte[] privateKey)
        {
            using (ECDiffieHellmanCng dh = new ECDiffieHellmanCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob)))
            {
                using (CngKey otherKey = CngKey.Import(otherPartyPublicKey, CngKeyBlobFormat.EccPublicBlob))
                {
                    return dh.DeriveKeyMaterial(otherKey);
                }
            }
        }

        // this is common byte combine method for AES and DH implementation
        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Array.Copy(first, combined, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
        }

        // this section related about bcrypt
        public static string BcryptEncoding(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

    }
}
