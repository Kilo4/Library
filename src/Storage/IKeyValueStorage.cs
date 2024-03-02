namespace Webapi.Storage;

public interface IKeyValueStorage
{
    int GetValue(string key);
    int SetValue(string key, int inputValue);
}