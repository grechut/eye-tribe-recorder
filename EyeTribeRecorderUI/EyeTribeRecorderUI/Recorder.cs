using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace EyeTribeRecorderUI
{
    public class Recorder
    {
        private GazeListener lastGazeListener;
        private Guid userId;
        private int saveCount = 1;
        private DateTime recordingStarted;
        private DateTime recordingFinished;

        private string resultsPath = "RecordingResults";
        private string zippedResultsPath;

        public Recorder(Guid userId)
        {
            this.userId = userId;
            zippedResultsPath = Path.Combine(resultsPath, "Zipped");
            CreateResultsDirectory();
        }

        public void Start()
        {
            lastGazeListener = new GazeListener();
            recordingStarted = DateTime.UtcNow;
        }

        public string Save()
        {
            lastGazeListener.Deactivate();
            recordingFinished = DateTime.UtcNow;
            StringBuilder resultMsg = new StringBuilder();
            resultMsg.AppendLine("Recording time: " + (recordingFinished - recordingStarted).TotalMilliseconds + "(ms)");
            resultMsg.AppendLine("Approximate sampling frequency: " + lastGazeListener.RecordedData.Count() / (recordingFinished - recordingStarted).TotalSeconds + "Hz");
            SaveFile();
            return resultMsg.ToString();
        }

        public void Zip()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Path.Combine(resultsPath, userId.ToString()));
                zip.Save(Path.Combine(zippedResultsPath, userId.ToString() + ".zip"));
            }
        }

        public void CreateResultsDirectory()
        {
            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }
            if (!Directory.Exists(zippedResultsPath))
            {
                Directory.CreateDirectory(zippedResultsPath);
            }

            Directory.CreateDirectory(Path.Combine(resultsPath, userId.ToString()));
        }

        public void SaveFile()
        {
            //            Task.Run(() =>
            //            {
            var fileName = string.Format("{0}_{1}.csv", userId, saveCount++);
            var filePath = Path.Combine(resultsPath, userId.ToString(), fileName);
            File.WriteAllLines(filePath, lastGazeListener.RecordedData.Select(x => x.ToEyeTrackedCsv()));
            //            });
        }
    }
}
