using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using DJMusicManager.Models;
using DJMusicManager.ViewModels;

namespace DJMusicManager.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Opens native folder picker and then triggers the command <see cref="MainWindowViewModel.LoadSelectedFolderCommand"/>.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void SelectFolderAsync_OnClick(object? sender, RoutedEventArgs e)
    {
        var selectedFolders = await StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions
            {
                Title = "Select a Folder",
                AllowMultiple = false
            });
        
        _ = (DataContext as MainWindowViewModel)?.LoadSelectedFolderCommand(selectedFolders);
    }

    private void TreeView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count <= 0) return;
        // Get the first newly selected item
        var selected = e.AddedItems[0];

        // If your items are of type Folder
        if (selected is Folder folder)
        {
            (DataContext as MainWindowViewModel)?.LoadMp3MetaDataCommand();
        }

    }
}