using ConsoleApp1;
using Newtonsoft.Json;

namespace ConsoleApp2;

public class GetCoursePrivatBank
{
    private static async Task<List<Exchange>> GetCourse()
    {
        string url = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=11";
        List<Exchange> exchangeRates = new List<Exchange>();
        using HttpClient httpclient = new HttpClient();

        try
        {
            var response = await httpclient.GetAsync(url);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            exchangeRates = JsonConvert.DeserializeObject<List<Exchange>>(jsonResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка : {ex.Message}");
        }
        return exchangeRates;
    }

    public async Task<Exchange> UsdCoursePrivBank()
    {
        var exchangeRates = await GetCourse();

        return exchangeRates.FirstOrDefault(rate => rate.ccy == "USD");
    }

    public async Task<Exchange> EurCoursePrivBank()
    {
        var exchangeRates = await GetCourse();

        return exchangeRates.FirstOrDefault(rate => rate.ccy == "EUR");
    }
}