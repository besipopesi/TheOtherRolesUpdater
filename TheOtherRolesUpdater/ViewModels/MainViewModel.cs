using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TheOtherRolesUpdater.Commands;
using TheOtherRolesUpdater.Exceptions;
using TheOtherRolesUpdater.Models;
using TheOtherRolesUpdater.Scanners;
using TheOtherRolesUpdater.Services;

namespace TheOtherRolesUpdater.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public event EventHandler ScanFoldersStarted;
        public event EventHandler ScanFoldersCompleted;
        public event EventHandler UpdateSelectedFolderStarted;
        public event EventHandler UpdateSelectedFolderCompleted;
        public event EventHandler UpdateAllFoldersStarted;
        public event EventHandler UpdateAllFoldersCompleted;
        public event EventHandler ExceptionThrown;
        public event EventHandler NoAdminRightsForEpicGamesExceptionRaised;

        private List<AmongUsFolder> _amongUsFolders;
        public List<AmongUsFolder> AmongUsFolders
        {
            get => _amongUsFolders;
            set
            {
                if (value != _amongUsFolders)
                {
                    _amongUsFolders = value;
                    RaisePropertyChanged();
                    UpdateSelectedFolderCommand.RaiseCanExecuteChanged();
                    UpdateAllFoldersCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private AmongUsFolder _selectedAmongUsFolder;
        public AmongUsFolder SelectedAmongUsFolder
        {
            get => _selectedAmongUsFolder;
            set
            {
                if (value != _selectedAmongUsFolder)
                {
                    _selectedAmongUsFolder = value;
                    if (value != null)
                        SelectedAmongUsFolder.Refresh();
                    RaisePropertyChanged();
                    UpdateSelectedFolderCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _latestRelease;
        public string LatestRelease
        {
            get => _latestRelease;
            set
            {
                if (value != _latestRelease)
                {
                    _latestRelease = value;
                    RaisePropertyChanged();
                    UpdateSelectedFolderCommand.RaiseCanExecuteChanged();
                    UpdateAllFoldersCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ScanFoldersCommand { get; private set; }
        public DelegateCommand UpdateSelectedFolderCommand { get; private set; }
        public DelegateCommand UpdateAllFoldersCommand { get; private set; }

        private bool _isWorking = false;

        public MainViewModel()
        {
            ScanFoldersCommand = new DelegateCommand(
                (canExecute) => CanScanFoldersCommandRun(),
                ScanFolders());

            UpdateSelectedFolderCommand = new DelegateCommand(
                (canExecute) => CanUpdateSelectedFolderCommandRun(),
                UpdateSelectedFolder());

            UpdateAllFoldersCommand = new DelegateCommand(
                (canExecute) => CanUpdateAllFoldersCommandRun(),
                UpdateAllFolders());
        }

        private void RaiseUiEvent(EventHandler eventHandler)
        {
            _isWorking = !_isWorking;
            eventHandler?.Invoke(this, EventArgs.Empty);
            ScanFoldersCommand.RaiseCanExecuteChanged();
            UpdateSelectedFolderCommand.RaiseCanExecuteChanged();
            UpdateAllFoldersCommand.RaiseCanExecuteChanged();
        }

        private bool CanScanFoldersCommandRun()
        {
            return !_isWorking;
        }

        private Action<object> ScanFolders()
        {
            return async (execute) =>
            {
                RaiseUiEvent(ScanFoldersStarted);
                AmongUsFolders = new List<AmongUsFolder>(await GameFolderScanner.GetGameFolders(GameFolderScannerType.Deep));
                RaiseUiEvent(ScanFoldersCompleted);
            };
        }

        private bool CanUpdateSelectedFolderCommandRun()
        {
            if (SelectedAmongUsFolder == null)
                return false;

            bool canRun = !_isWorking;
            canRun &= !string.IsNullOrEmpty(SelectedAmongUsFolder.Path);
            canRun &= !string.IsNullOrEmpty(SelectedAmongUsFolder.GameVersion);
            canRun &= !SelectedAmongUsFolder.GameVersion.Equals(LatestRelease);
            if (!string.IsNullOrEmpty(LatestRelease))
                canRun &= !LatestRelease.Equals(MagicStrings.NOT_FOUND);
            return canRun;
        }

        private Action<object> UpdateSelectedFolder()
        {
            return async (execute) =>
            {
                RaiseUiEvent(UpdateSelectedFolderStarted);
                try
                {
                    if (SelectedAmongUsFolder.Platform == Platform.EpicGames && !Helpers.HasApplicationAdminRights())
                        throw new NoAdminRightsForEpicGamesException();

                    await PluginInstallService.InstallPlugin(SelectedAmongUsFolder.Path);
                }
                catch (NoAdminRightsForEpicGamesException ex)
                {
                    NoAdminRightsForEpicGamesExceptionRaised?.Invoke(this, new ExceptionEventArgs(ex));
                }
                catch (Exception ex)
                {
                    ExceptionThrown?.Invoke(this, new ExceptionEventArgs(ex));
                }
                SelectedAmongUsFolder.Refresh();
                RaisePropertyChanged(nameof(SelectedAmongUsFolder));
                RaiseUiEvent(UpdateSelectedFolderCompleted);
            };
        }

        private bool CanUpdateAllFoldersCommandRun()
        {
            if (AmongUsFolders == null || !AmongUsFolders.Any())
                return false;

            bool canRun = !_isWorking;
            if (!string.IsNullOrEmpty(LatestRelease))
                canRun &= !LatestRelease.Equals(MagicStrings.NOT_FOUND);
            //if (SelectedAmongUsFolder != null)
            //    canRun &= !string.IsNullOrEmpty(SelectedAmongUsFolder.Path);
            return canRun;
        }

        private Action<object> UpdateAllFolders()
        {
            return async (execute) =>
            {
                RaiseUiEvent(UpdateAllFoldersStarted);
                foreach (AmongUsFolder amongUsFolder in AmongUsFolders)
                {
                    amongUsFolder.Refresh();
                    if (!amongUsFolder.GameVersion.Equals(LatestRelease))
                    {
                        await PluginInstallService.InstallPlugin(amongUsFolder.Path);
                    }
                }
                RaiseUiEvent(UpdateAllFoldersCompleted);
            };
        }
    }
}
