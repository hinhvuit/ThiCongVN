using AttcMN.Framework.DataValidation;
using AttcMN.Framework.Exceptions;
using AttcMN.Framework.Extensions;
using AttcMN.Framework.JsonSerialization;
using AttcMN.Framework.Templates.Extensions;
using AttcMN.Framework.VirtualFileServer;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace AttcMN.Framework.RemoteRequest;

/// <summary>
/// HttpClient 撖寡情蝏??其辣
/// </summary>
public sealed partial class HttpRequestPart
{
    /// <summary>
    /// 霂瑟?憭梯揖鈭辣
    /// </summary>
    public event EventHandler<HttpRequestFaildedEventArgs> OnRequestFailded;

    /// <summary>
    /// MiniProfiler ?掩??
    /// </summary>
    private const string MiniProfilerCategory = "httpclient";

    /// <summary>
    /// ??GET 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> GetAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??GET 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> GetAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??GET 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task GetToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??GET 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??GET 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> GetAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??GET 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Get).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> PostAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> PostAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PostToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> PostAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> PostAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??POST 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> PostAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Post).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> PutAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> PutAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PutToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> PutAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> PutAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??PUT 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> PutAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Put).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> DeleteAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> DeleteAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task DeleteToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> DeleteAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> DeleteAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??DELETE 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> DeleteAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Delete).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ??PATCH 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> PatchAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??PATCH 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> PatchAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??PATCH 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task PatchToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??Patch 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> PatchAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??Patch 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> PatchAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??PATCH 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> PatchAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Patch).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ??HEAD 霂瑟?餈? T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> HeadAsAsync<T>(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendAsAsync<T>(cancellationToken);
    }

    /// <summary>
    /// ??HEAD 霂瑟?餈? Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<(Stream Stream, Encoding Encoding)> HeadAsStreamAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// ??HEAD 霂瑟?撟嗅? Stream 靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HeadToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendToSaveAsync(path, cancellationToken);
    }

    /// <summary>
    /// ??Head 霂瑟?餈? String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> HeadAsStringAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// ??Head 霂瑟?餈? ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<byte[]> HeadAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// ??HEAD 霂瑟?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HttpResponseMessage> HeadAsync(CancellationToken cancellationToken = default)
    {
        return SetHttpMethod(HttpMethod.Head).SendAsync(cancellationToken);
    }

    /// <summary>
    /// ?窈瘙???T 撖寡情
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<T> SendAsAsync<T>(CancellationToken cancellationToken = default)
    {
        // 憒? T ??HttpResponseMessage 蝐餃?嚗?餈?
        if (typeof(T) == typeof(HttpResponseMessage))
        {
            var httpResponseMessage = await SendAsync(cancellationToken);
            return (T)(object)httpResponseMessage;
        }
        // 憭?摮泵銝脩掩??
        if (typeof(T) == typeof(string))
        {
            var str = await SendAsStringAsync(cancellationToken);
            return (T)(object)str;
        }
        // 憭? byte[] ?啁?
        if (typeof(T) == typeof(byte[]))
        {
            var byteArray = await SendAsByteArrayAsync(cancellationToken);
            return (T)(object)byteArray;
        }

        // 霂餃?瘚?摰?
        var (stream, encoding) = await SendAsStreamAsync(cancellationToken);
        if (stream == null) return default;

        // 憒? T ??Stream 蝐餃?嚗?餈?
        if (typeof(T) == typeof(Stream)) return (T)(object)stream;

        // ?斗?臬?舐 Gzip
        using var streamReader = new StreamReader(
            !SupportGZip
            ? stream
            : new GZipStream(stream, CompressionMode.Decompress), encoding);

        var text = await streamReader.ReadToEndAsync();
        // ?瘚?
        await stream.DisposeAsync();

        // 憒?摮泵銝脖蛹蝛綽?????霈文?
        if (string.IsNullOrWhiteSpace(text)) return default;

        // 閫?? Json 摨???靘
        var jsonSerializer = App.GetService(JsonSerializerProvider, RequestScoped ?? App.RootServices) as IJsonSerializerProvider;

        // ????瘚?
        var result = jsonSerializer.Deserialize<T>(text, JsonSerializerOptions);
        return result;
    }

    /// <summary>
    /// ?窈瘙???Stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(Stream Stream, Encoding Encoding)> SendAsStreamAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(cancellationToken);
        if (response == null || response.Content == null) return default;

        // ?瑕? charset 蝻?
        var encoding = GetCharsetEncoding(response);

        // 霂餃???瘚?
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        stream.Position = 0;

        return (stream, encoding);
    }

    /// <summary>
    /// ?窈瘙僎撠?靽??唳?啁???
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SendToSaveAsync(string path, CancellationToken cancellationToken = default)
    {
        var (stream, _) = await SendAsStreamAsync(cancellationToken);
        await stream.CopyToSaveAsync(path);
    }

    /// <summary>
    /// ?窈瘙???String
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> SendAsStringAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(cancellationToken);
        if (response == null || response.Content == null) return default;

        // ?瑕? charset 蝻?
        var encoding = GetCharsetEncoding(response);

        // 霂餃??捆摮?瘚?
        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);

        // ????蝻?閫??
        return encoding.GetString(content);
    }

    /// <summary>
    /// ?窈瘙???ByteArray
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<byte[]> SendAsByteArrayAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(cancellationToken);
        if (response == null || response.Content == null) return default;

        // 霂餃????交?
        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        return content;
    }

    /// <summary>
    /// ?窈瘙?
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken = default)
    {
        // 璉?交?阡?蝵桐?霂瑟??寞?
        if (Method == null) throw new NullReferenceException(nameof(Method));

        // ?遣摰Ｘ蝡航窈瘙極??
        var clientFactory = App.GetService<IHttpClientFactory>(RequestScoped ?? App.RootServices)
            ?? throw new InvalidOperationException("Please add `services.AddRemoteRequest()` in Startup.cs.");

        // ?遣 HttpClient 撖寡情嚗???摰?
        var clientName = ClientName ?? string.Empty;
        // 餈?銝?閬?using ?嚗ttps://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0#httpclient-and-lifetime-management
        // 暺恕???蛹銝文???銝文???隡?券???
        var httpClient = ClientProvider?.Invoke() ?? (
                               string.IsNullOrWhiteSpace(clientName)
                                ? clientFactory.CreateClient()
                                : clientFactory.CreateClient(clientName));

        // ?斗?賢?摰Ｘ蝡舀?阡?蝵桐? BaseAddress嚗?敹◆隞?/ 蝏偏
        var httpClientOriginalString = httpClient.BaseAddress?.OriginalString;
        if (!string.IsNullOrWhiteSpace(httpClientOriginalString) && !httpClientOriginalString.EndsWith("/"))
            throw new InvalidOperationException($"The `{ClientName}` of HttpClient BaseAddress must be end with '/'.");

        // 瘛餃?暺恕 User-Agent
        if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                             "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.81 Safari/537.36 Edg/104.0.1293.47");
        }

        // ?蔭 HttpClient ?行
        HttpClientInterceptors.ForEach(u =>
        {
            u?.Invoke(httpClient);
        });

        // 璉?亥窈瘙?嚗??恥?瑞垢 BaseAddress 瘝⊥??蔭銝?RequestUrl 銋瓷?蔭
        if (string.IsNullOrWhiteSpace(httpClient.BaseAddress?.OriginalString) && string.IsNullOrWhiteSpace(RequestUrl)) throw new NullReferenceException(RequestUrl);

        // 憭?璅⊥?桅?
        RequestUrl = RequestUrl.Render(Templates, EncodeUrl);

        // ?遣霂瑟?撖寡情
        var request = new HttpRequestMessage(Method, RequestUrl)
        {
            Version = new Version(HttpVersion)
        };
        request.AppendQueries(Queries, EncodeUrl, IgnoreNullValueQueries);

        // 霈曄蔭霂瑟??交?憭?
        request.AppendHeaders(Headers);

        // 撉?璅∪??嚗雿鈭?body 蝐餃?嚗?
        if (ValidationState.Enabled)
        {
            // ?斗?臬?舐 Null 撉?銝?body ?潔蛹 null
            if (ValidationState.IncludeNull && Body == null) throw new InvalidOperationException($"The `{nameof(Body)}` can not be null.");

            // 撉?璅∪?
            Body?.Validate();
        }

        // 霈曄蔭 HttpContent
        SetHttpContent(request);

        // ?蔭霂瑟??行
        RequestInterceptors.ForEach(u =>
        {
            u?.Invoke(httpClient, request);
        });

        // ??窈瘙?
        App.PrintToMiniProfiler(MiniProfilerCategory, "Sending", $"[{Method}] {httpClientOriginalString}{request.RequestUri?.OriginalString}");

        // ?撘虜
        Exception exception = default;
        HttpResponseMessage response = default;

        try
        {
            if (RetryPolicy == null) response = await httpClient.SendAsync(request, cancellationToken);
            else
            {
                // 憭梯揖??
                await Retry.InvokeAsync(async () =>
                {
                    // ?窈瘙?
                    response = await httpClient.SendAsync(request, cancellationToken);
                }, RetryPolicy.Value.NumRetries, RetryPolicy.Value.RetryTimeout);
            }
        }
        catch (Exception ex)
        {
            // 閫血??芸?銋?隞?
            if (response != null && OnRequestFailded != null) OnRequestFailded(this, new HttpRequestFaildedEventArgs(request, response, ex));

            exception = ex;
        }

        // 璉?亙?摨???臬銝?301,302 ??摨仍撣?Location
        if (response?.StatusCode == HttpStatusCode.MovedPermanently || response?.StatusCode == HttpStatusCode.Found)
        {
            // ?瑕? Location 憭湧銝剔??託RL
            var redirectUrl = response.Headers.Location.AbsoluteUri;

            try
            {
                if (RetryPolicy == null) response = await httpClient.GetAsync(redirectUrl, cancellationToken);
                else
                {
                    // 憭梯揖??
                    await Retry.InvokeAsync(async () =>
                    {
                        // ??窈瘙?啁? URL
                        response = await httpClient.GetAsync(redirectUrl, cancellationToken);
                    }, RetryPolicy.Value.NumRetries, RetryPolicy.Value.RetryTimeout);
                }
            }
            catch (Exception ex)
            {
                // 閫血??芸?銋?隞?
                if (response != null && OnRequestFailded != null) OnRequestFailded(this, new HttpRequestFaildedEventArgs(request, response, ex));

                exception = ex;
            }
        }

        // 霂瑟???
        if (response?.IsSuccessStatusCode == true && exception == default)
        {
            // ???霂瑟?
            App.PrintToMiniProfiler(MiniProfilerCategory, "Succeeded", $"[StatusCode: {response.StatusCode}] Succeeded");

            // 靚???行??
            ResponseInterceptors.ForEach(u =>
            {
                u?.Invoke(httpClient, response);
            });
        }
        // 霂瑟?撘虜
        else
        {
            // 霂餃??秤瘨
            var errors = exception != null
                ? exception?.Message
                : response?.ReasonPhrase;
            var statusCode = (int)(response?.StatusCode ?? HttpStatusCode.InternalServerError);

            // ?憭梯揖霂瑟?
            App.PrintToMiniProfiler(MiniProfilerCategory, "Failed", $"[StatusCode: {statusCode}] {errors}", exception != null);

            // 靚撘虜?行??
            if (ExceptionInterceptors != null && ExceptionInterceptors.Count > 0) ExceptionInterceptors.ForEach(u =>
            {
                u?.Invoke(httpClient, response, errors);
            });

            // ?霂瑟?撘虜
            if (exception != null) throw exception;
        }

        return response;
    }

    /// <summary>
    /// 霈曄蔭 HttpContent
    /// </summary>
    /// <param name="request"></param>
    private void SetHttpContent(HttpRequestMessage request)
    {
        // GET/HEAD 霂瑟?銝?挽蝵?Body 霂瑟?嚗?023.08.02 蝘駁甇文?哨????舫??蝔祗閮?舀? GET 霈曄蔭 Body嚗?
        // if (Method == HttpMethod.Get || Method == HttpMethod.Head) return;

        HttpContent httpContent = null;

        // 憭??? Body 蝐餃?
        switch (ContentType)
        {
            case "multipart/form-data":

                var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
                var multipartFormDataContent = new MultipartFormDataContent(boundary);

                // ?????隡?隞?
                foreach (var httpFile in Files)
                {
                    // ?瑕??辣 Content-Type 蝐餃?
                    FS.TryGetContentType(httpFile.FileName, out var contentType);

                    // 憭? Bytes ?辣
                    if (httpFile.Bytes != null)
                    {
                        var byteArrayContent = new ByteArrayContent(httpFile.Bytes);
                        byteArrayContent.Headers.TryAddWithoutValidation("Content-Type", contentType ?? "application/octet-stream");

                        if (string.IsNullOrWhiteSpace(httpFile.FileName))
                            multipartFormDataContent.Add(byteArrayContent, Uri.EscapeDataString(httpFile.Name));
                        else
                            multipartFormDataContent.Add(byteArrayContent, Uri.EscapeDataString(httpFile.Name), Uri.EscapeDataString(httpFile.FileName));
                    }

                    // 憭? Stream ?辣
                    if (httpFile.FileStream != null)
                    {
                        var streamContent = new StreamContent(httpFile.FileStream, (int)httpFile.FileStream.Length);
                        streamContent.Headers.TryAddWithoutValidation("Content-Type", contentType ?? "application/octet-stream");

                        if (string.IsNullOrWhiteSpace(httpFile.FileName))
                            multipartFormDataContent.Add(streamContent, Uri.EscapeDataString(httpFile.Name));
                        else
                            multipartFormDataContent.Add(streamContent, Uri.EscapeDataString(httpFile.Name), Uri.EscapeDataString(httpFile.FileName));
                    }
                }

                // 憭??嗡?蝐餃?
                var dic = ConvertBodyToDictionary();
                if (dic != null && dic.Count > 0)
                {
                    foreach (var (key, value) in dic)
                    {
                        multipartFormDataContent.Add(new StringContent(value ?? string.Empty, ContentEncoding), string.Format("\"{0}\"", key));
                    }
                }

                // 閫? boundary 撣血?撘?桅?
                multipartFormDataContent.Headers.Remove("Content-Type");
                multipartFormDataContent.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);

                // 霈曄蔭?捆蝐餃?
                httpContent = multipartFormDataContent;
                break;

            case "application/octet-stream":
                if (Files.Count > 0 && Files[0].Bytes.Length > 0)
                {
                    httpContent = new ByteArrayContent(Files[0].Bytes);

                    // 霈曄蔭?捆蝐餃?
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                }
                break;

            case "application/json":
            case "text/json":
            case "application/*+json":
                if (Body != null)
                {
                    httpContent = new StringContent(SerializerObject(Body), ContentEncoding);

                    // 霈曄蔭?捆蝐餃?
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                }
                break;

            case "application/x-www-form-urlencoded":
                // 閫??摮
                var keyValues = ConvertBodyToDictionary();

                if (keyValues == null || keyValues.Count == 0) return;

                // 霈曄蔭?捆蝐餃?
                if (EncodeUrl)
                {
                    httpContent = new FormUrlEncodedContent(keyValues);
                }
                else
                {
                    var formData = string.Join('&', keyValues.Select(kv => $"{kv.Key}={kv.Value}"));
                    httpContent = new StringContent(formData, ContentEncoding, ContentType);
                }

                break;

            case "application/xml":
            case "text/xml":
            case "text/html":
            case "text/plain":
                if (Body != null) httpContent = new StringContent(Body.ToString(), ContentEncoding, ContentType);
                break;

            default:
                // ?嗡?蝐餃??舫? HttpRequestMessage ?行?刻挽蝵?
                break;
        }

        // 霈曄蔭 HttpContent
        if (httpContent != null) request.Content = httpContent;
    }

    /// <summary>
    /// 頧祆 Body 銝?摮蝐餃?
    /// </summary>
    /// <returns></returns>
    private IDictionary<string, string> ConvertBodyToDictionary()
    {
        IDictionary<string, string> keyValues = null;
        if (Body == null) return default;

        // 憭????
        if (Body is IDictionary<string, string> dic) keyValues = dic;
        else if (Body is IDictionary<string, object> dicObj) keyValues = dicObj.ToDictionary(u => u.Key, u => SerializerObject(u.Value));
        else keyValues = Body.ToDictionary().ToDictionary(u => u.Key, u => SerializerObject(u.Value));
        return keyValues;
    }

    /// <summary>
    /// 摨??笆鞊?
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    private string SerializerObject(object body)
    {
        if (body == null) return default;
        if (body is string) return body as string;
        if (body.GetType().IsValueType) return body.ToString();

        // 閫??摨??極??
        var jsonSerializer = App.GetService(JsonSerializerProvider, RequestScoped ?? App.RootServices) as IJsonSerializerProvider;
        return jsonSerializer.Serialize(body, JsonSerializerOptions);
    }

    /// <summary>
    /// 閫?????交? charset 蝻?
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private static Encoding GetCharsetEncoding(HttpResponseMessage response)
    {
        if (response == null) return Encoding.UTF8;

        // ?瑕? charset
        string charset;

        // ?瑕???憭渡?蝻??澆?
        var withContentType = response.Content.Headers.TryGetValues("Content-Type", out var contentTypes);
        if (withContentType)
        {
            charset = contentTypes.First()
                                  .Split(';', StringSplitOptions.RemoveEmptyEntries)
                                  .Where(u => u.Contains("charset", StringComparison.OrdinalIgnoreCase))
                                  .FirstOrDefault() ?? "charset=UTF-8";
        }
        else charset = "charset=UTF-8";

        var encoding = charset.Split('=', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "UTF-8";

        // ????charset ?妍
        var encodingName = encoding.Equals("utf8", StringComparison.OrdinalIgnoreCase) ? "UTF-8" :
                           encoding.Equals("utf16", StringComparison.OrdinalIgnoreCase) ? "UTF-16" :
                           encoding.Equals("utf32", StringComparison.OrdinalIgnoreCase) ? "UTF-32" :
                           encoding;

        // ?瑕? Encoding
        try
        {
            return Encoding.GetEncoding(encodingName);
        }
        catch (ArgumentException)
        {
            // 憒???霂 encodingName嚗?餈?暺恕??UTF-8 蝻?
            return Encoding.UTF8;
        }
    }
}
