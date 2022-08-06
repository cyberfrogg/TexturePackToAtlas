using TexturePackToAtlas.Dialogues;

namespace TexturePackToAtlas;

public class App
{
    private readonly IReadOnlyCollection<IDialogue> _dialogues;
    private readonly string _outputFileName;

    private string? _texturepackPath;
    private int _atlasResolution;
    private int _tileResolution;

    public App(IEnumerable<IDialogue> dialogues, string outputFileName)
    {
        _dialogues = dialogues.ToList().AsReadOnly();
        _outputFileName = outputFileName;
    }
    
    public void Run()
    {
        Console.WriteLine("Welcome to TexturePack to Atlas converter! (by cyberfrogg) \n");
        PrintEndOperationGap();
        
        StartDialogue(0, OnUnZippingCompleted);
    }

    private void OnUnZippingCompleted(object resultOut)
    {
        _texturepackPath = resultOut as string;
        Console.WriteLine($"Unzipping done! Unzipped archive relative path: {_texturepackPath}");
        PrintEndOperationGap();
        
        StartDialogue(1, OnAtlasSetupDone);
    }

    private void OnAtlasSetupDone(object resultOut)
    {
        var result = (List<int>)resultOut;

        _atlasResolution = result[0];
        _tileResolution = result[1];
        
        ConvertToAtlas(_texturepackPath, _outputFileName);
    }

    private void ConvertToAtlas(string texturepackPath, string outputPath)
    {
        var converter = new Converter(texturepackPath, outputPath,_atlasResolution, _tileResolution, true);
        converter.Convert();
        
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        Environment.Exit(1);
    }

    private void StartDialogue(int index, Action<object> completed)
    {
        _dialogues.ElementAt(index).Run(completed);
    }
    private void PrintEndOperationGap()
    {
        Console.WriteLine("\n --------------------------- \n");
    }
}