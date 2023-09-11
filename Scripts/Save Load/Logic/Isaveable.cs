namespace Zain.Save
{
    public interface Isaveable
    {
        //ֻ��
        string GUID { get; }

        void RegisterSaveable()
        {
            SavaLoadManager.Instance.RegisterSaveable(this);
        }
        GameSaveData GenerateSaveData();
        void RestoreData(GameSaveData saveData);
    }
}