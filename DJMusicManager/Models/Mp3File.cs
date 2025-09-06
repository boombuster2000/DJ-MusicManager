using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using TagLib;

namespace DJMusicManager.Models;

public partial class Mp3File(string filePath) : ObservableObject
{
    private bool _isMetaDataLoaded = false;
    
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
    /// The string version of the artists that are part of the song.
    /// </summary>
    [ObservableProperty]
    public partial string? ArtistsString { get; set; }
    
    partial void OnArtistsStringChanged(string? value)
    {
        if (!_isMetaDataLoaded) return;
        if (value == null) return;
        
        Debug.Print("Setting artists.");
        SetArtists();
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
    public partial string[]? Genres { get; set; }
    
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

            Duration = file.Properties.Duration;
            Genres = file.Tag.Genres;
            Bpm = file.Tag.BeatsPerMinute;
        }
        catch (TagLib.CorruptFileException e)
        {
            Title = $"Corrupt - {this.FilePath}";
        }
        catch (Exception e)
        {
            Debug.Print($"Failed to load {this.FilePath}: {e.Message}");
        }
        
        _isMetaDataLoaded = true;
        
    }

    private void SetArtists()
    {
        try
        {
            using var file = TagLib.File.Create(FilePath);
            file.Tag.Performers = ArtistsArray;
            file.Save();
        }
        catch (Exception ex)
        {
            // handle errors (e.g. file locked, permissions)
            Debug.Print($"Failed to save artists to {this.FilePath}: {ex.Message}");
            ArtistsString = null;
        }
        
    }
}
