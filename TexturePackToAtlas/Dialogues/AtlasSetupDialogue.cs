namespace TexturePackToAtlas.Dialogues;

public class AtlasSetupDialogue : IDialogue
{
    private readonly IReadOnlyCollection<int> _outputResolutions;
    private readonly IReadOnlyCollection<int> _singleTileResolution;

    public AtlasSetupDialogue(IEnumerable<int> outputResolutions, IEnumerable<int> singleTileResolution)
    {
        _outputResolutions = outputResolutions.ToList().AsReadOnly();
        _singleTileResolution = singleTileResolution.ToList().AsReadOnly();
    }
    
    public void Run(Action<object> completed)
    {
        while (true)
        {
            try
            {
                var resolutionsString = EnumerableToString(_outputResolutions, ", ");
                var singleTileResolutionsString = EnumerableToString(_singleTileResolution, ", ");
            
                Console.WriteLine("Setup Atlas settings. ");
                Console.WriteLine($"Select Output texture .png file resolution (best: 1024). \nList of available resolutions: [{resolutionsString}]: ");

                var resolutionUserInput = Console.ReadLine();
                if (int.TryParse(resolutionUserInput, out var parsedResolutionInput))
                {
                    if (!_outputResolutions.Contains(parsedResolutionInput))
                    {
                        throw new Exception($"Resolution list [{resolutionsString}] does not contain this number: {parsedResolutionInput}!");
                    }

                    var outputAtlasResolution = parsedResolutionInput;
                    
                    Console.WriteLine($"Output file resolution will be: {outputAtlasResolution} ");
                    
                    
                    
                    Console.WriteLine($"Select Single Block/Tile resolution. Default minecraft texturepack resolution is 16. \nList of available Single Block/Tile resolutions: {singleTileResolutionsString}");
                    
                    var singleTileResolutionUserInput = Console.ReadLine();
                    if (int.TryParse(singleTileResolutionUserInput, out var parsedSingleTileResolutionInput))
                    {
                        if (!_singleTileResolution.Contains(parsedSingleTileResolutionInput))
                        {
                            throw new Exception($"Single tile Resolution list [{singleTileResolutionsString}] does not contain this number: {parsedSingleTileResolutionInput}!");
                        }
                        
                        var outputSingleTileResolution = parsedSingleTileResolutionInput;
                        Console.WriteLine($"Output Tile/Block resolution will be: {outputSingleTileResolution} ");
                        
                        completed?.Invoke(new List<int> {outputAtlasResolution, outputSingleTileResolution});
                        break;
                    }
                }
                else
                {
                    throw new Exception("Input is not a number!");
                }
                
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} \n"); 
            }
        }
    }

    private string EnumerableToString (IEnumerable<int> enumerable, string gap)
    {
        string output = "";
        
        for (int i = 0; i < enumerable.Count(); i++)
        {
            var element = enumerable.ElementAt(i);
            var setGap = i + 1 >= enumerable.Count() ? "" : gap;
            output += $"{element}{setGap}";
        }

        return output;
    }
}