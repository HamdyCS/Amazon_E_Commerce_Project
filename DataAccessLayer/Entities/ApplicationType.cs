using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public  class ApplicationType
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? DescriptionEn { get; set; }

        public string? DescriptionAr { get; set; }

        [NotMapped]
        public EnApplicationType enApplicationType => (EnApplicationType)Id;

        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}


