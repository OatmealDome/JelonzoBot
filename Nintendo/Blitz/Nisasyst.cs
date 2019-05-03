using System;
using System.IO;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Nintendo.Blitz
{
    public static class Nisasyst
    {
        private static string KeyMaterialString = "e413645fa69cafe34a76192843e48cbd691d1f9fba87e8a23d40e02ce13b0d534d10301576f31bc70b763a60cf07149cfca50e2a6b3955b98f26ca84a5844a8aeca7318f8d7dba406af4e45c4806fa4d7b736d51cceaaf0e96f657bb3a8af9b175d51b9bddc1ed475677260f33c41ddbc1ee30b46c4df1b24a25cf7cb6019794";
        public static byte[] MagicNumbers = Encoding.ASCII.GetBytes("nisasyst");

        public static byte[] Decrypt(byte[] data, string gamePath)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            using (MemoryStream outputStream = new MemoryStream())
            {
                Decrypt(inputStream, outputStream, gamePath);

                return outputStream.ToArray();
            }
        }
        
        public static void Decrypt(Stream inputStream, Stream outputStream, string gamePath)
        {   
            // Read the entire file into an array
            byte[] encryptedData = new byte[inputStream.Length - 8];
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.Read(encryptedData, 0, (int)inputStream.Length - 8);

            // Generate a CRC32 over the game path
            Crc32 crc32 = new Crc32();
            uint seed = crc32.Get(Encoding.ASCII.GetBytes(gamePath));

            // Create a new SeadRandom instance using the seed
            Nintendo.Sead.Random seadRandom = new Nintendo.Sead.Random(seed);

            // Create the encryption key and IV
            byte[] encryptionKey = CreateSequence(seadRandom);
            byte[] iv = CreateSequence(seadRandom);

            // Generate a KeyParameter instance
            KeyParameter parameter = new KeyParameter(encryptionKey);

            // Initialize the AES-CBC cipher
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/NoPadding");
            cipher.Init(false, new ParametersWithIV(parameter, iv));

            // Return the decrypted bytes
            byte[] output = cipher.DoFinal(encryptedData);

            // Seek to the beginning
            outputStream.Seek(0, SeekOrigin.Begin);

            // Write to the output stream
            outputStream.Write(output, 0, output.Length);

            // Seek back to the beginning
            outputStream.Seek(0, SeekOrigin.Begin);
        }

        public static bool IsNisasystFile(Stream stream)
        {
            // Check length
            if (stream.Length <= 8)
            {
                return false;
            }

            // Read the magic numbers
            byte[] lastBytes = new byte[8];
            stream.Seek(stream.Length - 8, SeekOrigin.Begin);
            stream.Read(lastBytes, 0, 8);
            
            // Seek back to the beginning
            stream.Seek(0, SeekOrigin.Begin);

            return lastBytes.SequenceEqual(MagicNumbers);
        }

        private static byte[] CreateSequence(Nintendo.Sead.Random random)
        {
            // Create byte array
            byte[] sequence = new byte[16];

            // Create each byte
            for (int i = 0; i < sequence.Length; i++)
            {
                // Create empty byte string
                string byteString = "";

                // Get characters from key material
                byteString += KeyMaterialString[(int)(random.GetUInt32() >> 24)];
                byteString += KeyMaterialString[(int)(random.GetUInt32() >> 24)];

                // Parse the resulting byte
                sequence[i] = Convert.ToByte(byteString, 16);
            }

            // Return the sequence
            return sequence;
        }

    }
}