namespace Webapi.Storage;

public interface IKeyValueStorage
{
    decimal? GetValue(string key);
    void SetValue(string key, decimal value);
}