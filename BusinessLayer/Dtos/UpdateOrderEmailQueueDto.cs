using DataAccessLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Dtos
{
    public class UpdateOrderEmailQueueDto
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string TextBody { get; set; }

        public string Messsage { get; set; }

        public string ImageUrl { get; set; }

        public string TrackOrderUrl { get; set; }

        public long ApplicationId { get; set; }

    }
}
