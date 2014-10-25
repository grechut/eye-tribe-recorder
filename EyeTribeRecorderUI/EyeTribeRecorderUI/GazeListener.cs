using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TETCSharpClient;
using TETCSharpClient.Data;

namespace EyeTribeRecorderUI
{
    public class GazeListener : IGazeListener
    {
        private int screenResX;
        private int screenResY;

        public List<Point> RecordedData { get; set; }

        public GazeListener()
        {
            GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
            if (GazeManager.Instance.IsActivated == false)
            {
                throw new Exception("Cannot activate the eye tracker");
            }
            if (GazeManager.Instance.IsCalibrated == false)
            {
                throw new Exception("Eye tracker is not calibrated");
            }

            screenResX = GazeManager.Instance.ScreenResolutionWidth;
            screenResY = GazeManager.Instance.ScreenResolutionHeight;
            RecordedData = new List<Point>();
            GazeManager.Instance.AddGazeListener(this);
        }

        public void OnGazeUpdate(GazeData gazeData)
        {
            double gX = gazeData.SmoothedCoordinates.X / screenResX;
            double gY = gazeData.SmoothedCoordinates.Y / screenResY;

            Console.WriteLine("(x,y): " + gX + ", " + gY);
            // todo: what should we do with 0,0 values?
            RecordedData.Add(new Point()
            {
                X = gX,
                Y = gY
            });
        }

        public void Deactivate()
        {
            GazeManager.Instance.Deactivate();
        }
    }

    public class Point
    {
        public double X { get; set; }

        public double Y { get; set; }

        public string ToEyeTrackedCsv()
        {
            return string.Format("{0};{1};", X.ToString("F2"), Y.ToString("F2"));
        }
    }
}
