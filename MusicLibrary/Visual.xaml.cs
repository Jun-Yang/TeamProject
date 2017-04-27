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
using System.Windows.Shapes;
using NAudio;
using NAudio.CoreAudioApi;
using System.Windows.Threading;
using System.Runtime.InteropServices;

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
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0.1);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            
            dispatcherTimer.Start();

        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MediaTimeline timeline = new MediaTimeline();
            //var device = (NAudio.CoreAudioApi.MMDevice)CbDevices.SelectedItem;
            var device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            PbVisual.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 80 + 0.5));
            PbVisual2.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 65 + 0.2));
        }

    }
}
