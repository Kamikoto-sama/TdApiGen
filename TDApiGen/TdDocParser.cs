using System.Xml;

namespace TDApiGen;

public static class TdDocParser
{
    public static LuaFunc[] Parse(string docXml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(docXml);
        return doc.DocumentElement!
            .Cast<XmlElement>()
            .Select(ParseFunc)
            .ToArray();
    }

    private static LuaFunc ParseFunc(XmlElement func)
    {
        var name = func.GetAttr("name");
        var @params = ParseParams(func.GetChildren("input"));
        var returnValue = ParseReturnValue(func.GetChildren("output"));
        return new LuaFunc(name, @params, returnValue);
    }

    private static LuaFuncReturn? ParseReturnValue(IEnumerable<XmlNode> xmlNodes)
    {
        var returnValue = xmlNodes.FirstOrDefault();
        if (returnValue == null)
            return null;

        var name = returnValue.GetAttr("name");
        var type = ParseType(returnValue.GetAttr("type"));
        var desc = returnValue.GetAttr("desc");
        return new LuaFuncReturn(name, type, desc);
    }

    private static LuaFuncParam[] ParseParams(IEnumerable<XmlNode> @params)
    {
        return @params.Select(param =>
        {
            var name = param.GetAttr("name");
            var type = ParseType(param.GetAttr("type"));
            var optional = bool.Parse(param.GetAttr("optional"));
            var desc = param.GetAttr("desc");
            return new LuaFuncParam(name, type, optional, desc);
        }).ToArray();
    }

    private static readonly HashSet<string> luaTypes = new()
    {
        "number",
        "string",
        "boolean",
        "table"
    };

    private static string ParseType(string type)
    {
        if (luaTypes.Contains(type))
            return type;

        return type switch
        {
            "float" or "int" => $"number [{type}]",
            _ => $"any [{type}]"
        };
    }

    private static string GetAttr(this XmlNode element, string name) => element.Attributes!.GetNamedItem(name)!.Value!;

    private static IEnumerable<XmlNode> GetChildren(this XmlNode element, string nodeTypeName) => element.ChildNodes.Cast<XmlNode>().Where(x => x.Name == nodeTypeName);
}