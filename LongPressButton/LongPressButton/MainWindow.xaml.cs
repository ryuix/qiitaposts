using LongPressButton.Extensions;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Threading;

namespace LongPressButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.LongPress
                .LongPressAsObservable(TimeSpan.FromSeconds(2))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(_ => this.TestMessage.Text += $"LongPressed!:{DateTime.Now}\n");

            this.Progress
                .ProgressAsObservable(TimeSpan.FromMilliseconds(1))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
                {
                    this.ProgressBar.Value = x;
                    if (x.Equals(100d))
                        this.TestMessage.Text += $"ProgressCompleted!:{DateTime.Now}\n";
                });

            this.Resumable
                .ResumableProgressAsObservable(TimeSpan.FromMilliseconds(1))
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x =>
                {
                    this.ProgressBar.Value = x;
                    if (x.Equals(100d))
                        this.TestMessage.Text += $"ResumableProgressCompleted!:{DateTime.Now}\n";
                });

        }
    }
}
