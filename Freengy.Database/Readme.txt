Before using database contexts one must setup static initializer - Initializer class.
It will hold DB paths shared between all contexts:

Initializer.SetStorageDirectoryPath(appDataFolderPath);
Initializer.SetDbFileName(dbFileName);