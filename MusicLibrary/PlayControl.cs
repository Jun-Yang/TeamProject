using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicLibrary
{
    partial class PlayControl
    {
        internal PlayMode mode;
        internal static bool isPlaying = false;
        internal static MediaPlayer mediaPlayer = new MediaPlayer();
        internal static double savedVolume = 0;

        private PlayControl()
        {
            mediaPlayer.MediaEnded += new EventHandler(Media_Ended);
            
        }

        private void Media_Ended(object sender, EventArgs e)
        {
            //if (mode == PlayMode.Sequence)
            //{
            //    BtPl
            //}

        }

        internal static void Play(Image imgObj)
        {
            if (imgObj != null)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/image/pause.png");
                img.EndInit();
                imgObj.Source = img;
                mediaPlayer.Play();
                isPlaying = true;
            }
            else
            {
                MessageBox.Show("Internal error");
            }
        }

        internal static void Pause(Image imgObj)
        {
            if (imgObj != null)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/image/play.png");
                img.EndInit();
                imgObj.Source = img;
                mediaPlayer.Pause();
                isPlaying = false;
            }
            else
            {
                MessageBox.Show("Internal error");
            }
        }

        internal static void Stop(Image imgObj)
        {
            if (imgObj != null)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/image/play.png");
                img.EndInit();
                imgObj.Source = img;
                mediaPlayer.Stop();
                isPlaying = false;
            }
            else
            {
                MessageBox.Show("Internal error");
            }
        }

        internal static void Speaker(Image imgObj)
        {
            if (imgObj != null)
            {
                if (mediaPlayer.Volume != 0)
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/image/mute.png");
                    img.EndInit();
                    imgObj.Source = img;
                    savedVolume = mediaPlayer.Volume;
                    mediaPlayer.Volume = 0;
                }
                else
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/image/loud.png");
                    img.EndInit();
                    imgObj.Source = img;
                    mediaPlayer.Volume = savedVolume;
                }
            }
            else
            {
                MessageBox.Show("Internal error");
            }
        }

        internal static void SetVolume(double v)
        {
            mediaPlayer.Volume = v;
            savedVolume = v;
        }    
    }
}
