using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AttcMN.Framework.SpecificationDocument;

/// <summary>
/// 靽格迤 閫???獢?object schema嚗?銝?曄內銝?any
/// </summary>
/// <remarks>?詨 issue嚗ttps://github.com/swagger-api/swagger-codegen-generators/issues/692 </remarks>
[SuppressSniffer]
public class AnySchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 摰餈誘?冽瘜?
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        var type = context.Type;

        if (type == typeof(object))
        {
            model.AdditionalPropertiesAllowed = false;
        }
    }
}
