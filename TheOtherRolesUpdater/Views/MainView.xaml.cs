using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using TheOtherRolesUpdater.Models;
using TheOtherRolesUpdater.Scanners;
using TheOtherRolesUpdater.Services;
using TheOtherRolesUpdater.ViewModels;

namespace TheOtherRolesUpdater.Views
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    /// 

    public partial class MainView : UserControl
    {
        private MainViewModel viewModel;

        public MainView()
        {
            Loaded += OnLoaded;

            InitializeComponent();
        }

        protected async override void OnInitialized(EventArgs e)
        {
            viewModel = DataContext as MainViewModel;

            (IEnumerable<AmongUsFolder> loadedAmongUsFolders, AmongUsFolder selectedAmongUsFolder) = SavedFoldersService.ReadSavedFolders();

            if (loadedAmongUsFolders.Any())
                viewModel.AmongUsFolders = new List<AmongUsFolder>(loadedAmongUsFolders);
            else
                viewModel.AmongUsFolders = new List<AmongUsFolder>(Task.Run(() => GameFolderScanner.GetGameFolders(GameFolderScannerType.Quick)).Result);

            if (selectedAmongUsFolder != null)
                viewModel.SelectedAmongUsFolder = selectedAmongUsFolder;

            viewModel.LatestRelease = await LatestReleaseVersionScanner.GetLatestReleaseVersion();

            viewModel.ScanFoldersStarted += ScanFoldersStarted;
            viewModel.ScanFoldersCompleted += ScanFoldersCompleted;
            viewModel.UpdateSelectedFolderStarted += UpdateSelectedFolderStarted;
            viewModel.UpdateSelectedFolderCompleted += UpdateSelectedFolderCompleted;
            viewModel.UpdateAllFoldersStarted += UpdateAllFoldersStarted;
            viewModel.UpdateAllFoldersCompleted += UpdateAllFoldersCompleted;
            viewModel.ExceptionThrown += ExceptionThrown;
            viewModel.NoAdminRightsForEpicGamesExceptionRaised += NoAdminRightsForEpicGamesExceptionRaised;

            base.OnInitialized(e);
        }

        private void ScanFoldersStarted(object sender, EventArgs e)
        {
            ScanFoldersProgressBar.Visibility = Visibility.Visible;
        }

        private void ScanFoldersCompleted(object sender, EventArgs e)
        {
            ScanFoldersProgressBar.Visibility = Visibility.Hidden;
        }

        private void UpdateSelectedFolderStarted(object sender, EventArgs e)
        {
            UpdateSelectedFolderProgressBar.Visibility = Visibility.Visible;
        }

        private void UpdateSelectedFolderCompleted(object sender, EventArgs e)
        {
            UpdateSelectedFolderProgressBar.Visibility = Visibility.Hidden;
        }

        private void UpdateAllFoldersStarted(object sender, EventArgs e)
        {
            UpdateAllFoldersProgressBar.Visibility = Visibility.Visible;
        }

        private void UpdateAllFoldersCompleted(object sender, EventArgs e)
        {
            UpdateAllFoldersProgressBar.Visibility = Visibility.Hidden;
        }

        private void ExceptionThrown(object sender, EventArgs e)
        {
            ExceptionEventArgs args = (ExceptionEventArgs)e;
            MessageBox.Show(args.Message, args.Caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void NoAdminRightsForEpicGamesExceptionRaised(object sender, EventArgs e)
        {
            ExceptionEventArgs args = (ExceptionEventArgs)e;
            MessageBox.Show(args.Message, args.Caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Closing += OnParentWindowClosing;
        }

        private void OnParentWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SavedFoldersService.WriteSavedFolders(viewModel.AmongUsFolders, viewModel.SelectedAmongUsFolder);
        }
    }
}
