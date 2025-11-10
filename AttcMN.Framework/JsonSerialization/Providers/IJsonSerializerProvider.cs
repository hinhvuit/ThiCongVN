namespace AttcMN.Framework.JsonSerialization;

/// <summary>
/// Json 摨???靘
/// </summary>
public interface IJsonSerializerProvider
{
    /// <summary>
    /// 摨??笆鞊?
    /// </summary>
    /// <param name="value"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    string Serialize(object value, object jsonSerializerOptions = default);

    /// <summary>
    /// ????摮泵銝?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    T Deserialize<T>(string json, object jsonSerializerOptions = default);

    /// <summary>
    /// ????摮泵銝?
    /// </summary>
    /// <param name="json"></param>
    /// <param name="returnType"></param>
    /// <param name="jsonSerializerOptions"></param>
    /// <returns></returns>
    object Deserialize(string json, Type returnType, object jsonSerializerOptions = default);

    /// <summary>
    /// 餈?霂餃??典??蔭??JSON ?★
    /// </summary>
    /// <returns></returns>
    object GetSerializerOptions();
}
