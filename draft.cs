using System;
using System.Security.Cryptography;
using System.Text;

namespace SecureLibrary
{
    public class EncryptionHelper
    {
        public static byte[] EncryptAesGcm(string plainText, byte[] key, byte[] nonce)
        {
            using (AesGcm aesGcm = new AesGcm(key))
            {
                byte[] encryptedData = new byte[plainText.Length];
                byte[] tag = new byte[16]; // 128-bit tag
                aesGcm.Encrypt(nonce, Encoding.UTF8.GetBytes(plainText), encryptedData, tag);
                return Combine(encryptedData, tag);
            }
        }

        public static string DecryptAesGcm(byte[] cipherText, byte[] key, byte[] nonce)
        {
            using (AesGcm aesGcm = new AesGcm(key))
            {
                byte[] tag = new byte[16];
                byte[] encryptedData = new byte[cipherText.Length - 16];
                Array.Copy(cipherText, encryptedData, encryptedData.Length);
                Array.Copy(cipherText, encryptedData.Length, tag, 0, tag.Length);

                byte[] decryptedData = new byte[encryptedData.Length];
                aesGcm.Decrypt(nonce, encryptedData, tag, decryptedData);

                return Encoding.UTF8.GetString(decryptedData);
            }
        }

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

        private static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] combined = new byte[first.Length + second.Length];
            Array.Copy(first, combined, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
        }
    }
}
