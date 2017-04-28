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
    /// Interaction logic for PrintMusicLibraryWindow.xaml
    /// </summary>
    public partial class PrintMusicLibraryWindow : Window
    {
        public PrintMusicLibraryWindow(List<Song> ListMusicLibrary)
        {
            InitializeComponent();

            DgMusicLibrary.AutoGenerateColumns = false;
            DgMusicLibrary.ItemsSource = ListMusicLibrary;
        }

        private void BtPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                DgMusicLibrary.Measure(pageSize);
                DgMusicLibrary.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(DgMusicLibrary, Title);
            }
        }

        private void BtCancel_Clcik(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
