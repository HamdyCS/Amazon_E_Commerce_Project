using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Contracks
{
    public interface IEmailQueue : IBackgroundQueue<EmailQueueDto>
    {
    }
}
