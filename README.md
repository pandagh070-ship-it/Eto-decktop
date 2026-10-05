# ETO Desktop

Independent Windows desktop customization app.

Target: Windows 7 SP1 and newer with .NET Framework 4.8.

Current MVP:
- Image wallpaper
- GIF image loading
- Custom bottom taskbar overlay
- Digital clock and date
- Windows taskbar hide/show
- Start with Windows
- Persistent XML settings
- Emergency restore: ETODesktop.exe --restore

Designed to stay lightweight for older PCs.

Planned:
- True animated GIF playback optimization
- Video wallpaper
- Taskbar position/style editor
- Widgets
- Theme presets
- Drag-and-drop wallpaper
- Performance mode for low-end GPUs

Build with Visual Studio 2019/2022 or MSBuild:
msbuild ETODesktop.sln /p:Configuration=Release
