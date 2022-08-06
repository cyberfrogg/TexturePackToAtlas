namespace TexturePackToAtlas.Dialogues;

public interface IDialogue
{
    void Run(Action<object> completed);
}
