using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ETODesktop
{
    public class MainForm : Form
    {
        private DesktopSettings settings;
        private Panel taskbar;
        private Label clock;
        private Label date;
        private Timer timer;
        private PictureBox wallpaper;
        private Button choose;
        private Button toggleBar;
        private Button startup;
        private Button close;

        public MainForm()
        {
            settings=DesktopSettings.Load();
            Text="ETO Desktop";
            FormBorderStyle=FormBorderStyle.None;
            ShowInTaskbar=false;
            WindowState=FormWindowState.Maximized;
            BackColor=Color.Black;
            Load+=OnLoad;
            FormClosing+=OnClosing;
        }

        private void OnLoad(object sender,EventArgs e)
        {
            BuildWallpaper();
            BuildTaskbar();
            timer=new Timer { Interval=500 };
            timer.Tick+=(s,a)=>UpdateClock();
            timer.Start();
            UpdateClock();
            if(settings.CustomTaskbar) NativeMethods.HideTaskbar();
            ApplyStartup();
        }

        private void BuildWallpaper()
        {
            wallpaper=new PictureBox { Dock=DockStyle.Fill, SizeMode=PictureBoxSizeMode.Zoom, BackColor=Color.Black };
            Controls.Add(wallpaper);
            wallpaper.SendToBack();
            if(!string.IsNullOrEmpty(settings.WallpaperPath) && File.Exists(settings.WallpaperPath))
                TryLoadImage(settings.WallpaperPath);
            else
                wallpaper.BackColor=Color.FromArgb(18,18,24);
        }

        private void TryLoadImage(string path)
        {
            try {
                using(var img=Image.FromFile(path))
                    wallpaper.Image=new Bitmap(img);
            } catch { wallpaper.Image=null; }
        }

        private void BuildTaskbar()
        {
            taskbar=new Panel {
                Height=Math.Max(32,settings.TaskbarHeight),
                Dock=DockStyle.Bottom,
                BackColor=Color.FromArgb(Math.Max(0,Math.Min(255,settings.TaskbarOpacity)),22,22,28)
            };
            Controls.Add(taskbar);

            choose=MakeButton("Wallpaper");
            choose.Click+=(s,e)=>ChooseWallpaper();
            taskbar.Controls.Add(choose);

            toggleBar=MakeButton("Windows Taskbar");
            toggleBar.Left=110;
            toggleBar.Click+=(s,e)=>{ settings.CustomTaskbar=!settings.CustomTaskbar; if(settings.CustomTaskbar) NativeMethods.HideTaskbar(); else NativeMethods.ShowTaskbar(); settings.Save(); };
            taskbar.Controls.Add(toggleBar);

            startup=MakeButton("Startup: "+(settings.StartWithWindows?"ON":"OFF"));
            startup.Left=245;
            startup.Click+=(s,e)=>{ settings.StartWithWindows=!settings.StartWithWindows; ApplyStartup(); startup.Text="Startup: "+(settings.StartWithWindows?"ON":"OFF"); settings.Save(); };
            taskbar.Controls.Add(startup);

            close=MakeButton("Exit");
            close.Left=385;
            close.Click+=(s,e)=>Close();
            taskbar.Controls.Add(close);

            clock=new Label { AutoSize=false, Dock=DockStyle.Right, Width=180, TextAlign=ContentAlignment.MiddleCenter, ForeColor=Color.White, Font=new Font("Segoe UI",11,FontStyle.Bold) };
            taskbar.Controls.Add(clock);
            date=new Label { AutoSize=false, Dock=DockStyle.Right, Width=150, TextAlign=ContentAlignment.MiddleCenter, ForeColor=Color.Gainsboro, Font=new Font("Segoe UI",8) };
            taskbar.Controls.Add(date);
        }

        private Button MakeButton(string text)
        {
            return new Button { Text=text, Height=taskbar==null?34:taskbar.Height-6, Width=105, Top=3, FlatStyle=FlatStyle.Flat, ForeColor=Color.White, BackColor=Color.FromArgb(35,35,44) };
        }

        private void UpdateClock()
        {
            if(clock!=null) clock.Text=DateTime.Now.ToString("HH:mm:ss");
            if(date!=null) date.Text=DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void ChooseWallpaper()
        {
            using(var d=new OpenFileDialog { Filter="Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files|*.*" })
            if(d.ShowDialog()==DialogResult.OK) {
                settings.WallpaperPath=d.FileName;
                settings.WallpaperType=Path.GetExtension(d.FileName).ToLowerInvariant()==".gif"?"GIF":"Image";
                TryLoadImage(d.FileName);
                settings.Save();
            }
        }

        private void ApplyStartup()
        {
            using(var key=Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run",true))
            {
                if(key==null) return;
                string exe=Application.ExecutablePath;
                if(settings.StartWithWindows) key.SetValue("ETODesktop","\""+exe+"\"");
                else key.DeleteValue("ETODesktop",false);
            }
        }

        private void OnClosing(object sender,FormClosingEventArgs e)
        {
            if(timer!=null) timer.Stop();
            NativeMethods.ShowTaskbar();
            settings.Save();
        }
    }
}
