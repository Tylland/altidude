namespace Altidude.Domain
{
    public interface ICheckpointStorage
    {
        void Save(string name, string token);
        string GetToken(string token);
    }
}
