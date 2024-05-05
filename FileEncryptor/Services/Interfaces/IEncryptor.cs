using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.Services.Interfaces
{
    internal interface IEncryptor
    {
        void Encrypt(string SiurcePath, string DestinationPath, string Password, int BufferLength = 104200);
        bool Decrypt(string SiurcePath, string DestinationPath, string Password, int BufferLength = 104200);

        Task EncryptAsync(string SourcePath, string DestinationPath, string Password, int BufferLength = 104200);

        Task<bool> DecryptAsync(string SiurcePath, string DestinationPath, string Password, int BufferLength = 104200);

    }
}
