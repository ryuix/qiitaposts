using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LongPressButton.Extensions
{
    public static class ButtonExtensions
    {
        public static IObservable<MouseButtonEventArgs> PreviewMouseDownAsObservable(this ButtonBase button)
            => Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => button.PreviewMouseDown += h,
                h => button.PreviewMouseDown -= h);

        public static IObservable<MouseButtonEventArgs> PreviewMouseUpAsObservable(this ButtonBase button)
            => Observable.FromEvent<MouseButtonEventHandler, MouseButtonEventArgs>(
                h => (s, e) => h(e),
                h => button.PreviewMouseUp += h,
                h => button.PreviewMouseUp -= h);

        public static IObservable<RoutedEventArgs> LostFocusAsObservable(this ButtonBase button)
            => Observable.FromEvent<RoutedEventHandler, RoutedEventArgs>(
                h => (s, e) => h(e),
                h => button.LostFocus += h,
                h => button.LostFocus -= h);

        public static IObservable<long> LongPressAsObservable(this ButtonBase button, TimeSpan time)
        {
            var down = button.PreviewMouseDownAsObservable();
            var up = button.PreviewMouseUpAsObservable();

            return down
                .Select(_ => Observable.Timer(time).TakeUntil(up))
                .Switch();
        }

        public static IObservable<long> LongPressWithProgressAsObservable(this ButtonBase button, TimeSpan time)
        {
            var down = button.PreviewMouseDownAsObservable();
            var up = button.PreviewMouseUpAsObservable();

            return down
                .Select(_ => Observable.Interval(time).TakeUntil(up))
                .Switch();
        }

        public static IObservable<double> ProgressAsObservable(this ButtonBase button, TimeSpan time)
        {
            var down = button.PreviewMouseDownAsObservable();
            var up = button.PreviewMouseUpAsObservable();
            var progress = down.Select(_ => Observable.Generate(0d, i => i <= 100, i => ++i, i => i, i => time).TakeUntil(up));

            return progress.Switch();
        }

        public static IObservable<double> ResumableProgressAsObservable(this ButtonBase button, TimeSpan time)
        {
            double value = 0;
            double limit = 100d;

            var down = button.PreviewMouseDownAsObservable();
            var up = button.PreviewMouseUpAsObservable();

            var increment = down
                .Do(_ => 
                {
                    if (value.Equals(limit))
                        value = 0;
                })
                .Select(_ => Observable.Generate(value, i => i <= 100, i => ++i, i => i, i => time).TakeUntil(up));

            var decrement = up
                .Where(_ => !value.Equals(limit))
                .Select(_ => Observable.Generate(value, i => i >= 0d, i => --i, i => i, i => time).TakeUntil(down));

            return Observable.Merge(increment, decrement).Switch().Do(x => value = x);
        }
    }
}
