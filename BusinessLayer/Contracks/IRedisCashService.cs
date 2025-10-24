namespace BusinessLayer.Contracks
{
    public interface IRedisCashService
    {
        Task<T?> GetValueByKeyAsync<T>(string key);

        Task SetValueByKeyAsync<T>(string key, T value, TimeSpan? expiry = null);
    }
}
