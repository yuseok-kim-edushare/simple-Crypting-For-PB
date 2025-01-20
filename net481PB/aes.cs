using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

[ComVisible(true)]
[Guid("9E506401-739E-402D-A11F-C77E7768362B")]
[ClassInterface(ClassInterfaceType.None)]
public class AesGcmCngExample
{
    private const string BCRYPT_AES_ALGORITHM = "AES";
    private const string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

    [DllImport("bcrypt.dll")]
    private static extern int BCryptOpenAlgorithmProvider(out IntPtr hAlgorithm, string pszAlgId, string pszImplementation, int dwFlags);

    [DllImport("bcrypt.dll")]
    private static extern int BCryptSetProperty(IntPtr hObject, string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

    [DllImport("bcrypt.dll")]
    private static extern int BCryptGenerateSymmetricKey(IntPtr hAlgorithm, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbSecret, int cbSecret, int dwFlags);

    [DllImport("bcrypt.dll")]
    private static extern int BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, out int pcbResult, int dwFlags);

    [DllImport("bcrypt.dll")]
    private static extern int BCryptDestroyKey(IntPtr hKey);

    [DllImport("bcrypt.dll")]
    private static extern int BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, int dwFlags);

    [StructLayout(LayoutKind.Sequential)]
    public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
    {
        public int cbSize;
        public int dwInfoVersion;
        public IntPtr pbNonce;
        public int cbNonce;
        public IntPtr pbAuthData;
        public int cbAuthData;
        public IntPtr pbTag;
        public int cbTag;
        public IntPtr pbMacContext;
        public int cbMacContext;
        public int cbAAD;
        public long cbData;
        public int dwFlags;

        public static BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO Create()
        {
            return new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
            {
                cbSize = Marshal.SizeOf(typeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO)),
                dwInfoVersion = 1
            };
        }
    }

    public static void EncryptData(byte[] plaintext, byte[] key, byte[] nonce, out byte[] ciphertext, out byte[] tag)
    {
        IntPtr hAlgorithm;
        IntPtr hKey;

        BCryptOpenAlgorithmProvider(out hAlgorithm, BCRYPT_AES_ALGORITHM, null, 0);
        BCryptSetProperty(hAlgorithm, BCRYPT_CHAIN_MODE_GCM, System.Text.Encoding.Unicode.GetBytes(BCRYPT_CHAIN_MODE_GCM), BCRYPT_CHAIN_MODE_GCM.Length * 2, 0);
        BCryptGenerateSymmetricKey(hAlgorithm, out hKey, IntPtr.Zero, 0, key, key.Length, 0);

        BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO authInfo = BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO.Create();
        authInfo.pbNonce = Marshal.AllocHGlobal(nonce.Length);
        Marshal.Copy(nonce, 0, authInfo.pbNonce, nonce.Length);
        authInfo.cbNonce = nonce.Length;

        ciphertext = new byte[plaintext.Length];
        tag = new byte[16]; // GCM tag size

        BCryptEncrypt(hKey, plaintext, plaintext.Length, ref authInfo, nonce, nonce.Length, ciphertext, ciphertext.Length, out int cbResult, 0);
        Marshal.Copy(authInfo.pbTag, tag, 0, tag.Length);

        BCryptDestroyKey(hKey);
        BCryptCloseAlgorithmProvider(hAlgorithm, 0);
        Marshal.FreeHGlobal(authInfo.pbNonce);
    }
}
