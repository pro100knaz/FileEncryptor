using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileEncryptor.View.Windows
{
    public partial class ProgressWindow
    {
        #region readonly Status : string -  Статусное сообщение

        ///<summary> Статусное сообщение </summary>
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(
                nameof(Status),
                typeof(string),
                typeof(ProgressWindow),
                new PropertyMetadata(default(string)));


        ///<summary>  Статусное сообщение </summary>
        //[Category("")]
        [Description("summary")]
        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        #endregion

        #region readonly ProgressValue : double - Процент выполнения статусного окна

        ///<summary> Процент выполнения статусного окна </summary>
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register(
                nameof(ProgressValue),
                typeof(double),
                typeof(ProgressWindow),
                new PropertyMetadata(double.NaN, OnProgressChanged));

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progress_value = (double)e.NewValue;
            var progress_view = ((ProgressWindow)d).ProgressView;

            progress_view.Value = progress_value;
            progress_view.IsIndeterminate = double.IsNaN(progress_value);

        }

        ///<summary> Процент выполнения статусного окна </summary>
        //[Category("")]
        [Description("Процент выполнения статусного окна")]
        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        #endregion


        private IProgress<double> _ProgressInformer;

        public IProgress<double> ProgressInformer => _ProgressInformer ??= new Progress<double>(p => ProgressValue = p);

        private IProgress<string> _StatusInformer;

        public IProgress<string> StatusInformer => _StatusInformer ??= new Progress<string>(status => Status = status);


        private IProgress<(double Percent, string Message)> _ProgressStatusInformer;

        public IProgress<(double Percent, string Message)> ProgressStatusInformer => _ProgressStatusInformer
            ??= new Progress<(double Percent, string Message)>(
                p =>
                {
                    ProgressValue = p.Percent;
                    Status = p.Message;
                });

        private CancellationTokenSource _cancellation;
        public CancellationToken Cancel
        {
            get
            {
                if (_cancellation != null) return _cancellation.Token;
                _cancellation = new CancellationTokenSource();
                CancelButton.IsEnabled = true;
                return _cancellation.Token;
            }
        }

        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            _cancellation?.Cancel();
        }

    }
}
