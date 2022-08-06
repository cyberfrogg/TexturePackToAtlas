
using System.Drawing;

namespace TexturePackToAtlas;

public class Converter
{
    private readonly string _texturepackPath;
    private readonly string _outputFileName;
    private readonly int _atlasResolution;
    private readonly int _oneTileResolution;
    private readonly bool _writeToConsole;
    private readonly string _assetsFolder;
    private readonly string _minecraftTexturesFolder;

    public Converter(string texturepackPath, string outputFileName, int atlasResolution, int oneTileResolution, bool writeToConsole)
    {
        _texturepackPath = texturepackPath;
        _outputFileName = outputFileName;
        _atlasResolution = atlasResolution;
        _oneTileResolution = oneTileResolution;
        _writeToConsole = writeToConsole;

        _assetsFolder = $"{_texturepackPath}/assets";
        _minecraftTexturesFolder = $"{_assetsFolder}/minecraft/textures";
    }

    public void Convert()
    {
        var filtered = FilterFiles(GetAllFilesInDirectory(_minecraftTexturesFolder));
        Log($"Got {filtered.Count()} filtered files with resolution {_oneTileResolution}x{_oneTileResolution} in texturepack");
        Log("Converting files to atlas");
        var outputImage = FilesToAtlas(filtered);
        Log("Saving atlas to file...");
        outputImage.Save(_outputFileName);
        Log($"--- Done! --- Path: {_outputFileName}");
    }

    private Image FilesToAtlas(IEnumerable<string> files)
    {
        var filesArray = files.ToList();
        var atlas = new Bitmap(_atlasResolution, _atlasResolution);

        int index = 0;
        for (int y = 0; y < _atlasResolution; y += _oneTileResolution)
        {
            if (index + 1 > filesArray.Count)
                break;
            
            for (int x = 0; x < _atlasResolution; x += _oneTileResolution)
            {
                if (index + 1 > filesArray.Count)
                    break;

                try
                {
                    Log($"Drawing [{index.ToString("00000")}] tile: {filesArray[index]} to atlas.");

                    var tile = Image.FromFile(filesArray[index]);
                    Graphics g = Graphics.FromImage(atlas);
                    g.DrawImage(tile, new Point(x, y));
                }
                catch (Exception ex)
                {
                    Log($"Failed to proceed file at index: {index}");
                }
                
                
                index++;
            }
        }

        return atlas;
    }

    private IEnumerable<string> FilterFiles(IEnumerable<string> paths)
    {
        var pngFiles = paths.Where(x => Path.GetExtension(x) == ".png");
        var matchSizeFiles = pngFiles.Where(x =>
        {
            var image = Image.FromFile(x);
            return image.Width == _oneTileResolution && image.Height == _oneTileResolution;
        });
        
        return matchSizeFiles;
    }

    private void Log(string message)
    {
        if(_writeToConsole)
            Console.WriteLine(message);
    }
    
    private IEnumerable<string> GetAllFilesInDirectory(string path) {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(path);
        while (queue.Count > 0) {
            path = queue.Dequeue();
            try {
                foreach (string subDir in Directory.GetDirectories(path)) {
                    queue.Enqueue(subDir);
                }
            }
            catch(Exception ex) {
                Console.Error.WriteLine(ex);
            }
            string[] files = null;
            try {
                files = Directory.GetFiles(path);
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex);
            }
            if (files != null) {
                for(int i = 0 ; i < files.Length ; i++) {
                    yield return files[i];
                }
            }
        }
    }
}