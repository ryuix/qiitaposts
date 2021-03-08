using AllowOnlyOneOpen.Extensions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllowOnlyOneOpen.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactivePropertySlim<bool> AIsExpanded { get; } = new();
        public ReactivePropertySlim<bool> BIsExpanded { get; } = new();
        public ReactivePropertySlim<bool> CIsExpanded { get; } = new();
        public ReactivePropertySlim<bool> DIsExpanded { get; } = new();


        public MainWindowViewModel()
        {
            var group = new [] { AIsExpanded, BIsExpanded, CIsExpanded, DIsExpanded };

            group.AllowOnlyOneTrue();

        }
    }
}
