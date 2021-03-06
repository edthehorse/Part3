using System;
using System.Windows.Input;

using NorthwindUwp.Helpers;
using NorthwindUwp.Models;
using NorthwindUwp.Services;

using Windows.UI.Xaml;

namespace NorthwindUwp.ViewModels
{
    public class CategoryMasterDetailDetailViewModel : Observable
    {
        private const string NarrowStateName = "NarrowState";

        private const string WideStateName = "WideState";

        private ICommand _stateChangedCommand;

        public ICommand StateChangedCommand
        {
            get
            {
                if (_stateChangedCommand == null)
                {
                    _stateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
                }

                return _stateChangedCommand;
            }
        }

        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public CategoryMasterDetailDetailViewModel()
        {
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
