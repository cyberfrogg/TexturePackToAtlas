using TexturePackToAtlas.Dialogues;

namespace TexturePackToAtlas
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            CleanTempFolder();
            
            var dialogues = new List<IDialogue>()
            {
                new UnzippingDialogue(),
                new AtlasSetupDialogue(new []{128, 256, 512, 1024, 2048, 4096, 8192, 16384}, new []{8, 16, 32, 64, 128, 512, 1024, 2048, 4096})
            };
            
            var app = new App(dialogues, "output.png");
            
            app.Run();
        }

        private static void CleanTempFolder()
        {
            if(!Directory.Exists("temp"))
                return;

            Directory.Delete("temp", true);
        }
    }
}