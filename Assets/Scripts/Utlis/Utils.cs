using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static object GetValue(object src, string Name)
    {
        var field = src.GetType()?.GetField(Name) ?? null;
        if (field == null) return null;

        return field.GetValue(src);
    }

    public static T ToEnum<T>(string str)
    {
        System.Array arr = System.Enum.GetValues(typeof(T));
        foreach(T t in arr)
        {
            if (t.ToString() == str)
                return t;
        }
        return default(T);
    }
}

public static class VResources
{
    static Dictionary<string, Object> resourceCache = new Dictionary<string, Object>();
    public static T Load <T>(string path) where T : Object
    {
        if (path == string.Empty)
            return null;
        else
        {
            if (!resourceCache.ContainsKey(path))
                resourceCache.Add(path, Resources.Load<T>(path));
            return (T)resourceCache[path];
        }
    }
}
