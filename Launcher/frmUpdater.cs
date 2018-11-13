using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using File = System.IO.File;

namespace Intersect_Updater
{
    public partial class frmUpdater : Form
    {
        public Settings settings;
        private static readonly string[] Scopes = new[] { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        private static ConcurrentQueue<Update> UpdateList = new ConcurrentQueue<Update>();
        private static long UpdateSize = 0;
        private static long DownloadedBytes = 0;
        private static long FilesToDownload = 0;
        private static long FilesDownloaded = 0;
        private static bool CheckingForUpdates = true;
        private static int DotCount = 0;
        private static Thread[] UpdateThreads = new Thread[10];
        private TransparentLabel lbl;

        public frmUpdater()
        {
            InitializeComponent();
            lbl = new TransparentLabel(lblStatus);
            lbl.ForeColor = Color.White;
        }

        private void frmUpdater_Load(object sender, EventArgs e)
        {
            //Try to load up icon
            var icon = Icon.ExtractAssociatedIcon(EntryAssemblyInfo.ExecutablePath);
            if (icon != null) this.Icon = icon;
            
            var settingsToTry = new List<string>();
            settingsToTry.Add("settings.json");
            var loadedSettings = false;
            foreach (var directory in Directory.GetDirectories(Directory.GetCurrentDirectory()))
            {
                if (File.Exists(Path.Combine(directory, "settings.json")))
                {
                    settingsToTry.Add(Path.Combine(directory, "settings.json"));
                }
            }
            foreach (var settingsFile in settingsToTry)
            {
                try
                {
                    settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile));
                    loadedSettings = true;
                    break;
                }
                catch (Exception ex)
                {
                    
                }
            }

            if (!loadedSettings)
            {
                MessageBox.Show(@"Could not find settings.json in updater folder or subdirectories! Closing now!");
                this.Close();
                return;
            }
            if (loadedSettings && string.IsNullOrEmpty(settings.FolderId) || string.IsNullOrEmpty(settings.ApiKey))
            {
                MessageBox.Show(@"Missing folder id or api key in settings. Updater will now close!");
                this.Close();
                return;
            }
            if (!string.IsNullOrEmpty(settings.Background))
            {
                if (File.Exists(settings.Background))
                {
                    var launcherImage = File.ReadAllBytes(settings.Background);
                    try
                    {
                        using (var ms = new MemoryStream(launcherImage))
                        {
                            picBackground.BackgroundImage = Bitmap.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            this.Text = settings.UpdaterTitle;
            Task.Run(Run);
        }

        private async Task Run()
        {
            Google.Apis.Services.BaseClientService.Initializer bcs = new Google.Apis.Services.BaseClientService.Initializer();
            bcs.ApiKey = settings.ApiKey;
            bcs.ApplicationName = "Intersect Updater";
            bcs.GZipEnabled = true;

            Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(bcs);
            await CheckFilesRecursively(service,"", settings.FolderId);

            lbl.MeasureString = null;

            var updating = UpdateList.Count > 0;
            FilesToDownload = UpdateList.Count;
            CheckingForUpdates = false;
            if (UpdateList.Count > 0)
            {
                List<Update> updates = new List<Update>();
                updates.AddRange(UpdateList);

                UpdateList = new ConcurrentQueue<Update>();
                var updatePaths = new HashSet<string>();
                for (int i = 0; i < updates.Count; i++)
                {
                    if (!string.IsNullOrEmpty(settings.Background))
                    {
                        var backgroundpath = Path.GetFullPath(settings.Background);
                        var updatePath = Path.GetFullPath(updates[i].FilePath);
                        if (backgroundpath == updatePath)
                        {
                            var update = updates[i];
                            updates.Remove(updates[i]);
                            updatePaths.Add(update.FilePath);
                            UpdateList.Enqueue(update);
                            break;
                        }
                    }
                }


                var updatesToRemove = new List<Update>();
                foreach (var update in updates)
                {
                    if (!updatePaths.Contains(update.FilePath))
                    {
                        updatePaths.Add(update.FilePath);
                    }
                    else
                    {
                        updatesToRemove.Add(update);
                    }
                }

                foreach (var update in updatesToRemove)
                {
                    updates.Remove(update);
                }

                foreach (var update in updates)
                {
                    UpdateList.Enqueue(update);
                }

                BeginInvoke((Action)(() => UpdateStatus()));
                for (int i = 0; i < UpdateThreads.Length; i++)
                {
                    UpdateThreads[i] = new Thread(DownloadFiles);
                    UpdateThreads[i].Start();
                }
            }

            var threadsRunning = true;
            while (threadsRunning && updating)
            {
                threadsRunning = false;
                foreach (var thread in UpdateThreads)
                {
                    if (thread.IsAlive) threadsRunning = true;
                    break;
                }
                Application.DoEvents();
            }

            if (updating)
            {
                BeginInvoke((Action)(() => lbl.Text = @"Update complete! Launching game!"));
            }
            else
            {
                BeginInvoke((Action)(() => lbl.Text = @"No updates found! Launching game!"));
            }
           
            //Launch Game
            await Wait();
            string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();
            var path = Path.Combine(AssemblyPath, settings.LaunchApplication);
            var workingDir = Path.GetDirectoryName(path);
            var psi = new ProcessStartInfo(path);
            psi.WorkingDirectory = workingDir;
            Process.Start(psi);
            BeginInvoke((Action)(() => Close()));
        }

        private void UpdateStatus()
        {
            var percentage = ((float) FilesDownloaded / (float) FilesToDownload) * 100f;
            lbl.Text = "Updates found! Downloading updates! " +  (int)percentage + "% Done";
        }

        private async Task Wait()
        {
            System.Threading.Thread.Sleep(5000);
        }

        private void DownloadFiles()
        {
            var backoff = 1000;
            Google.Apis.Services.BaseClientService.Initializer bcs = new Google.Apis.Services.BaseClientService.Initializer();
            bcs.ApiKey = settings.ApiKey;
            bcs.ApplicationName = "Intersect Updater";
            bcs.GZipEnabled = true;

            Google.Apis.Drive.v3.DriveService service = new Google.Apis.Drive.v3.DriveService(bcs);
            while (!UpdateList.IsEmpty)
            {
                Update update;
                if (UpdateList.TryDequeue(out update))
                {
                    if (DownloadUpdate(service, update))
                    {

                    }
                    else
                    {
                        //Back off
                        UpdateList.Enqueue(update);
                        backoff = backoff * 2;
                        System.Threading.Thread.Sleep(backoff);
                    }
                }
            }

        }

        private bool DownloadUpdate(DriveService service,Update update)
        {
            var request = service.Files.Get(update.UpdateFile.Id);
            var updatePath = Path.GetFullPath(update.FilePath);
            if (updatePath == Path.GetFullPath(EntryAssemblyInfo.ExecutablePath))
            {
                return true; //Don't try to update self -- it won't work!
            }
            if (File.Exists(update.FilePath)) File.Delete(update.FilePath);
            using (var stream = new FileStream(update.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {

                // Add a handler which will be notified on progress changes.
                // It will notify on each chunk download and when the
                // download is completed or failed.
                long lastProgress = 0;
                var succeeded = true;
                request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                            {
                                var diff = progress.BytesDownloaded - lastProgress;
                                DownloadedBytes += diff;
                                lastProgress = progress.BytesDownloaded;
                                break;
                            }
                            case DownloadStatus.Completed:
                            {
                                var diff = progress.BytesDownloaded - lastProgress;
                                DownloadedBytes += diff;
                                lastProgress = progress.BytesDownloaded;
                                if (!string.IsNullOrEmpty(settings.Background))
                                {
                                    var backgroundpath = Path.GetFullPath(settings.Background);

                                    if (backgroundpath == updatePath)
                                    {
                                        picBackground.BackgroundImage = Bitmap.FromStream(stream);
                                    }
                                }
                                    FilesDownloaded++;
                                break;
                            }
                            case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                succeeded = false;
                                break;
                            }
                        }
                        BeginInvoke((Action) (() => UpdateStatus()));
                    };
                request.Download(stream);
                System.Threading.Thread.Sleep(1000);
                return succeeded;
            }
        }

        private async Task CheckFilesRecursively(DriveService service, string currentFolder, string folderId, string nextPageToken = null)
        {
            // Define parameters of request.
            Google.Apis.Drive.v3.FilesResource.ListRequest listRequest = service.Files.List();
            string folderID = folderId; //Change this for your folder ID.
            listRequest.Q = "'" + folderID + "' in parents";
            listRequest.PageSize = 100;
            if (!string.IsNullOrEmpty(nextPageToken)) listRequest.PageToken = nextPageToken;
            listRequest.Fields = "nextPageToken, files(id, name, md5Checksum, size, mimeType)";

            //Start at root, look for all files that we don't have or files that need updating and grab them!
            // List files.
            FileList fileList = listRequest.Execute();
            IList<Google.Apis.Drive.v3.Data.File> files = fileList.Files;

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (IsFolder(file.MimeType))
                    {
                        if (!Directory.Exists(Path.Combine(currentFolder, file.Name)))
                            Directory.CreateDirectory(Path.Combine(currentFolder, file.Name));
                        await CheckFilesRecursively(service, Path.Combine(currentFolder, file.Name), file.Id, null);
                    }
                    else if (IsFile(file.MimeType))
                    {
                        if (!File.Exists(Path.Combine(currentFolder, file.Name)))
                        {
                            //Queue File Up
                            UpdateList.Enqueue(new Update(Path.Combine(currentFolder,file.Name),file));
                            UpdateSize += file.Size ?? 0;
                        }
                        else
                        {
                            //if file size or md5 doesn't match, queue file up
                            long length = new System.IO.FileInfo(Path.Combine(currentFolder, file.Name)).Length;
                            string md5Hash = "";
                            using (var md5 = MD5.Create())
                            {
                                using (var stream =
                                    new BufferedStream(File.OpenRead(Path.Combine(currentFolder, file.Name)), 1200000))
                                {
                                    md5Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
                                }
                            }
                            if (md5Hash != file.Md5Checksum || length != file.Size)
                            {
                                UpdateList.Enqueue(new Update(Path.Combine(currentFolder, file.Name), file));
                                UpdateSize += file.Size ?? 0;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(fileList.NextPageToken))
                CheckFilesRecursively(service, currentFolder, folderID, fileList.NextPageToken);
        }

        private bool IsFolder(string mimeType)
        {
            if (mimeType == "application/vnd.google-apps.folder") return true;
            return false;
        }

        private bool IsFile(string mimeType)
        {
            if (!mimeType.Contains("vnd.google-apps")) return true;
            return false;
        }

        private void lbl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Transparent);
        }

        private void picBackground_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(175, 0, 0, 0)), 0, lblStatus.Top, this.Width, lblStatus.Height);
        }

        private void UpdateStatus(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatus), new object[] { text });
                return;
            }
            lblStatus.Text = text;
            lbl.Text = text;
            lbl.MeasureString = @"Please wait, checking for updates" + new string('.', 3);
        }

        private void tmrChecking_Tick(object sender, EventArgs e)
        {
            if (CheckingForUpdates)
            {
                DotCount++;
                if (DotCount > 3) DotCount = 0;
                UpdateStatus(@"Please wait, checking for updates" + new string('.', DotCount));
            }
        }
    }
}
