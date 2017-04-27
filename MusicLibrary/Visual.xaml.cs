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

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for Visual.xaml
    /// </summary>
    public partial class Visual : Window
    {
        public Visual()
        {
            InitializeComponent();
            NAudio.CoreAudioApi.MMDeviceEnumerator enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.Active);

            CbDevices.Items.Add(devices.ToArray());
            //CbDevices.Items.


        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (CbDevices.SelectedItem != null)
            {
                var device = (NAudio.CoreAudioApi.MMDevice)CbDevices.SelectedItem;
                PbVisual.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100 + 0.5));
            }
        }
    }
}
