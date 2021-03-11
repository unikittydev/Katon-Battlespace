
namespace Game
{
    public interface IGameData
    {
        string OnSave();

        void OnLoad(string json);
    }
}
