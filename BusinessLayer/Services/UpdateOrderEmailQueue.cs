using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UpdateOrderEmailQueue : BackgroundQueue<UpdateOrderEmailQueueDto>,IUpdateOrderEmailQueue
    {
    }
}
