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

    public static bool IsPhoneValid(string number)
    {
        if (number.Length != 13 || string.IsNullOrEmpty(number))
        {
            return false;
        }

        for (int i = 1; i < number.Length; i++)
        {
            if (!char.IsDigit(number[i]))
            {
                return false;
            }
        }

        return true;
    }
}