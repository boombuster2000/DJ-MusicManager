using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DJMusicManager.Models;

public class Folder
{
    public string Name { get; }
    private string FullPath { get; }
    
    public ObservableCollection<Folder> SubFolders { get; set; } = [];
    public ObservableCollection<Mp3File> Mp3Files { get; } = [];

    private bool _isLoaded;
    
    public Folder(string rootPath)
    {
        Name = Path.GetFileName(rootPath);
        FullPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        if (_isLoaded) return;
        _isLoaded = true;

        // Gather file system info in background
        var mp3s = await Task.Run(() => Directory.GetFiles(FullPath, "*.mp3"));
        var subDirs = await Task.Run(() => Directory.GetDirectories(FullPath));

        // Now update collections on the UI thread
        foreach (var mp3Path in mp3s)
            Mp3Files.Add(new Mp3File(mp3Path));

        var subFolders = subDirs.Select(d => new Folder(d)).OrderBy(f => f.Name);
        foreach (var folder in subFolders)
            SubFolders.Add(folder);
    }
}
