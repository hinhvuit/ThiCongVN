using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using AttcMN.Framework.ConfigurableOptions;
using AttcMN.Framework.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AttcMN.Framework.SpecificationDocument;

/// <summary>
/// 閫???獢??蝵桅★
/// </summary>
public sealed class SpecificationDocumentSettingsOptions : IConfigurableOptions<SpecificationDocumentSettingsOptions>
{
    /// <summary>
    /// ?﹝??
    /// </summary>
    public string DocumentTitle { get; set; }

    /// <summary>
    /// 暺恕????
    /// </summary>
    public string DefaultGroupName { get; set; }

    /// <summary>
    /// ?舐???舀?
    /// </summary>
    public bool? EnableAuthorized { get; set; }

    /// <summary>
    /// ?澆??蛹V2?
    /// </summary>
    public bool? FormatAsV2 { get; set; }

    /// <summary>
    /// ?蔭閫???獢??
    /// </summary>
    public string RoutePrefix { get; set; }

    /// <summary>
    /// ?﹝撅?霈曄蔭
    /// </summary>
    public DocExpansion? DocExpansionState { get; set; }

    /// <summary>
    /// XML ?膩?辣
    /// </summary>
    public string[] XmlComments { get; set; }

    /// <summary>
    /// ??靽⊥
    /// </summary>
    public SpecificationOpenApiInfo[] GroupOpenApiInfos { get; set; }

    /// <summary>
    /// 摰摰?
    /// </summary>
    public SpecificationOpenApiSecurityScheme[] SecurityDefinitions { get; set; }

    /// <summary>
    /// ?蔭 Servers
    /// </summary>
    public OpenApiServer[] Servers { get; set; }

    /// <summary>
    /// ?? Servers
    /// </summary>
    public bool? HideServers { get; set; }

    /// <summary>
    /// 暺恕 swagger.json 頝舐璅⊥
    /// </summary>
    public string RouteTemplate { get; set; }

    /// <summary>
    /// ?蔭摰?蝚砌??孵???蝏?
    /// </summary>
    public string[] PackagesGroups { get; set; }

    /// <summary>
    /// ?舐?蜀 Schema 蝑
    /// </summary>
    public bool? EnableEnumSchemaFilter { get; set; }

    /// <summary>
    /// ?舐?倌??蝑
    /// </summary>
    public bool? EnableTagsOrderDocumentFilter { get; set; }

    /// <summary>
    /// ??桀?嚗耨甇?IIS ?遣 Application ?桅?嚗?
    /// </summary>
    public string ServerDir { get; set; }

    /// <summary>
    /// ?蔭閫???獢?敶縑??
    /// </summary>
    public SpecificationLoginInfo LoginInfo { get; set; }

    /// <summary>
    /// ?舐 All Groups ?
    /// </summary>
    public bool? EnableAllGroups { get; set; }

    /// <summary>
    /// ?蜀蝐餃????潛掩??
    /// </summary>
    public bool? EnumToNumber { get; set; }

    /// <summary>
    /// ???蔭
    /// </summary>
    /// <param name="options"></param>
    /// <param name="configuration"></param>
    public void PostConfigure(SpecificationDocumentSettingsOptions options, IConfiguration configuration)
    {
        options.DocumentTitle ??= "Specification Api Document";
        options.DefaultGroupName ??= "Default";
        options.FormatAsV2 ??= false;
        //options.RoutePrefix ??= "api";    // ?臭誑?? UseInject() ?蔭嚗?隞交釣??
        options.DocExpansionState ??= DocExpansion.List;

        // ?蝸憿寧瘜典??芋??/?辣瘜券?
        var frameworkPackageName = Reflect.GetAssemblyName(GetType());
        var projectXmlComments = App.Assemblies.Where(u => u.GetName().Name != frameworkPackageName).Select(t => t.GetName().Name);
        var externalXmlComments = App.ExternalAssemblies.Any() ? App.Settings.ExternalAssemblies.Select(u => u.EndsWith(".dll") ? u[0..^4] : u) : Array.Empty<string>();
        XmlComments ??= projectXmlComments.Concat(externalXmlComments).ToArray();

        GroupOpenApiInfos ??= new SpecificationOpenApiInfo[]
        {
                new SpecificationOpenApiInfo()
                {
                    Group=options.DefaultGroupName
                }
        };

        EnableAuthorized ??= true;
        if (EnableAuthorized == true)
        {
            SecurityDefinitions ??= new SpecificationOpenApiSecurityScheme[]
            {
                    new SpecificationOpenApiSecurityScheme
                    {
                        Id="Bearer",
                        Type= SecuritySchemeType.Http,
                        Name="Authorization",
                        Description="JWT Authorization header using the Bearer scheme.",
                        BearerFormat="JWT",
                        Scheme="bearer",
                        In= ParameterLocation.Header,
                        Requirement=new SpecificationOpenApiSecurityRequirementItem
                        {
                            Scheme=new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference
                                {
                                    Id="Bearer",
                                    Type= ReferenceType.SecurityScheme
                                }
                            },
                            Accesses=Array.Empty<string>()
                        }
                    }
            };
        }

        Servers ??= Array.Empty<OpenApiServer>();
        HideServers ??= true;
        RouteTemplate ??= "swagger/{documentName}/swagger.json";
        PackagesGroups ??= Array.Empty<string>();
        EnableEnumSchemaFilter ??= true;
        EnableTagsOrderDocumentFilter ??= true;
        EnableAllGroups ??= false;
        EnumToNumber ??= false;
    }
}
