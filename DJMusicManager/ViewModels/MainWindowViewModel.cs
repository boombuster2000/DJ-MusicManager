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
    public Task LoadSelectedFolderCommand(IReadOnlyList<IStorageFolder> selectedFolders)
    {
        if (selectedFolders.Count <= 0) return Task.CompletedTask;
        
        SelectedFolder = new Folder(selectedFolders[0].TryGetLocalPath() ?? string.Empty);
        
        return Task.CompletedTask;
    }

}