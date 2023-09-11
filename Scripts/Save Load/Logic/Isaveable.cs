namespace Zain.Save
{
    public interface Isaveable
    {
        //Ö»¶Á
        string GUID { get; }

        void RegisterSaveable()
        {
            SavaLoadManager.Instance.RegisterSaveable(this);
        }
        GameSaveData GenerateSaveData();
        void RestoreData(GameSaveData saveData);
    }
}