namespace HelloMaui.Utils;

public class SimulatedDelay : IAsyncDisposable, IDisposable
{
    private bool _disposedValue;
    private readonly TimeSpan _delay;

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~SimulatedDelay()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public SimulatedDelay(TimeSpan delay)
    {
        _delay = delay;
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();

        Dispose(disposing: false);

        GC.SuppressFinalize(this);
    }

    private async Task DisposeAsyncCore()
    {
        await Task.Delay(_delay).ConfigureAwait(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            _disposedValue = true;
        }
    }


    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
