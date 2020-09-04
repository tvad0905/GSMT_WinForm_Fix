namespace FileExportScheduler
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    internal class FileReadWrite
    {
        private static byte[] iv = new byte[8];
        public static byte[] key = new byte[] { 
            0x15, 10, 0x40, 10, 100, 40, 200, 4, 0x15, 0x36, 0x41, 0xf6, 5, 0x3e, 1, 0x36,
            0x36, 6, 8, 9, 0x41, 4, 0x41, 9
        };

        public static string ReadFile(string FilePath)
        {
            FileInfo info = new FileInfo(FilePath);
            if (!info.Exists)
            {
                return string.Empty;
            }
            FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            TripleDES edes = new TripleDESCryptoServiceProvider();
            CryptoStream stream2 = new CryptoStream(stream, edes.CreateDecryptor(key, iv), CryptoStreamMode.Read);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < stream.Length; i++)
            {
                int num = stream2.ReadByte();
                if (num == 0)
                {
                    break;
                }
                builder.Append(Convert.ToChar(num));
            }
            try
            {
                stream2.Close();
            }
            catch (Exception)
            {
            }
            stream.Close();
            return builder.ToString();
        }

        public static void WriteFile(string FilePath, string Data)
        {
            FileStream stream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write);
            TripleDES edes = new TripleDESCryptoServiceProvider();
            CryptoStream stream2 = new CryptoStream(stream, edes.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] bytes = Encoding.ASCII.GetBytes(Data);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.WriteByte(0);
            stream2.Close();
            stream.Close();
        }
    }
}

