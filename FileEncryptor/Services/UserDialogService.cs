﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FileEncryptor.Services.Interfaces;
using FileEncryptor.View.Windows;
using Microsoft.Win32;

namespace FileEncryptor.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool OpenFile(string Title, out string SelectedFile, string Filter = "Вск файлы (*.*)|*.*")
        {
            var file_dialog = new OpenFileDialog()
            {
                Title = Title,
                Filter = Filter
            };


            if (file_dialog.ShowDialog() != true)
            {
                SelectedFile = null;
                return false;
            }

            SelectedFile = file_dialog.FileName;

            return true;

        }

        public bool OpenFiles(string Title, out IEnumerable<string> SelectedFiles, string Filter = "Вск файлы (*.*)|*.*")
        {
            var file_dialog = new OpenFileDialog()
            {
                Title = Title,
                Filter = Filter
            };

            if (file_dialog.ShowDialog() != true)
            {
                SelectedFiles = Enumerable.Empty<string>();
                return false;
            }

            SelectedFiles = file_dialog.FileNames;

            return true;

        }

        public bool SafeFile(string Title, out string SelectedFile, string DefaultFileName = null, string Filter = "Вск файлы (*.*)|*.*")
        {
            var file_dialog = new SaveFileDialog()
            {
                Title = Title,
                Filter = Filter
            };
            if(!string.IsNullOrWhiteSpace(DefaultFileName))
                file_dialog.FileName = DefaultFileName;


            if (file_dialog.ShowDialog() != true)
            {
                SelectedFile = null;
                return false;
            }

            SelectedFile = file_dialog.FileName;

            return true;
        }

        public void Information(string Title, string Message)
        {
            MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Warning(string Title, string Message)
        {
            MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void Error(string Title, string Message)
        {
            MessageBox.Show(Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public (IProgress<double> Progress, IProgress<string> Status, CancellationToken Cancel, Action Close) ShowProgress(string Title)
        {
            var progress_wondow = new ProgressWindow()
            {
                Title = Title,
                Owner = App.FocusedWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            progress_wondow.Show();

            return (progress_wondow.ProgressInformer, progress_wondow.StatusInformer, progress_wondow.Cancel,
                progress_wondow.Close);

        }

    }
}
