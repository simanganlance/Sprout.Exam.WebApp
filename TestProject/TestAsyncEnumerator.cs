﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _innerEnumerator;

    public TestAsyncEnumerator(IEnumerator<T> innerEnumerator)
    {
        _innerEnumerator = innerEnumerator ?? throw new ArgumentNullException(nameof(innerEnumerator));
    }

    public T Current => _innerEnumerator.Current;

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(_innerEnumerator.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        _innerEnumerator.Dispose();
        return new ValueTask();
    }
}
