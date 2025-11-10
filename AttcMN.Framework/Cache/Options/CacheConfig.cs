using AttcMN.Framework.Cache.Enums;
using AttcMN.Framework.Cache.Options;

namespace AttcMN.Framework.Cache.Redis;

public class CacheConfig
{
    public CacheType CacheType { get; set; }
    
    public RedisConfig? RedisConfig { get; set; }
}

