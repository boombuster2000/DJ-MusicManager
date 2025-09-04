using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DJMusicManager.Models;

public class Folder
{
    /// <summary>
    /// Name of the folder.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The full-path of the folder.
    /// </summary>
    private string FullPath { get; }
    
    /// <summary>
    /// Contains the child folders of the folder.
    /// </summary>
    public ObservableCollection<Folder> SubFolders { get; set; } = [];
    
    /// <summary>
    /// Contains the child mp3 files of the folder.
    /// </summary>
    public ObservableCollection<Mp3File> Mp3Files { get; } = [];

    /// <summary>
    /// Creates empty folder shell.
    /// </summary>
    public Folder()
    {
        Name = "";
        FullPath = "";
    }
    
    /// <summary>
    /// Sets <see cref="Name"/> and <see cref="FullPath"/> and then loads folder with <see cref="LoadAsync()"/>.
    /// </summary>
    ///
    /// <seealso cref="LoadAsync"/>
    /// <param name="rootPath">The path of the selected folder.</param>
    public Folder(string rootPath)
    {
        Name = Path.GetFileName(rootPath);
        FullPath = rootPath;
        _ = LoadAsync();
    }

    /// <summary>
    /// Asynchronously retrieves the subfolders and MP3 files of this folder,
    /// and populates <see cref="SubFolders"/> and <see cref="Mp3Files"/>.
    /// </summary>
    private async Task LoadAsync()
    {
        
        // ReSharper disable once InconsistentNaming
        // Gets files in background
        var mp3s = await Task.Run(() => Directory.GetFiles(FullPath, "*.mp3"));
        var subDirs = await Task.Run(() => Directory.GetDirectories(FullPath));

        // Updates collections on the UI thread
        foreach (var mp3Path in mp3s)
            Mp3Files.Add(new Mp3File(mp3Path));

        // Sorts subfolders to be in order
        var subFolders = subDirs.Select(d => new Folder(d)).OrderBy(f => f.Name);
        foreach (var folder in subFolders)
            SubFolders.Add(folder);
    }
}
