using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Play();
            timer?.Start();
        }

        private void PauseButon_OnClick(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Pause();
            timer?.Stop();
        }

        private void StopButon_OnClick(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Stop();
            timer?.Stop();
        }

        private void VideoPlayer_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show($"File {VideoPlayer.Source} not found");
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            VideoPlayer.ScrubbingEnabled = true;
            VideoPlayer.Stop();
            UpdateTimerValue();
        }

        private void VideoPlayer_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            TimerSlider.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.01);
            timer.Tick += UpdateTimeSlider;
        }

        void UpdateTimeSlider(object sender, EventArgs e)
        {
            TimerSlider.Value = VideoPlayer.Position.TotalSeconds;
            UpdateTimerValue();
        }

        private void UpdateTimerValue()
        {
            string videoPlayerActualTime = VideoPlayer.Position.ToString(@"hh\:mm\:ss");
            string videoPlayerTotalTime =
                VideoPlayer.NaturalDuration.HasTimeSpan
                    ? VideoPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss")
                    : "00:00:00";
            TimerValue.Content = $"{videoPlayerActualTime}/{videoPlayerTotalTime}";
        }

        private void TimerSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            VideoPlayer.Position = TimeSpan.FromSeconds(TimerSlider.Value);
            VideoPlayer.Play();
        }

        private void TimerSlider_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            VideoPlayer.Pause();
            timer?.Stop();
        }

        private void TimerSlider_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            VideoPlayer.Play();
            timer?.Start();
        }

        private void BackwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            var seconds = VideoPlayer.Position.TotalSeconds;
            seconds -= 5;
            if (seconds < 0)
                seconds = 0;

            VideoPlayer.Position = TimeSpan.FromSeconds(seconds);
        }

        private void ForwardButton_OnClick(object sender, RoutedEventArgs e)
        {
            var seconds = VideoPlayer.Position.TotalSeconds;
            seconds += 5;
            if (seconds > VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds)
                seconds = VideoPlayer.NaturalDuration.TimeSpan.TotalSeconds;

            VideoPlayer.Position = TimeSpan.FromSeconds(seconds);
        }
    }
}
