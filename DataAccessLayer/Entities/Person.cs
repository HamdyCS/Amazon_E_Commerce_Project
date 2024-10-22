using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class Person
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<PeopleAddress> PeopleAddresses { get; set; } = new List<PeopleAddress>();

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
