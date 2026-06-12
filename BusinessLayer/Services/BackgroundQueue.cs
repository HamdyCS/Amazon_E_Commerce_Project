using BusinessLayer.Contracks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace BusinessLayer.Services
{
    public class BackgroundQueue<T> : IBackgroundQueue<T> where T : class
    {
        private readonly Channel<T> _channel;

        public BackgroundQueue()
        {
           _channel = Channel.CreateUnbounded<T>();
        }
        public async Task EnQueueAsync(T item)
        {
            await _channel.Writer.WriteAsync(item);
        }

        public async Task<T> DeQueueAsync(CancellationToken cancellationToken)
        {
           return await _channel.Reader.ReadAsync();
        }
    }
}
