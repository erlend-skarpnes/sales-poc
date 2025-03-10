using SalesToolPoc.ApiService.Email;

namespace SalesPoc.ApiService.Tests2;

public class UnitTest1
{
    public UnitTest1(IEmailService emailService)
    {
        
    }
    [Fact]
    public void Test1()
    {
        string? mikael;
        {
            using var test = new UnitTest1()
            {
                mikael = test.ToString();
            };

        }
        var anne = mikael.Length;
    }

}
