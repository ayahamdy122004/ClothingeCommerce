namespace E_Commerce.services.CachServices
{
    public interface ICachService
    {
        Task CachResponseAsync(string cachkey, object Response, TimeSpan expiration);
        Task<string>GetCachResponseAsync(string cachkey);
    }
}
