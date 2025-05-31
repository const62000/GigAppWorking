namespace GigApp.Domain.Entities;

public partial class Address
{
    public override string ToString()
    {
        return $"{StreetAddress1} {StreetAddress2}, {City}, {StateProvince}, {Country}";
    }
}

