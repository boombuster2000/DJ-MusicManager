using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DJMusicManager.ViewModels;

namespace DJMusicManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void SelectFolderAsync_OnClick(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "Select a Folder",
                AllowMultiple = false
            });
        
        _ = (DataContext as MainWindowViewModel)?.LoadSelectedFolderCommand(folders);
    }
}