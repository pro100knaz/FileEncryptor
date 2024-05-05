using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.Services.Interfaces
{
    internal interface IUserDialog
    {
        bool OpenFile(string Title, out string SelectedFile, string Filter = "Вск файлы (*.*)|*.*");
        bool OpenFiles(string Title, out IEnumerable<string> SelectedFiles, string Filter = "Вск файлы (*.*)|*.*");

        bool SafeFile(string Title, out string SelectedFile, string DefaultFileName = null, string Filter = "Все файлы (*.*)|*.*");

        void Information(string Title , string Message);
        void Warning(string Title , string Message);
        void Error(string Title , string Message);
    }
}
