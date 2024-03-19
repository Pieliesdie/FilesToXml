using System;

namespace FilesToXml.Core.Helpers;

public static class Disposable
{
    public static IDisposable Create(Action action)
    {
        return new AnonymousDisposable(action);
    }

    private readonly struct AnonymousDisposable : IDisposable
    {
        private readonly Action dispose;
        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose;
        }
        public void Dispose()
        {
            dispose?.Invoke();
        }
    }
}