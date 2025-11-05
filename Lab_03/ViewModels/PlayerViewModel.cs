using Lab_03.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public DelegateCommand SetPackNameCommand { get;}
        public QuestionPackViewModel? ActivePack
        {
            get => _mainWindowViewModel?.ActivePack;
        }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            SetPackNameCommand = new DelegateCommand(SetPackName, CanSetPackName);
        }

        private void SetPackName (object arg)
        {

        }
        private bool CanSetPackName (object obj)
        {
            return true;
        }
    }
}
