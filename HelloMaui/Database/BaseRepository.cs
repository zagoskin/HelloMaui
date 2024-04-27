using Polly;
using Polly.Retry;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace HelloMaui.Database;
public abstract class BaseRepository<TModel> where TModel : class, new()
{
    private readonly Lazy<SQLiteAsyncConnection> _lazyDatabaseConnection;
    private SQLiteAsyncConnection DatabaseConnection => _lazyDatabaseConnection.Value;
    protected BaseRepository(IFileSystem fileSystem)
    {
        var databasePath = Path.Combine(fileSystem.AppDataDirectory, "SqliteDatabase.db3");
        _lazyDatabaseConnection = new(() =>
        {
            return new SQLiteAsyncConnection(
                databasePath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        });
    }

    protected async Task<TReturn> Execute<TReturn>(Func<SQLiteAsyncConnection, Task<TReturn>> action, CancellationToken token, int maxRetries = 10)
    {   
        var connection = await GetDatabaseConnection().ConfigureAwait(false);
        var resiliencePipeline = new ResiliencePipelineBuilder<TReturn>()
            .AddRetry(new RetryStrategyOptions<TReturn>()
            {
                MaxRetryAttempts = maxRetries,
                Delay = TimeSpan.FromMilliseconds(2),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            })            
            .Build();

        return await resiliencePipeline.ExecuteAsync(async _ => await action(connection), token).ConfigureAwait(false);
    }

    private async ValueTask<SQLiteAsyncConnection> GetDatabaseConnection()
    {
        if (DatabaseConnection.TableMappings.All(m => m.MappedType != typeof(TModel)))
        {
            await DatabaseConnection.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
            await DatabaseConnection.CreateTableAsync(typeof(TModel)).ConfigureAwait(false); 
        }        
        return DatabaseConnection;
    }
}
