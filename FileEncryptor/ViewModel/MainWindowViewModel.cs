﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor.ViewModel
{
    internal class MainWindowViewModel : ViewModel
    {

        #region string Title - "MainwINDOW tITLE"

        ///<summary> MainwINDOW tITLE </summary>
        private string _Title = "dawda";

        ///<summary> MainwINDOW tITLE </summary>
        public string Title
        {
            get => _Title;
            set => SetField(ref _Title, value);
        }

        #endregion

        public MainWindowViewModel()
        {
            
        }
    }
}
