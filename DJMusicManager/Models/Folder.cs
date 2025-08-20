using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DJMusicManager.Models;

public class Folder(string rootPath)
{
    public string Name { get; } = Path.GetFileName(rootPath);
    private string FullPath { get; } = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
    public List<Folder> SubFolders { get; } = new();
    public List<Mp3File> Mp3Files { get; } = new();

    private bool _isLoaded;

    public async Task LoadAsync()
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
        });
    }
}
