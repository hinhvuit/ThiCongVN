using System.Reflection;
using System.Runtime.Loader;

namespace AttcMN.Framework.Reflection;

/// <summary>
/// ????掩
/// </summary>
internal static class Reflect
{
    /// <summary>
    /// ?瑕??亙蝔???
    /// </summary>
    /// <returns></returns>
    internal static Assembly GetEntryAssembly()
    {
        return Assembly.GetEntryAssembly();
    }

    /// <summary>
    /// ?寞蝔???蝘啗??銵蝔???
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    internal static Assembly GetAssembly(string assemblyName)
    {
        // ?蝸蝔???
        return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
    }

    /// <summary>
    /// ?寞頝臬??蝸蝔???
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static Assembly LoadAssembly(string path)
    {
        if (!File.Exists(path)) return default;
        return Assembly.LoadFrom(path);
    }

    /// <summary>
    /// ??瘚?頧賜?摨?
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static Assembly LoadAssembly(MemoryStream assembly)
    {
        return Assembly.Load(assembly.ToArray());
    }

    /// <summary>
    /// ?寞蝔???蝘啜掩???湧?摰??瑕?餈??嗥掩??
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(string assemblyName, string typeFullName)
    {
        return GetAssembly(assemblyName).GetType(typeFullName);
    }

    /// <summary>
    /// ?寞蝔???蝐餃?摰?????銵蝐餃?
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(Assembly assembly, string typeFullName)
    {
        return assembly.GetType(typeFullName);
    }

    /// <summary>
    /// ?寞蝔???蝐餃?摰?????銵蝐餃?
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="typeFullName"></param>
    /// <returns></returns>
    internal static Type GetType(MemoryStream assembly, string typeFullName)
    {
        return LoadAssembly(assembly).GetType(typeFullName);
    }

    /// <summary>
    /// ?瑕?蝔???蝘?
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(Assembly assembly)
    {
        return assembly.GetName().Name;
    }

    /// <summary>
    /// ?瑕?蝔???蝘?
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(Type type)
    {
        return GetAssemblyName(type.GetTypeInfo());
    }

    /// <summary>
    /// ?瑕?蝔???蝘?
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    internal static string GetAssemblyName(TypeInfo typeInfo)
    {
        return GetAssemblyName(typeInfo.Assembly);
    }

    /// <summary>
    /// ?蝸蝔??掩???舀??澆?嚗?摨?;蝵?蝐餃??賢?蝛粹
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    internal static Type GetStringType(string str)
    {
        var typeDefinitions = str.Split(';');
        return GetType(typeDefinitions[0], typeDefinitions[1]);
    }
}
