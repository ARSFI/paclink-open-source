namespace Paclink.Data
{
    public interface IProperties
    {
        void Delete(string group, string name);
        void DeleteGroup(string group);
        string Get(string group, string name, string defaultValue);
        bool Get(string group, string name, bool defaultValue);
        int Get(string group, string name, int defaultValue);
        void Save(string group, string name, string value);
        void Save(string group, string name, int value);
        void Save(string group, string name, bool value);

        //default/optional interface member
        string ToIniFileFormat()
        {
            return "";
        }
    }
}
