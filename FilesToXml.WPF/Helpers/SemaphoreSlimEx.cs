namespace FilesToXml.WPF.Helpers;

public static class SemaphoreSlimEx
{
    public static Task UseSemaphoreAsync(this SemaphoreSlim semaphoreSlim, Action action)
    {
        return semaphoreSlim.UseSemaphoreAsync(() =>
        {
             action();
             return 0;
        });
    }
    public static async Task<T> UseSemaphoreAsync<T>(this SemaphoreSlim semaphoreSlim, Func<T> action)
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            return action();
        }
        finally
        {
            semaphoreSlim.Release();
        }
    }
}