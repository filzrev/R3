﻿namespace R3
{
    public static partial class EventFactory
    {
        // no scheduler(TimeProvider) overload

        public static CompletableEvent<int, Unit> Range(int start, int count)
        {
            long max = ((long)start) + count - 1;
            if (count < 0 || max > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return Empty<int>();
            }

            return new R3.Factories.Range(start, count);
        }

        public static CompletableEvent<int, Unit> Range(int start, int count, CancellationToken cancellationToken)
        {
            long max = ((long)start) + count - 1;
            if (count < 0 || max > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return Empty<int>();
            }

            return new R3.Factories.RangeC(start, count, cancellationToken);
        }
    }
}

namespace R3.Factories
{
    internal sealed class Range(int start, int count) : CompletableEvent<int, Unit>
    {
        protected override IDisposable SubscribeCore(Subscriber<int, Unit> subscriber)
        {
            for (int i = 0; i < count; i++)
            {
                subscriber.OnNext(start + i);
            }
            subscriber.OnCompleted(default);
            return Disposable.Empty;
        }
    }

    internal sealed class RangeC(int start, int count, CancellationToken cancellationToken) : CompletableEvent<int, Unit>
    {
        protected override IDisposable SubscribeCore(Subscriber<int, Unit> subscriber)
        {
            for (int i = 0; i < count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Disposable.Empty;
                }
                subscriber.OnNext(start + i);
            }
            subscriber.OnCompleted(default);
            return Disposable.Empty;
        }
    }
}