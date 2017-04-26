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

namespace MusicLibrary
{
    /// <summary>
    /// Interaction logic for MediaProperty.xaml
    /// </summary>
    public partial class MediaProperty : Window
    {
        public MediaProperty()
        {
            InitializeComponent();
        }
        
        private void MediaProperty_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBoxEx.Show("Do you want to save change?", "MusicPlayer", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //MiSave_Click(null, null);
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }
    }
}
