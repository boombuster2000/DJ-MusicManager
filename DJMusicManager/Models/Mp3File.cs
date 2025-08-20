using System;

namespace DJMusicManager.Models;

public class Mp3File(string filePath)
{
    private string FilePath { get; } = filePath ?? throw new ArgumentNullException(nameof(filePath));
    public string? Title { get; set; }
    public string[]? Artists { get; set; }
    public TimeSpan Duration { get; private set; }
    public string[]? Genres { get; set; }
    public double? Bpm { get; set; }

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
