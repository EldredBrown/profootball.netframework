using System;
using System.ComponentModel;
using EldredBrown.ProFootball.WpfApp.ViewModels;
using EldredBrown.ProFootball.WpfApp.ViewModels.FocusVMLib;

namespace EldredBrown.ProFootball.WpfApp.Interfaces
{
    public interface IViewModelBase
    {
        bool RequestUpdate { get; set; }
        DelegateCommand UpdateCommand { get; }

        event EventHandler<MoveFocusEventArgs> MoveFocus;
        event PropertyChangedEventHandler PropertyChanged;
    }
}
