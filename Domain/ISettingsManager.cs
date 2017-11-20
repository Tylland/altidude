namespace Altidude.Domain
{
    public interface ISettingsManager
    {
        void Save(string name, string value);
        string GetValue(string name);
    }
}
