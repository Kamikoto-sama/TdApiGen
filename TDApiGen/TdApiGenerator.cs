namespace TDApiGen;

public class TdApiGenerator
{
    private const string ApiUrl = "https://www.teardowngame.com/modding/api.html";

    private readonly LuaFunc[] functions;
    private readonly string outputPath;

    public TdApiGenerator(string docXml, string outputPath)
    {
        functions = TdDocParser.Parse(docXml);
        this.outputPath = outputPath;
    }

    public async Task GenerateAsync()
    {
        await using var fileStream = File.OpenWrite(outputPath);
        fileStream.Position = 0;

        await using var textWriter = new StreamWriter(fileStream);
        foreach (var func in functions)
        {
            await WriteFuncDoc(textWriter, func);
            await WriteFunSignature(textWriter, func);
            await textWriter.WriteLineAsync();
        }

        await textWriter.FlushAsync();
    }

    private static async Task WriteFunSignature(TextWriter textWriter, LuaFunc func)
    {
        var paramsStr = string.Join(", ", func.Params.Select(x => x.Name));
        var line = $"function {func.Name}({paramsStr}) end";
        await textWriter.WriteLineAsync(line);
    }

    private static async Task WriteFuncDoc(TextWriter textWriter, LuaFunc func)
    {
        await textWriter.WriteLineAsync($"---{ApiUrl}#{func.Name}");

        foreach (var param in func.Params)
            await WriteDocParam(textWriter, param);

        await WriteDocReturnValue(textWriter, func.ReturnValue);
    }

    private static async Task WriteDocReturnValue(TextWriter textWriter, LuaFuncReturn? returnValue)
    {
        if (returnValue == null)
            return;

        var returnLine = $"---@return {returnValue.Type} {returnValue.Name} {returnValue.Desc}";
        await textWriter.WriteLineAsync(returnLine);
    }

    private static async Task WriteDocParam(TextWriter textWriter, LuaFuncParam param)
    {
        var name = param.Name;
        if (param.IsOptional)
            name += "?";

        var line = $"---@param {name} {param.Type} {param.Desc}";
        await textWriter.WriteLineAsync(line);
    }
}