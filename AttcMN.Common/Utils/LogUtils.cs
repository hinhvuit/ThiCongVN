namespace AttcMN.Common.Utils
{
    public static class LogUtils
    {
        public static string GetBlock(object msg)
        {
            if (msg == null)
            {
                msg = "";
            }
            return "[" + msg.ToString() + "]";
        }
    }
}

