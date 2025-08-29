using System.ComponentModel.DataAnnotations;

namespace StringsAreEvil.API.DomainModels;

public class Count
{
    private string _value;

    [Required]
    [StringLength(256)]
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
        }
    }

    public override bool Equals(object obj)
    {
        return obj is Count item &&
               _value == item._value;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(_value);
        return hash.ToHashCode();
    }
}
