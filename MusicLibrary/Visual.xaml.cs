using NAudio.CoreAudioApi;
using System;
using System.Windows;
using System.Windows.Media;

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for Visual.xaml
    /// </summary>
    public partial class Visual : Window
    {
        NAudio.CoreAudioApi.MMDeviceEnumerator enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
        
        public Visual()
        {
            InitializeComponent();
            
            var devices = enumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.Active);
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0.1);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            
            dispatcherTimer.Start();

        }


        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            MediaTimeline timeline = new MediaTimeline();
            //var device = (NAudio.CoreAudioApi.MMDevice)CbDevices.SelectedItem;
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            PbVisual.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 80 + 0.5));
            PbVisual2.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 65 + 0.2));
        }

    }
}
