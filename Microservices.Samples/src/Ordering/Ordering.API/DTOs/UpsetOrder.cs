using System.ComponentModel.DataAnnotations;

namespace MicroServices.Samples.Services.Ordering.API.DTOs;


public class UpsertOrder
{
    public string Street { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string AdditionalAddress { get; set; }
    [StringLength(11,ErrorMessage ="IdentityCard must be 11 number")]
    public string IdentityId { get; set; }
    public string CustomerName { get; set; }
    [StringLength(8,ErrorMessage ="IdentityCard must be 8 number")]
    public string PhoneNumber { get; set; }
}