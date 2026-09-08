namespace FolderSynchronizer.App
{
    public interface IFolderSynchronizationService
    {
        public void Synchronize(string sourceFolder, string replicaFolder);
    }
}
