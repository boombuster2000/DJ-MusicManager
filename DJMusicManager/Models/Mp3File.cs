using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DJMusicManager.Models;

public partial class Mp3File(string filePath) : ObservableObject
{
    private bool _isMetaDataLoaded;
    
    /// <summary>
    /// This is the file-path of the mp3 file.
    /// </summary>
    private string FilePath { get; } = filePath;
    

    /// <summary>
    /// This is the title of the mp3 file.
    /// </summary>
    [ObservableProperty]
    public partial string? Title { get; set; }
    partial void OnTitleChanged(string? value)
    {
        if (!_isMetaDataLoaded) return;
        WriteTitleToFile();
    }
    
    
    /// <summary>
    /// The string version of the artists that are part of the song.
    /// </summary>
    [ObservableProperty]
    public partial string? ArtistsString { get; set; }
    partial void OnArtistsStringChanged(string? value)
    {
        if (!_isMetaDataLoaded) return;
        WriteArtistsToFile();
    }
    
    /// <summary>
    /// Array of artists performed in song, taken from <see cref="ArtistsString"/> and split with ";".
    /// <remarks> Artists should be seperated by ";" for it to be recognised as separate artists.</remarks>
    /// </summary>
    private string[]? ArtistsArray => ArtistsString?.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    
    
    /// <summary>
    /// How long the song is.
    /// </summary>
    [ObservableProperty]
    public partial TimeSpan Duration { get; private set; }
    
    /// <summary>
    /// The genres of the song would fit into.
    /// </summary>
    [ObservableProperty]
    public partial string? GenresString { get; set; }

    partial void OnGenresStringChanged(string? value)
    {
        if (!_isMetaDataLoaded) return;
        WriteGenresToFile();
    }
    
    private string[]? Genres => GenresString?.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// The bpm of the song.
    /// </summary>
    [ObservableProperty]
    public partial double? Bpm { get; set; }
    
    
    public void LoadMetaData()
    {
        if (_isMetaDataLoaded) return;
        
        try
        {
            using var file = TagLib.File.Create(this.FilePath);
            Title = file.Tag.Title;

            // Different Artists should be seperated using ";".
            // Users may have seperated artists using "," and therefore will be seen as 1 artist.
            ArtistsString = file.Tag.JoinedPerformers;
            GenresString = file.Tag.JoinedGenres;

            Duration = file.Properties.Duration;
            Bpm = file.Tag.BeatsPerMinute;
        }
        catch (TagLib.CorruptFileException)
        {
            Title = $"Corrupt - {this.FilePath}";
        }
        catch (Exception e)
        {
            Debug.Print($"Failed to load {this.FilePath}: {e.Message}");
        }
        
        _isMetaDataLoaded = true;
        
    }

    private void WriteArtistsToFile()
    {
        try
        {
            using var file = TagLib.File.Create(FilePath);
            file.Tag.Performers = ArtistsArray;
            file.Save();
        }
        catch (Exception e)
        {
            // handle errors (e.g. file locked, permissions)
            Debug.Print($"Failed to save artists to {this.FilePath}: {e.Message}");
            ArtistsString = null;
        }
    }

    private void WriteTitleToFile()
    {
        try
        {
            using var file = TagLib.File.Create(FilePath);
            file.Tag.Title = Title;
            file.Save();
        }
        catch (Exception e)
        {
            Debug.Print($"Failed to save title to {this.FilePath}: {e.Message}");
            Title = null;
        }
    }

    private void WriteGenresToFile()
    {
        try
        {
            using var file = TagLib.File.Create(FilePath);
            file.Tag.Genres = Genres;
            file.Save();
        }
        catch (Exception e)
        {
            Debug.Print($"Failed to save genres to {this.FilePath}: {e.Message}");
            GenresString = null;
        }
    }
}
