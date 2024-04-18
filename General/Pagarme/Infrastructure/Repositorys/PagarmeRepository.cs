using BloomersGeneralIntegrations.Pagarme.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersGeneralIntegrations.Pagarme.Infrastructure.Repositorys
{
    public class PagarmeRepository : IPagarmeRepository
    {
        private readonly ISQLServerConnection _conn;

        public PagarmeRepository(ISQLServerConnection conn) =>
            _conn = conn;

        public async Task InsereReceivableInDatabase(Root root)
        {
            foreach (var data in root.data)
            {
                var sql = $@"INSERT INTO [GENERAL].[dbo].[PagarmeRecebiveis_raw] ([lastupdateon], [id], [status], [amount], [fee], [anticipation_fee], [fraud_coverage_fee], [installment], [gateway_id], [split_id], [charge_id], [recipient_id],
                                                                                  [payment_date], [type], [payment_method], [accrual_at], [created_at]) 
                         VALUES(@Pedido, GETDATE(), @Retorno, @RemetenteID, @StatusFlash, @ChaveNFe)";
                try
                {
                    using (var conn = _conn.GetDbConnection())
                    {
                        await conn.ExecuteAsync(sql, new { lastupdateon = DateTime.Now, id = data.id, status = data.status, amount = data.amount.ToString().Insert(data.amount.ToString().Length, ","), fee = data.fee, anticipation_fee = data.anticipation_fee, fraud_coverage_fee = data.fraud_coverage_fee, installment = data.installment, gateway_id = data.gateway_id, split_id = data.split_id, charge_id = data.charge_id, recipient_id = data.recipient_id, payment_date = data.payment_date, type = data.type, payment_method = data.payment_method, accrual_at = data.accrual_at, created_at = data.created_at }, commandTimeout: 360);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(@$"Pagarme - InsereRecebivelInDatabase - Erro ao inserir registro: {data.id} na tabela GENERAL..Paybles - {ex.Message}");
                }
            }
        }
    }
}
