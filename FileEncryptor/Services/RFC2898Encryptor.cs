using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FileEncryptor.Services.Interfaces;

namespace FileEncryptor.Services
{

    internal class Rfc2898Encryptor : IEncryptor
    {
        private static readonly byte[] __Salt =
        {
            0x26, 0xdc, 0xff, 0x00,
            0xad, 0xed, 0x7a, 0xee,
            0xc5, 0xfe, 0x07, 0xaf,
            0x4d, 0x08, 0x22, 0x3c
        };

        private static byte[] CreateRandomSalt(int length = 16) => RandomNumberGenerator.GetBytes(Math.Max(1, length));


        private static ICryptoTransform GetEncryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateEncryptor();
        }

        private static ICryptoTransform GetDecryptor(string password, byte[] Slat = null)
        {
            var pdb = new Rfc2898DeriveBytes(password, Slat ?? __Salt);
            var algorithm = Aes.Create();
            algorithm.Key = pdb.GetBytes(32);
            algorithm.IV = pdb.GetBytes(16);
            return algorithm.CreateDecryptor();
        }

        public void Encrypt(string SourcePath, string DestinationPath, string Password, int BufferLength = 104200)
        {
            var encryptor = GetEncryptor(Password/*, Encoding.UTF8.GetBytes(SourcePath)*/);

            using var destination_encrypted = File.Create(DestinationPath, BufferLength);
            using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write); //поток для шифровки
            using var source = File.OpenRead(SourcePath);

            var buffer = new byte[BufferLength];
            int readed;
            do
            {
                Thread.Sleep(1);
                readed = source.Read(buffer, 0, BufferLength);
                destination.Write(buffer, 0, readed);
            }
            while (readed > 0);
            destination.FlushFinalBlock();
        }

        public bool Decrypt(string SourcePath, string DestinationPath, string Password, int BufferLength = 104200)
        {
            var decryptor = GetDecryptor(Password);

            using var destination_decrypted = File.Create(DestinationPath, BufferLength);
            using var destination = new CryptoStream(destination_decrypted, decryptor, CryptoStreamMode.Write);
            using var encrypted_source = File.OpenRead(SourcePath);

            var buffer = new byte[BufferLength];
            int readed;
            do
            {
                readed = encrypted_source.Read(buffer, 0, BufferLength);
                destination.Write(buffer, 0, readed);
            }
            while (readed > 0);

            try
            {
                destination.FlushFinalBlock();
            }
            catch (CryptographicException)
            {
                return false;
            }

            return true;
        }




        public async Task EncryptAsync(
            string SourcePath,
            string DestinationPath,
            string Password,
            int BufferLength = 104200,
            IProgress<double> Progress = null,  //тип данных который нужно возвращать
            CancellationToken Cancel = default)
        {

            if (!File.Exists(SourcePath)) throw new FileNotFoundException("fILE NOT FOUND");
            if (BufferLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(BufferLength), BufferLength, "Маленький размер");

            var encryptor = GetEncryptor(Password/*, Encoding.UTF8.GetBytes(SourcePath)*/);
            try
            {
                await using var destination_encrypted = File.Create(DestinationPath, BufferLength);
                await using var destination = new CryptoStream(destination_encrypted, encryptor, CryptoStreamMode.Write); //поток для шифровки
                await using var source = File.OpenRead(SourcePath);

                var file_length = source.Length;
                var last_percent = 0.0;

                var buffer = new byte[BufferLength];
                int readed;
                do
                {

                    readed = await source.ReadAsync(buffer, 0, BufferLength).ConfigureAwait(false);
                    //ConfigureAwait(false)- выделяет другой поток в пулле потоков в результате делая пользователььский интерфейс быстрее
                    //дополнительные действия по завершению асинхронной операции
                    await destination.WriteAsync(buffer, 0, readed).ConfigureAwait(false);

                    var current_percent = (double)source.Position / file_length;

                    if (current_percent - last_percent >= 0.001)
                    {
                        Progress?.Report(current_percent);
                        last_percent = current_percent;
                    }

                    if (Cancel.IsCancellationRequested)
                    {
                        //ОЧИСТОЧКА ОПЕРАЦИИ
                        Cancel.ThrowIfCancellationRequested();
                    }

                    Thread.Sleep(1);
                }
                while (readed > 0);
                destination.FlushFinalBlock();

                Progress?.Report(1);

            }
            catch (OperationCanceledException e)
            {
                File.Delete(DestinationPath);
                throw;
            }
        }




        public async Task<bool> DecryptAsync(string SourcePath,
            string DestinationPath,
            string Password,
            int BufferLength = 104200,
             IProgress<double> Progress = null,  //тип данных который нужно возвращать
            CancellationToken Cancel = default)
        {
            if (!File.Exists(SourcePath)) throw new FileNotFoundException("fILE NOT FOUND");
            if (BufferLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(BufferLength), BufferLength, "Маленький размер");

            var decryptor = GetDecryptor(Password);


            try
            {
                await using var destination_decrypted = File.Create(DestinationPath, BufferLength);
                await using var destination = new CryptoStream(destination_decrypted, decryptor, CryptoStreamMode.Write);
                await using var decrypted_source = File.OpenRead(SourcePath);

                var file_length = decrypted_source.Length;
                var last_percent = 0.0;


                var buffer = new byte[BufferLength];
                int readed;
                do
                {
                    readed = await decrypted_source.ReadAsync(buffer, 0, BufferLength).ConfigureAwait(false);

                    await destination.WriteAsync(buffer, 0, readed).ConfigureAwait(false);

                    Cancel.ThrowIfCancellationRequested();

                    var current_percent = (double)decrypted_source.Position / file_length;

                    if (current_percent - last_percent >= 0.001)
                    {
                      Progress?.Report(current_percent);
                      last_percent = current_percent;
                    }

                    Thread.Sleep(1);
                }
                while (readed > 0);

                try
                {
                    destination.FlushFinalBlock();
                }
                catch (CryptographicException e)
                {
                    return false;
                }

            }
            catch (OperationCanceledException e)
            {
                File.Delete(DestinationPath);
                throw;
            }
            return true;
        }
    }
}