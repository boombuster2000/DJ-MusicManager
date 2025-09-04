using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DJMusicManager.Models;

public partial class Mp3File(string filePath) : ObservableObject
{
    /// <summary>
    /// This is the file-path of the mp3 file.
    /// </summary>
    private string FilePath { get; } = filePath;

    /// <summary>
    /// This is the title of the mp3 file.
    /// </summary>
    [ObservableProperty]
    public partial string? Title { get; set; }
    
    /// <summary>
    /// The artists that are part of the song.
    /// </summary>
    ///
    [ObservableProperty]
    public partial string[]? Artists { get; set; }
    
    /// <summary>
    /// How long the song is.
    /// </summary>
    [ObservableProperty]
    public partial TimeSpan Duration { get; private set; }
    
    /// <summary>
    /// The genres of the song would fit into.
    /// </summary>
    [ObservableProperty]
    public partial string[]? Genres { get; set; }
    
    /// <summary>
    /// The bpm of the song.
    /// </summary>
    [ObservableProperty]
    public partial double? Bpm { get; set; }

    public void LoadMetaData()
    {
        var file = TagLib.File.Create(FilePath);
        Title = file.Tag.Title;
        
        // If performers exist, split names into separate elements
        // By default it does not split artists names into individual elements.
        Artists = file.Tag.Performers is { Length: > 0 } ? file.Tag.Performers[0].Split(", ") : [];

        Duration = file.Properties.Duration;
        Genres = file.Tag.Genres;
        Bpm = file.Tag.BeatsPerMinute;

    }
}
