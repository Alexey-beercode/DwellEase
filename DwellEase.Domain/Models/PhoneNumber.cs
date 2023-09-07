namespace DwellEase.Domain.Models;

public class PhoneNumber
{
    public string Number { get; set; } = null!;

    public PhoneNumber(string number)
    {
        if (IsPhoneValid(number))
        {
            Number = number;
        }
    }

    private bool IsPhoneValid(string number)
    {
        if (number.Length != 12 || string.IsNullOrEmpty(number))
        {
            return false;
        }

        foreach (var symbol in number)
        {
            if (!char.IsDigit(symbol))
            {
                return false;
            }
        }

        return true;
    }
}