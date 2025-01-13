using System;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace SecureLibrary
{
    public class EncryptionHelper
    {
        // this section for Symmetric Encryption with AES GCM mode
        public static byte[] EncryptAesGcm(string plainText, byte[] key, byte[] nonce)
        {
            using (AesGcm aesGcm = new AesGcm(key))
            {
                byte[] encryptedData = new byte[plainText.Length];
                byte[] tag = new byte[32]; // 256-bit tag
                aesGcm.Encrypt(nonce, Encoding.UTF8.GetBytes(plainText), encryptedData, tag);
                return Combine(encryptedData, tag);
            }
        }

        public static string DecryptAesGcm(byte[] cipherText, byte[] key, byte[] nonce)
        {
            using (AesGcm aesGcm = new AesGcm(key))
            {
                byte[] tag = new byte[32];
                byte[] encryptedData = new byte[cipherText.Length - 32];
                Array.Copy(cipherText, encryptedData, encryptedData.Length);
                Array.Copy(cipherText, encryptedData.Length, tag, 0, tag.Length);

                byte[] decryptedData = new byte[encryptedData.Length];
                aesGcm.Decrypt(nonce, encryptedData, tag, decryptedData);

                return Encoding.UTF8.GetString(decryptedData);
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
        public static string BcryptEncoding(String password)
        {string hashedPassword = BCrypt.Net.BCrypt.HashPassword(
            password, 
            BCrypt.Net.BCrypt.GenerateSalt(12)
        );
    }
}
