using BloomersGeneralIntegrations.AfterSale.Domain.Entities;
using BloomersIntegrationsCore.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;
using System.Data;

namespace BloomersGeneralIntegrations.AfterSale.Infrastructure.Repositorys;

public class AfterSaleRepository : IAfterSaleRepository
{
    private readonly ISQLServerConnection _conn;

    public AfterSaleRepository(ISQLServerConnection conn) =>
        (_conn) = (conn);

    public async Task CallDbProcMerge()
    {
        try
        {
            using (var conn = _conn.GetDbConnection())
            {
                await conn.ExecuteAsync($"GENERAL..P_AFTERSALEREVERSAS_SINCRONIZACAO", commandTimeout: 180, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"AfterSale - CallDbProcMerge - Erro ao realizar merge na tabela AfterSaleReversas, através da proc : P_AFTERSALEREVERSAS_SINCRONIZACAO - {ex.Message}");
        }
    }

    public async Task<BearerToken> GetBearerToken(string doc_company)
    {
        var sql = $@"SELECT CNPJ_EMP AS DOC_COMPANY, TOKEN FROM [GENERAL].[dbo].[PARAMETROS_AFTERSALE] WHERE CNPJ_EMP = '{doc_company}'";

        try
        {
            using (var conn = _conn.GetDbConnection())
            {
                var result = await conn.QueryAsync<BearerToken>(sql, commandTimeout: 360);
                return result.First();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(@$"AfterSale - GetBearerToken - Erro ao obter token de autorização da tabela GENERAL..Parametros_AfterSale - {ex.Message}");
        }
    }

    public async Task<IEnumerable<Company>> GetCompanys()
    {
        var sql = $@"SELECT CNPJ_EMP AS DOC_COMPANY FROM [BLOOMERS_LINX].[dbo].[LINXLOJAS_TRUSTED] WHERE EMPRESA = 1 OR EMPRESA = 5";

        try
        {
            using (var conn = _conn.GetDbConnection())
            {
                return await conn.QueryAsync<Company>(sql, commandTimeout: 360);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(@$"AfterSale - GetCnpjsEmp - Erro ao obter cnpjs das empresas da tabela BLOOMERS_LINX..LinxLojas_trusted - {ex.Message}");
        }
    }

    public async Task InsertIntoTable(List<Reverses> reverses)
    {
        try
        {
            foreach (var reverse in reverses)
            {
                var sql = @$"INSERT INTO GENERAL..AfterSaleReversas_raw 
                                (id, reverse_type, reverse_type_name, created_at, updated_at, order_id, total_amount, returned_invoice,
                                 customer_id, customer_first_name, customer_last_name, status_id, status_name, status_description,
                                 tracking_authorization_code, tracking_shipping_amount, refunds_count)
                          VALUES 
                                (@id, @reverse_type, @reverse_type_name, @created_at, @updated_at, @order_id, @total_amount, @returned_invoice,
                                 @customer_id, @customer_first_name, @customer_last_name, @status_id, @status_name, @status_description,
                                 @tracking_authorization_code, @tracking_shipping_amount, @refunds_count)";

                using (var conn = _conn.GetDbConnection())
                {
                    await conn.ExecuteAsync(sql, new
                    {
                        id = reverse.id,
                        reverse_type = reverse.reverse_type,
                        reverse_type_name = reverse.reverse_type_name,
                        created_at = reverse.created_at.Replace("T", " ").Replace(".000000Z", ""),
                        updated_at = reverse.updated_at.Replace("T", " ").Replace(".000000Z", ""),
                        order_id = reverse.order_id,
                        total_amount = reverse.total_amount,
                        returned_invoice = reverse.returned_invoice,
                        customer_id = reverse.customer.id,
                        customer_first_name = reverse.customer.first_name,
                        customer_last_name = reverse.customer.last_name,
                        status_id = reverse.status.id,
                        status_name = reverse.status.name,
                        status_description = reverse.status.description,
                        tracking_authorization_code = reverse.tracking.authorization_code,
                        tracking_shipping_amount = reverse.tracking.shipping_amount,
                        refunds_count = reverse.refunds_count
                    }, commandTimeout: 360);
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(@$"AfterSale - InsertIntoTable - Erro ao inserir reversas na tabela GENERAL..AfterSaleReversas - {ex.Message}");
        }
    }
}
