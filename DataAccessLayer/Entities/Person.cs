using DataAccessLayer.Validitions;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities;

public partial class Person
{
    public long Id { get; set; }


    public string FirstName { get; set; }


    public string LastName { get; set; }


    [CustomValidation(typeof(PersonValidtion), "DateOfBirthValidtion")]
    public DateTime? DateOfBirth { get; set; }


}
