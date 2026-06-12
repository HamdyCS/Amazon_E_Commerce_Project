using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracks
{
    public interface IBackgroundQueue<T>
    {
        Task EnQueueAsync(T item);

        Task<T> DeQueueAsync(CancellationToken cancellationToken);

    }
}
