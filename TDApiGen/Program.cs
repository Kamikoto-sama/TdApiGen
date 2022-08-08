using TDApiGen;

const string apiUrl = "https://www.teardowngame.com/modding/api.xml";

var outputPath = args.Length > 0 ? args[0] : "./teardown-api.lua";

var client = new HttpClient();
var response = await client.GetAsync(apiUrl);
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine("Downloaded");

var generator = new TdApiGenerator(content, outputPath);
await generator.GenerateAsync();