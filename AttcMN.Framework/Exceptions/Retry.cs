namespace AttcMN.Framework.Exceptions;

/// <summary>
/// ???掩
/// </summary>
[SuppressSniffer]
public sealed class Retry
{
    /// <summary>
    /// ????撣貊??寞?嚗??臭誑???孵?撘虜
    /// </summary>
    /// <param name="action"></param>
    /// <param name="numRetries">??甈⊥</param>
    /// <param name="retryTimeout">???湧??園</param>
    /// <param name="finalThrow">?臬?蝏?撘虜</param>
    /// <param name="exceptionTypes">撘虜蝐餃?,?臬?銝?/param>
    /// <param name="fallbackPolicy">??憭梯揖??</param>
    /// <param name="retryAction">???嗉??冽瘜?/param>
    public static void Invoke(Action action
        , int numRetries
        , int retryTimeout = 1000
        , bool finalThrow = true
        , Type[] exceptionTypes = default
        , Action<Exception> fallbackPolicy = default
        , Action<int, int> retryAction = default)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        InvokeAsync(async () =>
        {
            action();
            await Task.CompletedTask;
        }, numRetries, retryTimeout, finalThrow, exceptionTypes, fallbackPolicy == null ? null
        : async (ex) =>
        {
            fallbackPolicy?.Invoke(ex);
            await Task.CompletedTask;
        }, retryAction).GetAwaiter().GetResult();
    }

    /// <summary>
    /// ????撣貊??寞?嚗??臭誑???孵?撘虜
    /// </summary>
    /// <param name="action"></param>
    /// <param name="numRetries">??甈⊥</param>
    /// <param name="retryTimeout">???湧??園</param>
    /// <param name="finalThrow">?臬?蝏?撘虜</param>
    /// <param name="exceptionTypes">撘虜蝐餃?,?臬?銝?/param>
    /// <param name="fallbackPolicy">??憭梯揖??</param>
    /// <param name="retryAction">???嗉??冽瘜?/param>
    /// <returns><see cref="Task"/></returns>
    public static async Task InvokeAsync(Func<Task> action
        , int numRetries
        , int retryTimeout = 1000
        , bool finalThrow = true
        , Type[] exceptionTypes = default
        , Func<Exception, Task> fallbackPolicy = default
        , Action<int, int> retryAction = default)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        // 憒???甈⊥撠???鈭?0嚗??湔靚
        if (numRetries <= 0)
        {
            await action();
            return;
        }

        // 摮?餌???甈⊥
        var totalNumRetries = numRetries;

        // 銝??
        while (true)
        {
            try
            {
                await action();
                break;
            }
            catch (Exception ex)
            {
                // 憒??舫?霂活?啣?鈭?蝑?0嚗?蝏迫??
                if (--numRetries < 0)
                {
                    if (finalThrow)
                    {
                        if (fallbackPolicy != null) await fallbackPolicy.Invoke(ex);
                        throw;
                    }
                    else return;
                }

                // 憒?憛怠?鈭?exceptionTypes 銝?撣貊掩????exceptionTypes 銋?嚗?蝏迫??
                if (exceptionTypes != null && exceptionTypes.Length > 0 && !exceptionTypes.Any(u => u.IsAssignableFrom(ex.GetType())))
                {
                    if (finalThrow)
                    {
                        if (fallbackPolicy != null) await fallbackPolicy.Invoke(ex);
                        throw;
                    }
                    else return;
                }

                // ??靚憪?
                retryAction?.Invoke(totalNumRetries, totalNumRetries - numRetries);

                // 憒??舫?霂?撣豢憭找? 0嚗??湧????園?誧蝏剜銵?
                if (retryTimeout > 0) await Task.Delay(retryTimeout);
            }
        }
    }
}
