using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DJMusicManager.Models;

namespace DJMusicManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// The folder that's currently loaded.
    /// </summary>
    [ObservableProperty] public partial Folder LoadedFolder { get; private set; } =  new Folder();


    [ObservableProperty] public partial Folder SelectedFolder { get; set; } = new Folder();
    
    /// <summary>
    /// Updates <see cref="LoadedFolder"/> with new selected folder which updates the UI.
    /// </summary>
    /// <param name="selectedFolders">The selected folders from the user/pop-up window.</param>
    /// <returns></returns>
    [RelayCommand]
    public Task LoadSelectedFolderCommand(IReadOnlyList<IStorageFolder> selectedFolders)
    {
        if (selectedFolders.Count <= 0) return Task.CompletedTask;
        
        LoadedFolder = new Folder(selectedFolders[0].TryGetLocalPath() ?? string.Empty);
        
        return Task.CompletedTask;
    }


    [RelayCommand]
    public void LoadMp3MetaDataCommand()
    {
        SelectedFolder.LoadMp3s();
    }

}