using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using AttcMN.Framework.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AttcMN.Framework.SpecificationDocument;

/// <summary>
/// 靽格迤 閫???獢?Enum ?內
/// </summary>
[SuppressSniffer]
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 銝剜?甇??銵刻噢撘?
    /// </summary>
    private const string CHINESE_PATTERN = @"[\u4e00-\u9fa5]";

    /// <summary>
    /// 摰餈誘?冽瘜?
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        var type = context.Type;

        // ??嗡?蝔????蜀
        if (type.IsEnum && App.Assemblies.Contains(type.Assembly))
        {
            model.Enum.Clear();
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{model.Description}<br />");

            var enumValues = Enum.GetValues(type);

            bool convertToNumber;
            // 摰? [EnumToNumber] ?寞扳???
            if (type.IsDefined(typeof(EnumToNumberAttribute), false))
            {
                var enumToNumberAttribute = type.GetCustomAttribute<EnumToNumberAttribute>(false);
                convertToNumber = enumToNumberAttribute.Enabled;
            }
            else
            {
                convertToNumber = App.Configuration.GetValue("SpecificationDocumentSettings:EnumToNumber", false);
            }

            // ?銝剜??
            if (Enum.GetNames(type).Any(v => Regex.IsMatch(v, CHINESE_PATTERN)))
            {
                convertToNumber = true;
            }

            // ?瑕??蜀摰??潛掩??
            var enumValueType = type.GetField("value__").FieldType;

            foreach (var value in enumValues)
            {
                var numValue = value.ChangeType(enumValueType);

                // ?瑕??蜀???寞?
                var fieldinfo = type.GetField(Enum.GetName(type, value));
                var descriptionAttribute = fieldinfo.GetCustomAttribute<DescriptionAttribute>(true);
                model.Enum.Add(!convertToNumber
                    ? new OpenApiString(value.ToString())
                    : OpenApiAnyFactory.CreateFromJson($"{numValue}"));

                stringBuilder.Append($"&nbsp;{descriptionAttribute?.Description} {value} = {numValue}<br />");
            }
            model.Description = stringBuilder.ToString();

            if (!convertToNumber)
            {
                model.Type = "string";
                model.Format = null;
            }
        }
    }
}
