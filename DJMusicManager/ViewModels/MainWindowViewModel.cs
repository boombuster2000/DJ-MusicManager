using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DJMusicManager.Models;

namespace DJMusicManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] public partial Folder SelectedFolder { get; private set; } =  new Folder();


    [RelayCommand]
    public Task LoadSelectedFolderCommand(IReadOnlyList<IStorageFolder> folders)
    {
        if (folders.Count <= 0) return Task.CompletedTask;
        
        Debug.Print("Loading folders");
        SelectedFolder = new Folder(folders[0].TryGetLocalPath() ?? string.Empty);
        
        return Task.CompletedTask;
    }

}