using BloomersIntegrationsCore.Domain.Entities;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Domain.Entities.LinxMicrovix;
using BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.Base;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Repositorys.LinxMicrovix
{
    public class LinxProdutosCodBarRepository : ILinxProdutosCodBarRepository
    {
        private readonly ILinxMicrovixRepositoryBase<LinxProdutosCodBar> _linxMicrovixRepositoryBase;

        public LinxProdutosCodBarRepository(ILinxMicrovixRepositoryBase<LinxProdutosCodBar> linxMicrovixRepositoryBase) =>
            _linxMicrovixRepositoryBase = linxMicrovixRepositoryBase;

        public void BulkInsertIntoTableRaw(List<LinxProdutosCodBar> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetCompanysAsync(string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Company> GetCompanysNotAsync(string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetParametersAsync(string tableName, string database, string parameterCol)
        {
            string sql = $@"SELECT {parameterCol} FROM [BLOOMERS_LINX].[dbo].[LinxAPIParam] (nolock) where method = '{tableName}'";

            try
            {
                return await _linxMicrovixRepositoryBase.GetParametersAsync(tableName, sql);
            }
            catch
            {
                throw;
            }
        }

        public string GetParametersNotAsync(string tableName, string database, string parameterCol)
        {
            throw new NotImplementedException();
        }

        public Task<List<LinxProdutosCodBar>> GetRegistersExistsAsync(List<LinxProdutosCodBar> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public List<LinxProdutosCodBar> GetRegistersExistsNotAsync(List<LinxProdutosCodBar> registros, string tableName, string database)
        {
            throw new NotImplementedException();
        }

        public async Task InsereRegistroIndividualAsync(LinxProdutosCodBar registro, string tableName, string database)
        {
            string sql = @$"INSERT INTO {database}..{tableName}_raw 
                            ([lastupdateon], [portal], [cod_produto], [cod_barra], [timestamp]) 
                            Values 
                            (@lastupdateon, @portal, @cod_produto, @cod_barra, @timestamp)";

            try
            {
                await _linxMicrovixRepositoryBase.InsereRegistroIndividualAsync(tableName, sql, registro);
            }
            catch
            {
                throw;
            }
        }

        public void InsereRegistroIndividualNotAsync(LinxProdutosCodBar registro, string tableName, string database)
        {
            throw new NotImplementedException();
        }
    }
}
