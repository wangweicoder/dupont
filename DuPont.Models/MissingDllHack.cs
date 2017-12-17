using System.Data.Entity.SqlServer;

namespace DuPont.Models
{
    internal class MissingDllHack
    {
        private SqlProviderServices _ = SqlProviderServices.Instance;
    }
}
