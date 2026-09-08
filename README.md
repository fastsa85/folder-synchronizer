# folder-synchronizer
A program that synchronizes two folders: source and replica. The program maintains a full, identical copy of source folder at replica folder

## Usage

```text
FolderSynchronizer <source> <replica> <intervalSeconds> <logFile>
```

## Features

- The replica is kept identical to the source:
New and changed files are copied
Obsolete files and directories are removed

- Synchronization runs periodically
- Logs are written to both console and file

## Testing

The solution uses a testing pyramid strategy:

Unit tests — synchronization logic and logging behavior
Integration tests — filesystem integration file logging
E2E tests — real application process, CLI arguments, periodic synchronization, file changes and cleanup

## Assumptions and Limitations
- Synchronization is one-way only from source to replica
- File changes are detected using file size and last modified time
- Files are copied only when they are new or changed
- Renamed files considered as new and are copied (the related files with old name in replica are considered as obsolete and removed)
- The application assumes source and replica are different folders
- The console logging was intentionally not covered by automated tests because it is standard .net feature (tested only by manual sanity check)
- The application does not store the synchronization state between runs (after re-start the full copying is performed again)
- If the application is stopped while a synchronization cycle is in progress, the replica may be left in a partially synchronized state.