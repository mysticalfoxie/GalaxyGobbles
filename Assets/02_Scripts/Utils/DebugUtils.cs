using System.Reflection;
using UnityEditor;

public class DebugUtils
{
    private static MethodInfo _method; 
    
    public static void ClearConsole()
    {
        _method ??= GetClearMethod();
        _method.Invoke(new object(), null);
    }

    private static MethodInfo GetClearMethod()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        return method;
    }
}