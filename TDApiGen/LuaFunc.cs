namespace TDApiGen;

public record LuaFunc(string Name, LuaFuncParam[] Params, LuaFuncReturn? ReturnValue);

public record LuaFuncParam(string Name, string Type, bool IsOptional, string Desc);

public record LuaFuncReturn(string Name, string Type, string Desc);