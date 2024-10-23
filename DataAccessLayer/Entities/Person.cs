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
    public DateTime DateOfBirth { get; set; }

    public virtual IEnumerable<PersonAddress> PeopleAddresses { get; set; } = new List<PersonAddress>();

    public virtual IEnumerable<Phone> Phones { get; set; } = new List<Phone>();
}
