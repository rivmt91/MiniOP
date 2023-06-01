using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniOP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiniOP.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        MainModel _model = new MainModel();
        
        #region Properties
        [ObservableProperty]
        private int _kv;

        [ObservableProperty]
        private double _maMas;

        [ObservableProperty]
        private int _mode;

        [ObservableProperty]
        private int _sec;

        [ObservableProperty]
        private bool _isPulse = false;

        [ObservableProperty]
        private bool _isFluoro = false;

        [ObservableProperty]
        private bool _isRad = false;

        [ObservableProperty]
        private string? _portName;

        [ObservableProperty]
        private string? _statusImg = "Resources/STATUS_BEGIN.png";


        #endregion




        [RelayCommand]
        private void KvDown()
        {
            Thread.Sleep(1);
            _model.Send("KEY", "   12");
        }

        [RelayCommand]
        private void KvUp()
        {
            Thread.Sleep(1);
            _model.Send("KEY", "   11");
        }

        [RelayCommand]
        private void MaMasUp()
        {
            Thread.Sleep(1);
            if (Mode == 3) _model.Send("KEY", "   15");
            else _model.Send("KEY", "   13");

        }

        [RelayCommand]
        private void MaMasDown()
        {
            Thread.Sleep(1);
            if (Mode == 3) _model.Send("KEY", "   16");
            else _model.Send("KEY", "   14");
        }

        [RelayCommand]
        private void ModeFluoro()
        {
            Thread.Sleep(1);
            _model.Send("MOD", "    1");
            IsPulse = false;
            IsFluoro = true;
            IsRad = false;
        }

        [RelayCommand]
        private void ModePulse()
        {
            Thread.Sleep(1);
            _model.Send("MOD", "    2");
            IsPulse = true;
            IsFluoro = false;
            IsRad = false;
        }

        [RelayCommand]
        private void ModeRad()
        {
            Thread.Sleep(1);
            _model.Send("MOD", "    3");
            IsPulse = false;
            IsFluoro = false;
            IsRad = true;
        }

        [RelayCommand]
        private void TimeReset()
        {
            Thread.Sleep(1);
            _model.Send("KEY", "   19");
        }


        internal void ConnectCOMCommand(string? selectedPort)
        {
            _model.Connect(selectedPort); 
        }

        internal void DisconnectCOMCommand()
        {
            _model.Disconnect();
        }


    }
}
