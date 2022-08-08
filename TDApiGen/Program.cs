using TDApiGen;

var outputPath = args.Length > 0 ? args[0] : "./teardown-api.lua";

var apiDoc = await TdApiDocProvider.GetXmlDocAsync();
var luaFunctions = TdApiDocParser.Parse(apiDoc);
var apiLines = TdApiGenerator.GenerateApiLines(luaFunctions);
await File.WriteAllLinesAsync(outputPath, apiLines);