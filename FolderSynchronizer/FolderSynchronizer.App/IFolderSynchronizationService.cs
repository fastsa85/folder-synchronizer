using System;
using System.Collections.Generic;
using System.Text;

namespace FolderSynchronizer.App
{
    public interface IFolderSynchronizationService
    {
        public void Synchronize(string sourceFolder, string replicaFolder);
    }
}
