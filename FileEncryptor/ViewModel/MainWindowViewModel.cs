using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FileEncryptor.Infrastructure.Commands;
using FileEncryptor.Services.Interfaces;

namespace FileEncryptor.ViewModel
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _UserDialog;
        private readonly IEncryptor _Encryptor;

        private const string __EncryptedFileSuffix = ".encrypted";

        #region Properties


        #region string Title - "MainwINDOW tITLE"

        ///<summary> MainwINDOW tITLE </summary>
        private string _Title = "FileEncryptor";

        ///<summary> MainwINDOW tITLE </summary>
        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        #endregion

        #region string Password - "Password"

        ///<summary> Password </summary>
        private string _Password = "123";

        ///<summary> Password </summary>
        public string Password
        {
            get => _Password;
            set => SetField(ref _Password, value);
        }

        #endregion

        #region FileInfo SelectedFile - "Выбранный Файл"

        ///<summary> Выбранный Файл </summary>
        private FileInfo? _SelectedFile;

        ///<summary> Выбранный Файл </summary>
        public FileInfo? SelectedFile
        {
            get => _SelectedFile;
            set => SetField(ref _SelectedFile, value);
        }

        #endregion


        #endregion


        #region Commands

        #region Command SelectFileCommand - Команда выбора файла


        ///<summary> Команда выбора файла </summary>

        private ICommand _SelectFileCommand;

        ///<summary> Команда выбора файла </summary>

        public ICommand SelectFileCommand => _SelectFileCommand ??=
                                    new LambdaCommand(OnSelectFileCommandExecuted, CanSelectFileCommandExecute);

        ///<summary>Проверка возможности выполнения - Команда выбора файла </summary>

        private bool CanSelectFileCommandExecute(object p) => true;

        ///<summary>Логика выполнения - Команда выбора файла </summary>

        private void OnSelectFileCommandExecuted(object p)
        {
            if(!_UserDialog.OpenFile("Выбор файла для шифрования", out var file_path)) return;
            var selectedFile = new FileInfo(file_path);
            SelectedFile = selectedFile.Exists ? selectedFile : null;
            //SelectedFile = selectedFile ??= null;
        }

        #endregion

        #region Command EncryptCommand - Зашифровать Данные
        ///<summary> Зашифровать Данные </summary>
        private ICommand _EncryptCommand;

        ///<summary> Зашифровать Данные </summary>
        public ICommand EncryptCommand => _EncryptCommand ??=
                                    new LambdaCommand(OnEncryptCommandExecuted, CanEncryptCommandExecute);

        ///<summary>Проверка возможности выполнения - Зашифровать Данные </summary>
        private bool CanEncryptCommandExecute(object p) => (p is FileInfo file && file.Exists || SelectedFile != null) && !string.IsNullOrWhiteSpace(Password);

        ///<summary>Логика выполнения - Зашифровать Данные </summary>
        private void OnEncryptCommandExecuted(object p)
        {
            var file = p as FileInfo ?? SelectedFile;
            if (file == null) return;


            var default_file_name = file.FullName + __EncryptedFileSuffix;
            if (!_UserDialog.SafeFile("Выбор файл для сохранения", out var destination_path, default_file_name)) return;


        }

        #endregion

        #region Command DecryptCommand - Разшифровать


        ///<summary> Разшифровать </summary>

        private ICommand _DecryptCommand;

        ///<summary> Разшифровать </summary>

        public ICommand DecryptCommand => _DecryptCommand ??=
                                    new LambdaCommand(OnDecryptCommandExecuted, CanDecryptCommandExecute);

        ///<summary>Проверка возможности выполнения - Разшифровать </summary>

        private bool CanDecryptCommandExecute(object p) => (p is FileInfo file && file.Exists || SelectedFile != null) && !string.IsNullOrWhiteSpace(Password);

        ///<summary>Логика выполнения - Разшифровать </summary>

        private void OnDecryptCommandExecuted(object p)
        {
            if (!(p is FileInfo file)) return;


            var default_file_name = file.FullName.EndsWith(__EncryptedFileSuffix)
                ? file.FullName.Substring(0, file.FullName.Length - __EncryptedFileSuffix.Length)
                : file.FullName;
            if (!_UserDialog.SafeFile("Выбор файл для сохранения", out var destination_path, default_file_name)) return;

        }

        #endregion


        #endregion
        public MainWindowViewModel(IUserDialog userDialog, IEncryptor encryptor)
        {
            _UserDialog = userDialog;
            _Encryptor = encryptor;
        }
    }
}
