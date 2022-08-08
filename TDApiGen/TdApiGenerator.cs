using TDApiGen.Entities;

namespace TDApiGen;

public static class TdApiGenerator
{
    private const string ApiUrl = "https://www.teardowngame.com/modding/api.html";

    public static IEnumerable<string> GenerateApiLines(IEnumerable<LuaFunc> functions)
    {
        foreach (var func in functions)
        {
            foreach (var line in GenerateFuncDoc(func))
                yield return line;

            yield return GenerateFunSignature(func);
            yield return string.Empty;
        }
    }

    private static string GenerateFunSignature(LuaFunc func)
    {
        var paramsStr = string.Join(", ", func.Params.Select(x => x.Name));
        return $"function {func.Name}({paramsStr}) end";
    }

    private static IEnumerable<string> GenerateFuncDoc(LuaFunc func)
    {
        yield return GenerateDocLine($"{ApiUrl}#{func.Name}");

        foreach (var param in func.Params)
            yield return GenerateDocParam(param);

        var returnValue = func.ReturnValue;
        if (returnValue == null)
            yield break;

        yield return GenerateDocLine("return", returnValue.Type, returnValue.Name, returnValue.Desc);
    }

    private static string GenerateDocParam(LuaFuncParam param)
    {
        var name = param.Name;
        if (param.IsOptional)
            name += "?";

        return GenerateDocLine("param", name, param.Type, param.Desc);
    }

    private static string GenerateDocLine(params string[] @params)
    {
        var linePrefix = "---" + (@params.Length > 1 ? "@" : string.Empty);
        return linePrefix + string.Join(" ", @params);
    }
}