using TexturePackToAtlas.Utils;

namespace TexturePackToAtlas.Dialogues;

public class UnzippingDialogue : IDialogue
{
    public void Run(Action<object> completed)
    {
        var unzipper = new Unzipper("temp");
        
        while (true)
        {
            Console.WriteLine("Type the relative path to texture pack archive: ");
                
            try
            {
                var pathToUnzip = Console.ReadLine();
                Console.WriteLine("Trying to unzip archive...");
                var result = unzipper.Unzip(pathToUnzip);
                completed?.Invoke(result);
                break;
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine($"File not found. Check the relative path"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown Exception in {nameof(Unzipper)} caught: {ex.Message}"); 
            }
        }
    }
}