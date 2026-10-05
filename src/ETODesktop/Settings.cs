using System;
using System.IO;
using System.Xml.Serialization;

namespace ETODesktop
{
    [Serializable]
    public class DesktopSettings
    {
        public string WallpaperPath { get; set; }
        public string WallpaperType { get; set; }
        public bool CustomTaskbar { get; set; }
        public bool StartWithWindows { get; set; }
        public bool ShowClock { get; set; }
        public bool ShowDate { get; set; }
        public int TaskbarHeight { get; set; }
        public int TaskbarOpacity { get; set; }

        public static DesktopSettings Load()
        {
            string dir=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"ETODesktop");
            string file=Path.Combine(dir,"settings.xml");
            try {
                if(File.Exists(file))
                    using(var fs=File.OpenRead(file)) return (DesktopSettings)new XmlSerializer(typeof(DesktopSettings)).Deserialize(fs);
            } catch {}
            return new DesktopSettings { WallpaperType="None", CustomTaskbar=true, ShowClock=true, ShowDate=true, TaskbarHeight=42, TaskbarOpacity=92 };
        }

        public void Save()
        {
            string dir=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"ETODesktop");
            Directory.CreateDirectory(dir);
            string file=Path.Combine(dir,"settings.xml");
            using(var fs=File.Create(file)) new XmlSerializer(typeof(DesktopSettings)).Serialize(fs,this);
        }
    }
}
