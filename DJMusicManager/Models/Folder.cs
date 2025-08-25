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
        if (_isLoaded) return; // only load once
        _isLoaded = true;

        // Run on background thread so UI stays responsive
        await Task.Run(() =>
        {
            // Add MP3 files
            foreach (var mp3Path in Directory.GetFiles(FullPath, "*.mp3"))
                Mp3Files.Add(new Mp3File(mp3Path));

            // Add subfolders as empty shells — not loading their contents yet
            foreach (var subDir in Directory.GetDirectories(FullPath))
                SubFolders.Add(new Folder(subDir));

            var sorted = SubFolders.OrderBy(f => f.Name).ToList();

            SubFolders.Clear();
            foreach (var folder in sorted)
                SubFolders.Add(folder);
        });
    }
}
