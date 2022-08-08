namespace TDApiGen.Entities;

public record LuaFunc(string Name, LuaFuncParam[] Params, LuaFuncReturn? ReturnValue);