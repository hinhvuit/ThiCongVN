using AttcMN.Framework.Configs;

namespace AttcMN.Framework
{
    public static class RyApp
    {
        public static RuoYiConfig RuoYiConfig = App.GetConfig<RuoYiConfig>("RuoYiConfig");
        public static UserConfig UserConfig = App.GetConfig<UserConfig>("UserConfig");
    }
}

