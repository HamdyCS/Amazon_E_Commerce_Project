using DataAccessLayer.Validitions;
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


    [Required,CustomValidation(typeof(PersonValidtion), "DateOfBirthValidtion")]
    public DateTime DateOfBirth { get; set; }

   

   
}
