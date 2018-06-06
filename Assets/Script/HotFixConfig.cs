using System;
using System.Collections.Generic;
using XLua;

public static class HotFixConfig
{
    [Hotfix] 
    public static List<Type> hotFixClassList = new List<Type>()
    {
        typeof(HelloWorld),
    };
}