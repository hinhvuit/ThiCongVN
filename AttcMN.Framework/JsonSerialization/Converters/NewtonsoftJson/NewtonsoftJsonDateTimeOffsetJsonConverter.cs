using Newtonsoft.Json;
using AttcMN.Framework.Extensions;

namespace AttcMN.Framework.JsonSerialization;

/// <summary>
/// DateTimeOffset 蝐餃?摨???
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// ???
    /// </summary>
    public NewtonsoftJsonDateTimeOffsetJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonDateTimeOffsetJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="format"></param>
    /// <param name="outputToLocalDateTime"></param>
    public NewtonsoftJsonDateTimeOffsetJsonConverter(string format = "yyyy-MM-dd HH:mm:ss", bool outputToLocalDateTime = false)
        : this(format)
    {
        Localized = outputToLocalDateTime;
    }

    /// <summary>
    /// ?園?澆??撘?
    /// </summary>
    public string Format { get; private set; }

    /// <summary>
    /// ?臬颲銝箔蛹敶?園
    /// </summary>
    public bool Localized { get; private set; } = false;

    /// <summary>
    /// ????
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="hasExistingValue"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return DateTime.SpecifyKind(Penetrates.ConvertToDateTime(ref reader), Localized ? DateTimeKind.Local : DateTimeKind.Utc);
    }

    /// <summary>
    /// 摨???
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
    {
        // ?斗?臬摨???敶?園
        var formatDateTime = Localized ? value.ConvertToDateTime() : value;
        serializer.Serialize(writer, formatDateTime.ToString(Format));
    }
}

/// <summary>
/// DateTimeOffset 蝐餃?摨???
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonNullableDateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset?>
{
    /// <summary>
    /// ???
    /// </summary>
    public NewtonsoftJsonNullableDateTimeOffsetJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonNullableDateTimeOffsetJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="format"></param>
    /// <param name="outputToLocalDateTime"></param>
    public NewtonsoftJsonNullableDateTimeOffsetJsonConverter(string format = "yyyy-MM-dd HH:mm:ss", bool outputToLocalDateTime = false)
        : this(format)
    {
        Localized = outputToLocalDateTime;
    }

    /// <summary>
    /// ?園?澆??撘?
    /// </summary>
    public string Format { get; private set; }

    /// <summary>
    /// ?臬颲銝箔蛹敶?園
    /// </summary>
    public bool Localized { get; private set; } = false;

    /// <summary>
    /// ????
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="hasExistingValue"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return DateTime.SpecifyKind(Penetrates.ConvertToDateTime(ref reader), Localized ? DateTimeKind.Local : DateTimeKind.Utc);
    }

    /// <summary>
    /// 摨???
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
    {
        if (value == null) writer.WriteNull();
        else
        {
            // ?斗?臬摨???敶?園
            var formatDateTime = Localized ? value.ConvertToDateTime() : value;
            serializer.Serialize(writer, formatDateTime.Value.ToString(Format));
        }
    }
}
