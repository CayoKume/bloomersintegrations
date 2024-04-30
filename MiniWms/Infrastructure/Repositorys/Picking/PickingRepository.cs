using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomersMiniWmsIntegrations.Infrastructure.Repositorys
{
    public class PickingRepository
    {
        private readonly ISQLServerConnection _conn;

        public PickingRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);
    }
}
