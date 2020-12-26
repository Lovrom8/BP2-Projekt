using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace BP2Projekt.ViewModels
{
    public abstract class BazniDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; }

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand CloseDialogCommand { get; set; }

        public BazniDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand(() => RequestClose(null));
        }
        
        public virtual void RaiseRequestClose(IDialogResult dialogResult) => RequestClose?.Invoke(dialogResult);
       
        public virtual bool CanCloseDialog() => true;

        public virtual void OnDialogClosed() => Console.WriteLine($"Gasim prozor {Title}");
        
        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}
