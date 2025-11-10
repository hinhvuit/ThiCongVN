using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AttcMN.Framework.JsonSerialization;

/// <summary>
/// System.Text.Json 摨???靘嚗?霈文??堆?
/// </summary>
[Injection(Order = -999)]
public class SystemTextJsonSerializerProvider : IJsonSerializerProvider, ISingleton
{
    /// <summary>
    /// ?瑕? JSON ?蔭?★
    /// </summary>
    private readonly JsonOptions _jsonOptions;

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="options"></param>
    public SystemTextJsonSerializerProvider(IOptions<JsonOptions> options)
    {
        _jsonOptions = options.Value;
    }

    /// <summary>
    /// 摨??笆鞊?
    /// </summary>
    /// <param name="value"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    public string Serialize(object value, object jsonSerializerOptions = null)
    {
        return JsonSerializer.Serialize(value, (jsonSerializerOptions ?? GetSerializerOptions()) as JsonSerializerOptions);
    }

    /// <summary>
    /// ????摮泵銝?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    public T Deserialize<T>(string json, object jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize<T>(json, (jsonSerializerOptions ?? GetSerializerOptions()) as JsonSerializerOptions);
    }

    /// <summary>
    /// ????摮泵銝?
    /// </summary>
    /// <param name="json"></param>
    /// <param name="returnType"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    public object Deserialize(string json, Type returnType, object jsonSerializerOptions = null)
    {
        return JsonSerializer.Deserialize(json, returnType, (jsonSerializerOptions ?? GetSerializerOptions()) as JsonSerializerOptions);
    }

    /// <summary>
    /// 餈?霂餃??典??蔭??JSON ?★
    /// </summary>
    /// <returns></returns>
    public object GetSerializerOptions()
    {
        return _jsonOptions?.JsonSerializerOptions;
    }
}
