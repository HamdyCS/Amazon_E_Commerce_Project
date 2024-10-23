using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class Person
{
    public long Id { get; set; }

    [Required] 
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }


    [Required]
    public DateOnly DateOfBirth { get; set; }

    public virtual ICollection<PersonAddress> PeopleAddresses { get; set; } = new List<PersonAddress>();

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
