using System.IO.Compression;

namespace TexturePackToAtlas.Utils;

public class Unzipper
{
    private readonly string _tempFolder;

    public Unzipper(string tempFolder)
    {
        _tempFolder = tempFolder;
    }
    
    public string Unzip(string? path)
    {
        if (path == null || !File.Exists(path))
            throw new FileNotFoundException($"File or Directory does not exists or Path is Null: {path}");

        var destination = $"{_tempFolder}/texturepack";
        Directory.CreateDirectory(destination);
        ZipFile.ExtractToDirectory(path, destination);
        
        return destination;
    }
}