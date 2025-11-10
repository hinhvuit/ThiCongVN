using AttcMN.Framework;
using System.Reflection;

namespace AttcMN.Admin
{
    public class SingleFilePublish : ISingleFilePublish
    {
        public Assembly[] IncludeAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        public string[] IncludeAssemblyNames()
        {
            return new[]
            {
                "AttcMN.Framework",
                "AttcMN.Common",
                "AttcMN.Data",
                "AttcMN.Admin",
                "AttcMN.System"
            };
        }
    }
}
