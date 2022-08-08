namespace TDApiGen;

public static class TdApiDocProvider
{
    private const string ApiDocUrl = "https://www.teardowngame.com/modding/api.xml";

    public static async Task<string> GetXmlDocAsync()
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(ApiDocUrl);
        return await response.Content.ReadAsStringAsync();
    }
}