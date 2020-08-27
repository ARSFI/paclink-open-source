namespace Paclink.Data
{
    public interface IProperties
    {
         void DeleteProperty(string propertyName);
         string GetProperty(string propertyName, string defaultValue);
         bool GetProperty(string propertyName, bool defaultValue);
         int GetProperty(string propertyName, int defaultValue);
         void SaveProperty(string propertyName, string value);
    }
}
