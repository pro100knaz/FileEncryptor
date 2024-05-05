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

        public ICommand SelectFileCommand => _SelectFileCommand ??
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


        #endregion
        public MainWindowViewModel(IUserDialog userDialog)
        {
            _UserDialog = userDialog;
        }
    }
}
