namespace  MicroServices.Samples.Services.Ordering.API.Application.Models;


public class Customer
{
    public string Id {get;set;}//trùng với customerId của Order
    public string IdentityId {get;set;}//trùng với customerId của basket
    public string Name {get;set;}
    public string PhoneNumber {get;set;}
}