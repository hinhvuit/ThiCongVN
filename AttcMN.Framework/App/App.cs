using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using AttcMN.Framework.ConfigurableOptions;
using AttcMN.Framework.Reflection;
using AttcMN.Framework.Templates;
using StackExchange.Profiling;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace AttcMN.Framework;

/// <summary>
/// ?典?摨蝐?
/// </summary>
[SuppressSniffer]
public static class App
{
    /// <summary>
    /// 蝘?霈曄蔭嚗??憭圾??
    /// </summary>
    internal static AppSettingsOptions _settings;

    /// <summary>
    /// 摨?典??蔭
    /// </summary>
    public static AppSettingsOptions Settings => _settings ??= GetConfig<AppSettingsOptions>("AppSettings", true);

    /// <summary>
    /// ?典??蔭?★
    /// </summary>
    public static IConfiguration Configuration => CatchOrDefault(() => InternalApp.Configuration.Reload(), new ConfigurationBuilder().Build());

    /// <summary>
    /// ?瑕?Web銝餅?臬?嚗?嚗?行撘?憓??漣?臬?蝑?
    /// </summary>
    public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;

    /// <summary>
    /// ?瑕?瘜?銝餅?臬?嚗?嚗?行撘?憓??漣?臬?蝑?
    /// </summary>
    public static IHostEnvironment HostEnvironment => InternalApp.HostEnvironment;

    /// <summary>
    /// 摮?寞??∴??航銝箇征
    /// </summary>
    public static IServiceProvider RootServices => InternalApp.RootServices;

    /// <summary>
    /// ?斗?臬?臬??辣?臬?
    /// </summary>
    public static bool SingleFileEnvironment => string.IsNullOrWhiteSpace(Assembly.GetEntryAssembly().Location);

    /// <summary>
    /// 摨??蝔???
    /// </summary>
    public static readonly IEnumerable<Assembly> Assemblies;

    /// <summary>
    /// ??蝔??掩??
    /// </summary>
    public static readonly IEnumerable<Type> EffectiveTypes;

    /// <summary>
    /// ?瑕?霂瑟?銝???
    /// </summary>
    public static HttpContext HttpContext => CatchOrDefault(() => RootServices?.GetService<IHttpContextAccessor>()?.HttpContext);

    /// <summary>
    /// ?瑕?霂瑟?銝????
    /// </summary>
    /// <remarks>?芣???霈輸?△?Ｘ??亙???典潘??血?銝?null</remarks>
    public static ClaimsPrincipal User => HttpContext?.User;

    /// <summary>
    /// ?芣?蝞∠?撖寡情??
    /// </summary>
    public static readonly ConcurrentBag<IDisposable> UnmanagedObjects;

    /// <summary>
    /// 閫???????
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static IServiceProvider GetServiceProvider(Type serviceType)
    {
        // 憭??批?啣??函?摨?
        if (HostEnvironment == default) return RootServices;

        // 蝚砌??嚗?剜?行??瘜典?銝?靘??∩?銝箇征嚗???湔餈??寞??⊥?靘
        if (RootServices != null && InternalApp.InternalServices.Where(u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType))
                                                                .Any(u => u.Lifetime == ServiceLifetime.Singleton)) return RootServices;

        // 蝚砌???航??HttpContext 撖寡情??RequestServices
        var httpContext = HttpContext;
        if (httpContext?.RequestServices != null) return httpContext.RequestServices;
        // 蝚砌??嚗?撱箸???典?撟嗉????⊥?靘
        else if (RootServices != null)
        {
            var scoped = RootServices.CreateScope();
            UnmanagedObjects.Add(scoped);
            return scoped.ServiceProvider;
        }
        // 蝚砍??嚗?撱箸???∪笆鞊∴??扯?撌殷?
        else
        {
            var serviceProvider = InternalApp.InternalServices.BuildServiceProvider();
            UnmanagedObjects.Add(serviceProvider);
            return serviceProvider;
        }
    }

    /// <summary>
    /// ?瑕?霂瑟????冽?????
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TService GetService<TService>(IServiceProvider serviceProvider = default)
        where TService : class
    {
        return GetService(typeof(TService), serviceProvider) as TService;
    }

    /// <summary>
    /// ?瑕?霂瑟????冽?????
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static object GetService(Type type, IServiceProvider serviceProvider = default)
    {
        return (serviceProvider ?? GetServiceProvider(type)).GetService(type);
    }

    /// <summary>
    /// ?瑕?霂瑟????冽????⊿???
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static IEnumerable<TService> GetServices<TService>(IServiceProvider serviceProvider = default)
        where TService : class
    {
        return (serviceProvider ?? GetServiceProvider(typeof(TService))).GetServices<TService>();
    }

    /// <summary>
    /// ?瑕?霂瑟????冽????⊿???
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static IEnumerable<object> GetServices(Type type, IServiceProvider serviceProvider = default)
    {
        return (serviceProvider ?? GetServiceProvider(type)).GetServices(type);
    }

    /// <summary>
    /// ?瑕?霂瑟????冽?????
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static TService GetRequiredService<TService>(IServiceProvider serviceProvider = default)
        where TService : class
    {
        return GetRequiredService(typeof(TService), serviceProvider) as TService;
    }

    /// <summary>
    /// ?瑕?霂瑟????冽?????
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public static object GetRequiredService(Type type, IServiceProvider serviceProvider = default)
    {
        return (serviceProvider ?? GetServiceProvider(type)).GetRequiredService(type);
    }

    /// <summary>
    /// ?瑕??蔭
    /// </summary>
    /// <typeparam name="TOptions">撘箇掩?★蝐?/typeparam>
    /// <param name="path">?蔭銝剖笆摨?Key</param>
    /// <param name="loadPostConfigure"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetConfig<TOptions>(string path, bool loadPostConfigure = false)
    {
        var options = Configuration.GetSection(path).Get<TOptions>();

        // ?蝸暺恕?★?蔭
        if (loadPostConfigure && typeof(IConfigurableOptions).IsAssignableFrom(typeof(TOptions)))
        {
            var postConfigure = typeof(TOptions).GetMethod("PostConfigure");
            if (postConfigure != null)
            {
                options ??= Activator.CreateInstance<TOptions>();
                postConfigure.Invoke(options, new object[] { options, Configuration });
            }
        }

        return options;
    }

    /// <summary>
    /// ?瑕??★
    /// </summary>
    /// <typeparam name="TOptions">撘箇掩?★蝐?/typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptions<TOptions>(IServiceProvider serviceProvider = default)
        where TOptions : class, new()
    {
        return Penetrates.GetOptionsOnStarting<TOptions>()
            ?? GetService<IOptions<TOptions>>(serviceProvider ?? RootServices)?.Value;
    }

    /// <summary>
    /// ?瑕??★
    /// </summary>
    /// <typeparam name="TOptions">撘箇掩?★蝐?/typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptionsMonitor<TOptions>(IServiceProvider serviceProvider = default)
        where TOptions : class, new()
    {
        return Penetrates.GetOptionsOnStarting<TOptions>()
            ?? GetService<IOptionsMonitor<TOptions>>(serviceProvider ?? RootServices)?.CurrentValue;
    }

    /// <summary>
    /// ?瑕??★
    /// </summary>
    /// <typeparam name="TOptions">撘箇掩?★蝐?/typeparam>
    /// <param name="serviceProvider"></param>
    /// <returns>TOptions</returns>
    public static TOptions GetOptionsSnapshot<TOptions>(IServiceProvider serviceProvider = default)
        where TOptions : class, new()
    {
        // 餈?銝隞?閫??嚗?銝箸 Scoped 雿??
        return Penetrates.GetOptionsOnStarting<TOptions>()
            ?? GetService<IOptionsSnapshot<TOptions>>(serviceProvider)?.Value;
    }

    /// <summary>
    /// ?瑕??賭誘銵?蝵?
    /// </summary>
    /// <param name="args"></param>
    /// <param name="switchMappings"></param>
    /// <returns></returns>
    public static CommandLineConfigurationProvider GetCommandLineConfiguration(string[] args, IDictionary<string, string> switchMappings = null)
    {
        var commandLineConfiguration = new CommandLineConfigurationProvider(args, switchMappings);
        commandLineConfiguration.Load();

        return commandLineConfiguration;
    }

    /// <summary>
    /// ?瑕?敶?蝥輻? Id
    /// </summary>
    /// <returns></returns>
    public static int GetThreadId()
    {
        return Environment.CurrentManagedThreadId;
    }

    /// <summary>
    /// ?瑕?敶?霂瑟? TraceId
    /// </summary>
    /// <returns></returns>
    public static string GetTraceId()
    {
        return Activity.Current?.Id ?? (InternalApp.RootServices == null ? default : HttpContext?.TraceIdentifier);
    }

    /// <summary>
    /// ?瑕?銝畾萎誨?銵
    /// </summary>
    /// <param name="action">憪?</param>
    /// <returns><see cref="long"/></returns>
    public static long GetExecutionTime(Action action)
    {
        // 蝛箸???
        if (action == null) throw new ArgumentNullException(nameof(action));

        // 霈∠??亙?扯??園
        var timeOperation = Stopwatch.StartNew();
        action();
        timeOperation.Stop();
        return timeOperation.ElapsedMilliseconds;
    }

    /// <summary>
    /// ?瑕??瘜典????賢?掩??
    /// </summary>
    /// <param name="serviceType"></param>
    /// <returns></returns>
    public static ServiceLifetime? GetServiceLifetime(Type serviceType)
    {
        var serviceDescriptor = InternalApp.InternalServices
            .FirstOrDefault(u => u.ServiceType == (serviceType.IsGenericType ? serviceType.GetGenericTypeDefinition() : serviceType));

        return serviceDescriptor?.Lifetime;
    }

    /// <summary>
    /// 蝻? C# 蝐餃?銋誨????摨?
    /// </summary>
    /// <param name="csharpCode">摮泵銝脖誨??/param>
    /// <param name="assemblyName">?芸?銋?摨??妍</param>
    /// <param name="additionalAssemblies">????摨?</param>
    /// <returns><see cref="Assembly"/></returns>
    public static Assembly CompileCSharpClassCode(string csharpCode, string assemblyName = default, params Assembly[] additionalAssemblies)
    {
        // 蝻?隞??
        using var memoryStream = CompileCSharpClassCodeToStream(csharpCode, assemblyName, additionalAssemblies);

        // 餈?蝻?蝔???
        return Assembly.Load(memoryStream.ToArray());
    }

    /// <summary>
    /// 蝻? C# 蝐餃?銋誨??摮蛹 dll ?辣
    /// </summary>
    /// <param name="csharpCode">摮泵銝脖誨??/param>
    /// <param name="assemblyName">?芸?銋?摨??妍</param>
    /// <param name="additionalAssemblies">????摨?</param>
    /// <returns><see cref="Assembly"/></returns>
    public static Assembly CompileCSharpClassCodeToDllFile(string csharpCode, string assemblyName = default, params Assembly[] additionalAssemblies)
    {
        var assName = string.IsNullOrWhiteSpace(assemblyName) ? Path.GetRandomFileName() : assemblyName.Trim();

        // 蝻?隞??
        using var memoryStream = CompileCSharpClassCodeToStream(csharpCode, assName, additionalAssemblies);

        // 靽???dll ?辣
        using var fileStream = new FileStream(
            path: Path.Combine(AppContext.BaseDirectory, $"{assName}.dll"),
            mode: FileMode.OpenOrCreate,
            access: FileAccess.Write,
            share: FileShare.None,
            bufferSize: 8192,
            useAsync: true);

        memoryStream.CopyTo(fileStream);

        // 餈?蝻?蝔???
        return Assembly.Load(memoryStream.ToArray());
    }

    /// <summary>
    /// 蝻? C# 蝐餃?銋誨????摮?
    /// </summary>
    /// <param name="csharpCode">摮泵銝脖誨??/param>
    /// <param name="assemblyName">?芸?銋?摨??妍</param>
    /// <param name="additionalAssemblies">????摨?</param>
    /// <returns><see cref="Assembly"/></returns>
    public static MemoryStream CompileCSharpClassCodeToStream(string csharpCode, string assemblyName = default, params Assembly[] additionalAssemblies)
    {
        // 蝛箸???
        if (csharpCode == null) throw new ArgumentNullException(nameof(csharpCode));

        // ?僎蝔???
        var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var references = assemblyName != null && additionalAssemblies.Length > 0
            ? domainAssemblies.Concat(additionalAssemblies)
            : domainAssemblies;

        // ??霂剜???
        var syntaxTree = CSharpSyntaxTree.ParseText(csharpCode);

        // ?遣 C# 蝻???
        var compilation = CSharpCompilation.Create(
          string.IsNullOrWhiteSpace(assemblyName) ? Path.GetRandomFileName() : assemblyName.Trim(),
          new[]
          {
                    syntaxTree
          },
          references.Where(ass =>
          {
              unsafe
              {
                  return ass.TryGetRawMetadata(out var blob, out var length);
              }
          }).Select(ass =>
          {
              unsafe
              {
                  ass.TryGetRawMetadata(out var blob, out var length);
                  var moduleMetadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, length);
                  var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                  var metadataReference = assemblyMetadata.GetReference();
                  return metadataReference;
              }
          }),
          new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // 蝻?隞??
        var memoryStream = new MemoryStream();
        var emitResult = compilation.Emit(memoryStream);

        // 蝻?憭梯揖?撘虜
        if (!emitResult.Success)
        {
            throw new InvalidOperationException($"Unable to compile class code: {string.Join("\n", emitResult.Diagnostics.ToList().Where(w => w.IsWarningAsError || w.Severity == DiagnosticSeverity.Error))}");
        }

        memoryStream.Position = 0;

        return memoryStream;
    }

    /// <summary>
    /// ?撉?靽⊥??MiniProfiler
    /// </summary>
    /// <param name="category">?掩</param>
    /// <param name="state">?嗆?/param>
    /// <param name="message">瘨</param>
    /// <param name="isError">?臬銝箄郎????/param>
    public static void PrintToMiniProfiler(string category, string state, string message = null, bool isError = false)
    {
        if (!CanBeMiniProfiler()) return;

        // ?瘨
        var titleCaseCategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
        var customTiming = MiniProfiler.Current?.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseCategory} {state}" : message, state);
        if (customTiming == null) return;

        // ?斗?臬?航郎????
        if (isError) customTiming.Errored = true;
    }

    /// <summary>
    /// ???
    /// </summary>
    static App()
    {
        // ?芣?蝞∠?撖寡情
        UnmanagedObjects = new ConcurrentBag<IDisposable>();

        // ?蝸蝔???
        var assObject = GetAssemblies();
        Assemblies = assObject.Assemblies;
        ExternalAssemblies = assObject.ExternalAssemblies;

        // ?瑕????掩????
        EffectiveTypes = Assemblies.SelectMany(GetTypes);

        AppStartups = new ConcurrentBag<AppStartup>();
    }

    /// <summary>
    /// 摨???券?蝵桀笆鞊?
    /// </summary>
    internal static ConcurrentBag<AppStartup> AppStartups;

    /// <summary>
    /// 憭蝔???
    /// </summary>
    internal static IEnumerable<Assembly> ExternalAssemblies;

    /// <summary>
    /// ?瑕?摨??蝔???
    /// </summary>
    /// <returns>IEnumerable</returns>
    private static (IEnumerable<Assembly> Assemblies, IEnumerable<Assembly> ExternalAssemblies) GetAssemblies()
    {
        // ????摨???
        var excludeAssemblyNames = new string[] {
                "Database.Migrations"
            };

        // 霂餃?摨?蔭
        var supportPackageNamePrefixs = Settings.SupportPackageNamePrefixs ?? Array.Empty<string>();

        IEnumerable<Assembly> scanAssemblies;

        // ?瑕??亙蝔???
        var entryAssembly = Assembly.GetEntryAssembly();

        // ?蝡?撣????辣??
        if (!string.IsNullOrWhiteSpace(entryAssembly.Location))
        {
            var dependencyContext = DependencyContext.Default;

            // 霂餃?憿寧蝔??????冽溶???函?dll嚗??蔭?孵?????
            scanAssemblies = dependencyContext.RuntimeLibraries
               .Where(u =>
                      (u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j))) ||
                      (u.Type == "package" && supportPackageNamePrefixs.Any(p => u.Name.StartsWith(p))) ||
                      (Settings.EnabledReferenceAssemblyScan == true && u.Type == "reference"))    // ?斗?臬?舐撘蝔????
               .Select(u => Reflect.GetAssembly(u.Name));
        }
        // ?祉???/??隞嗅?撣?
        else
        {
            IEnumerable<Assembly> fixedSingleFileAssemblies = new[] { entryAssembly };

            // ?急?摰 ISingleFilePublish ?亙?掩??
            var singleFilePublishType = entryAssembly.GetTypes()
                                                .FirstOrDefault(u => u.IsClass && !u.IsInterface && !u.IsAbstract && typeof(ISingleFilePublish).IsAssignableFrom(u));
            if (singleFilePublishType != null)
            {
                var singleFilePublish = Activator.CreateInstance(singleFilePublishType) as ISingleFilePublish;

                // ?蝸?冽?芸?銋?蝵桀??辣??蝔???
                var nativeAssemblies = singleFilePublish.IncludeAssemblies();
                var loadAssemblies = singleFilePublish.IncludeAssemblyNames()
                                                .Select(u => Reflect.GetAssembly(u));

                fixedSingleFileAssemblies = fixedSingleFileAssemblies.Concat(nativeAssemblies)
                                                            .Concat(loadAssemblies);
            }
            else
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                // ?內瘝⊥?甇?＆?蔭??隞園?蝵?
                Console.WriteLine(TP.Wrapper("Deploy Console"
                    , "Single file deploy error."
                    , "##Exception## Single file deployment configuration error."));
                Console.ResetColor();
            }

            // ?? AppDomain.CurrentDomain ?急?嚗?霈支蛹撱嗉??蝸嚗迤撣詨?賣??亙蝔????臬撅?
            scanAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                    .Where(ass =>
                                            // ? System嚗icrosoft嚗etstandard 撘憭渡?蝔???
                                            !ass.FullName!.StartsWith(nameof(System))
                                            && !ass.FullName.StartsWith(nameof(Microsoft))
                                            && !ass.FullName.StartsWith("netstandard"))
                                    .Concat(fixedSingleFileAssemblies)
                                    .Distinct();
        }

        IEnumerable<Assembly> externalAssemblies = Array.Empty<Assembly>();

        // ?蝸 `appsetting.json` ?蔭???函?摨?
        if (Settings.ExternalAssemblies != null && Settings.ExternalAssemblies.Any())
        {
            foreach (var externalAssembly in Settings.ExternalAssemblies)
            {
                // ?蝸憭蝔???
                var assemblyFileFullPath = Path.Combine(AppContext.BaseDirectory
                    , externalAssembly.EndsWith(".dll") ? externalAssembly : $"{externalAssembly}.dll");

                // ?寞頝臬??蝸蝔???
                var loadedAssembly = Reflect.LoadAssembly(assemblyFileFullPath);
                if (loadedAssembly == default) continue;
                var assembly = new[] { loadedAssembly };

                // ?僎蝔???
                scanAssemblies = scanAssemblies.Concat(assembly);
                externalAssemblies = externalAssemblies.Concat(assembly);
            }
        }

        // 憭????摨?
        if (Settings.ExcludeAssemblies != null && Settings.ExcludeAssemblies.Any())
        {
            scanAssemblies = scanAssemblies.Where(ass => !Settings.ExcludeAssemblies.Contains(ass.GetName().Name, StringComparer.OrdinalIgnoreCase));
        }

        return (scanAssemblies, externalAssemblies);
    }

    /// <summary>
    /// ?蝸蝔??葉???掩??
    /// </summary>
    /// <param name="ass"></param>
    /// <returns></returns>
    private static IEnumerable<Type> GetTypes(Assembly ass)
    {
        var types = Array.Empty<Type>();

        try
        {
            types = ass.GetTypes();
        }
        catch
        {
            Console.WriteLine($"Error load `{ass.FullName}` assembly.");
        }

        return types.Where(u => u.IsPublic && !u.IsDefined(typeof(SuppressSnifferAttribute), false));
    }

    /// <summary>
    /// ?斗?臬?舐 MiniProfiler
    /// </summary>
    /// <returns></returns>
    internal static bool CanBeMiniProfiler()
    {
        // ??銝?閬??
        if (Settings.InjectMiniProfiler != true || HttpContext == null
            || !(HttpContext.Request.Headers.TryGetValue("request-from", out var value) && value == "swagger")) return false;

        return true;
    }

    /// <summary>
    /// GC ?暺恕?湧?
    /// </summary>
    private const int GC_COLLECT_INTERVAL_SECONDS = 5;

    /// <summary>
    /// 霈啣??餈?GC ??園
    /// </summary>
    private static DateTime? LastGCCollectTime { get; set; }

    /// <summary>
    /// ????恣?笆鞊?
    /// </summary>
    public static void DisposeUnmanagedObjects()
    {
        foreach (var dsp in UnmanagedObjects)
        {
            try
            {
                dsp?.Dispose();
            }
            finally { }
        }

        // 撘箏?? GC ??
        if (UnmanagedObjects.Any())
        {
            var nowTime = DateTime.UtcNow;
            if ((LastGCCollectTime == null || (nowTime - LastGCCollectTime.Value).TotalSeconds > GC_COLLECT_INTERVAL_SECONDS))
            {
                LastGCCollectTime = nowTime;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        UnmanagedObjects.Clear();
    }

    /// <summary>
    /// 憭??瑕?撖寡情撘虜?桅?
    /// </summary>
    /// <typeparam name="T">蝐餃?</typeparam>
    /// <param name="action">?瑕?撖寡情憪?</param>
    /// <param name="defaultValue">暺恕??/param>
    /// <returns>T</returns>
    private static T CatchOrDefault<T>(Func<T> action, T defaultValue = null)
        where T : class
    {
        try
        {
            return action();
        }
        catch
        {
            return defaultValue ?? null;
        }
    }
}
