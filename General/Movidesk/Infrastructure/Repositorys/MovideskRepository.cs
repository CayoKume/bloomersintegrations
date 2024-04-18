using BloomersGeneralIntegrations.Movidesk.Domain.Entities;
using BloomersIntegrationsCore.Infrastructure.SQLServer.Connection;
using Dapper;

namespace BloomersGeneralIntegrations.Movidesk.Infrastructure.Repositorys
{
    public class MovideskRepository : IMovideskRepository
    {
        private readonly ISQLServerConnection _conn;

        public MovideskRepository(ISQLServerConnection conn) =>
            (_conn) = (conn);

        public async Task<List<Ticket>> GetBlankTicketsFromDatabase()
        {
            string sql = $@"SELECT TOP 20 ID FROM GENERAL..MOVIDESKTICKETAPROVAFATURA WHERE ASSUNTO = '' AND DATA_CRIACAO = '' AND NOME_RESPONSAVEL = '' AND EMAIL_RESPONSAVEL = '' AND TELEFONE_RESPONSAVEL = ''";

            try
            {
                var result = await _conn.GetIDbConnection().QueryAsync<Ticket>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(@$" - GetBlankTicketsFromDatabase - Erro ao obter tickets em branco da tabela GENERAL..MovideskTicketAprovaFatura - {ex.Message}");
            }
        }

        public async Task<List<Ticket>> GetTicketsFromDatabase()
        {
            string sql = $@"SELECT TOP 20 ID FROM GENERAL..MOVIDESKTICKETAPROVAFATURA WHERE FATURA_GERADA = 'N' AND ASSUNTO != '' AND DATA_CRIACAO != '' AND NOME_RESPONSAVEL != ''";

            try
            {
                var result = await _conn.GetIDbConnection().QueryAsync<Ticket>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(@$" - GetTicketsFromDatabase - Erro ao obter tickets da tabela GENERAL..MovideskTicketAprovaFatura - {ex.Message}");
            }
        }

        public async Task<bool> InsertInvoice(Invoice invoice)
        {
            string sql = $@"INSERT INTO GENERAL..MovideskAprovaFatura                               
                            VALUES ('{invoice.nome_empresa}', '{invoice.cnpj_empresa}', '{invoice.id_fornecedor}', '{invoice.nome_fantasia_fornecedor}', '{invoice.cnpj_fornecedor}', '{invoice.endereco_fornecedor}', '{invoice.numero_endereco_fornecedor}', '{invoice.uf_fornecedor}', '{invoice.cidade_fornecedor}', '{invoice.cep_fornecedor}',
                                    '{invoice.agencia_fornecedor}', '{invoice.banco_fornecedor}', '{invoice.conta_corrente_fornecedor}', '{invoice.IdFaturaCadastrada}', '{invoice.forma_pagamento_fatura}', '{invoice.codigo_barras_fatura}', '{invoice.pix_fatura}', '{invoice.numero_documento_fatura}', '{invoice.serie_documento_fatura}', '{invoice.data_emissao_fatura.ToString("yyyy-MM-dd HH:mm:ss")}',
                                    '{invoice.data_vencimento_fatura.ToString("yyyy-MM-dd HH:mm:ss")}', '{invoice.data_lancamento_fatura.ToString("yyyy-MM-dd HH:mm:ss")}', '{invoice.numero_parcela_fatura}', '{invoice.total_parcelas_fatura}', '{invoice.valor_fatura.ToString("0.##").Replace(",", ".")}', '{invoice.centro_custo_fatura}',
                                    '{invoice.historico_contabil_fatura}', '{invoice.serializable}', '{invoice.id_ticket}', '{invoice.obs}', 'N', 'N', '')";
            try
            {
                var result = await _conn.GetIDbConnection().ExecuteAsync(sql, invoice);

                if (result > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(@$" - InsereFatura - Erro ao inserir fatura gerada na tabela GENERAL..MovideskAprovaFatura - {ex.Message}");
            }
        }

        public async Task<bool> InsertTicket(Ticket? ticket)
        {
            string sql = $@"INSERT INTO MovideskTicketAprovaFatura VALUES (GETDATE(),{ticket.id},'',GETDATE(),'','','','','N')";

            try
            {
                var result = await _conn.GetIDbConnection().ExecuteAsync(sql, ticket);

                if (result > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"InsereTicket - {ex}");
            }
        }

        public async Task UpdateTicketFromDatabase(List<Ticket> tickets)
        {
            foreach (var ticket in tickets)
            {
                string sql = $@"UPDATE [GENERAL].[dbo].[MovideskTicketAprovaFatura]
	                        SET
                                [lastupdateon] = GETDATE(),
	                            [assunto] = '{ticket.subject}',
                                [data_criacao] = '{ticket.createdDate.ToString("yyyy-MM-dd hh:mm:ss")}',
                                [nome_responsavel] = '{ticket.createdBy.businessName}',
                                [email_responsavel] = '{ticket.createdBy.email}',
                                [telefone_responsavel] = '{ticket.createdBy.phone}'
	                        WHERE
	                            [id] = '{ticket.id}'
                                AND [fatura_gerada] = 'N'";
                try
                {
                    await _conn.GetIDbConnection().ExecuteAsync(sql);
                }
                catch (Exception ex)
                {
                    throw new Exception(@$" - UpdateTicketFromDatabase - Erro ao atualizar ticket: {ticket.id} na tabela GENERAL..MovideskTicketAprovaFatura - {ex.Message}");
                }
            }
        }

        public async Task UpdateTicketStatusFromDatabase(Invoice invoice)
        {
            string sql = $@"UPDATE [GENERAL].[dbo].[MovideskTicketAprovaFatura]
	                        SET
                                [fatura_gerada] = 'S'
	                        WHERE
	                            [id] = '{invoice.id_ticket}'
                                AND [fatura_gerada] = 'N'";
            try
            {
                await _conn.GetIDbConnection().ExecuteAsync(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(@$" - UpdateTicketStatusFromDatabase - Erro ao atualizar ticket: {invoice.id_ticket} na tabela GENERAL..MovideskTicketAprovaFatura - {ex.Message}");
            }
        }
    }
}
