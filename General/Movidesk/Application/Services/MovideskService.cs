using BloomersGeneralIntegrations.Movidesk.Domain.Entities;
using BloomersGeneralIntegrations.Movidesk.Infrastructure.Apis;
using BloomersGeneralIntegrations.Movidesk.Infrastructure.Repositorys;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BloomersGeneralIntegrations.Movidesk.Application.Services
{
    public class MovideskService : IMovideskService
    {
        private readonly IAPICall _apiCall;
        private readonly IMovideskRepository _movideskRepository;

        public MovideskService(IMovideskRepository movideskRepository) =>
            (_movideskRepository) = (movideskRepository);

        public async Task BuildInvoicesFromTickets()
        {
            try
            {
                var filter = String.Empty;
                var response = String.Empty;
                var tickets = new List<Ticket>();

                tickets = await _movideskRepository.GetTicketsFromDatabase();
                if (tickets.Count() > 0)
                {
                    for (int i = 0; i < tickets.Count(); i++)
                    {
                        if (i != tickets.Count() - 1)
                            filter += $"id eq {tickets[i].id} or ";
                        else
                            filter += $"id eq {tickets[i].id}";
                    }
                    response = await _apiCall.GetAsync(filter);
                    var ticketsToBeBuild = JsonConvert.DeserializeObject<List<Ticket>>(response.Replace("'", "''"));
                    var faturas = BuildInvoicesFromTickets(ticketsToBeBuild);
                    foreach (var fatura in faturas)
                    {
                        if (await _movideskRepository.InsertInvoice(fatura))
                        {
                            await _movideskRepository.UpdateTicketStatusFromDatabase(fatura);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex.Message.Contains(" - "))
            {
                string[] subs = ex.Message.Split(" - ");
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertTicket(string request)
        {
            try
            {
                var ticketWebhook = JsonConvert.DeserializeObject<Ticket>(request);
                await _movideskRepository.InsertTicket(ticketWebhook);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateTicket(string request)
        {
            try
            {
                var filter = String.Empty;
                var response = String.Empty;
                var tickets = new List<Ticket>();

                tickets = await _movideskRepository.GetBlankTicketsFromDatabase();
                if (tickets.Count() > 0)
                {
                    for (int i = 0; i < tickets.Count(); i++)
                    {
                        if (i != tickets.Count() - 1)
                            filter += $"id eq {tickets[i].id} or ";
                        else
                            filter += $"id eq {tickets[i].id}";
                    }
                    response = await _apiCall.GetAsync(filter);
                    var updatedTickets = JsonConvert.DeserializeObject<List<Ticket>>(response.Replace("'", "''"));
                    await _movideskRepository.UpdateTicketFromDatabase(updatedTickets);
                }

                return true;
            }
            catch (Exception ex) when (ex.Message.Contains(" - "))
            {
                string[] subs = ex.Message.Split(" - ");
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<Invoice> BuildInvoicesFromTickets(IEnumerable<Ticket> tickets)
        {
            var listFaturas = new List<Invoice>();
            foreach (var ticket in tickets)
            {
                try
                {
                    var fatura = new Invoice();
                    foreach (var customField in ticket.customFieldValues)
                    {
                        try
                        {
                            if (customField.customFieldId == 90569 && ticket.customFieldValues.Where(c => c.customFieldId == 90569).First().value is not null)
                                fatura.codigo_barras_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 90569).First().value;

                            if (customField.customFieldId == 90557 && ticket.customFieldValues.Where(c => c.customFieldId == 90557).First().value is not null)
                                fatura.nome_fantasia_fornecedor = ticket.customFieldValues.Where(c => c.customFieldId == 90557).First().value;

                            if (customField.customFieldId == 90584 && ticket.customFieldValues.Where(c => c.customFieldId == 90584).First().value is not null)
                                fatura.cidade_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 90584).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                            if (customField.customFieldId == 90581 && ticket.customFieldValues.Where(c => c.customFieldId == 90581).First().value is not null)
                                fatura.endereco_fornecedor = ticket.customFieldValues.Where(c => c.customFieldId == 90581).First().value;

                            if (customField.customFieldId == 133070 && ticket.customFieldValues.Where(c => c.customFieldId == 133070).First().value is not null)
                                fatura.numero_endereco_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 133070).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                            if (customField.customFieldId == 90585 && ticket.customFieldValues.Where(c => c.customFieldId == 90585).First().items.Count() > 0)
                                fatura.uf_fornecedor = ticket.customFieldValues.Where(c => c.customFieldId == 90585).First().items.First().customFieldItem;

                            if (customField.customFieldId == 90580 && ticket.customFieldValues.Where(c => c.customFieldId == 90580).First().value is not null)
                                fatura.cep_fornecedor = ticket.customFieldValues.Where(c => c.customFieldId == 90580).First().value;

                            if (customField.customFieldId == 90560 && ticket.customFieldValues.Where(c => c.customFieldId == 90560).First().value is not null)
                                fatura.agencia_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 90560).First().value, @"[^0-9a-zA-Z]+", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                            if (customField.customFieldId == 90559 && ticket.customFieldValues.Where(c => c.customFieldId == 90559).First().items.Count() > 0)
                                fatura.banco_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 90559).First().items.First().customFieldItem, @"[^0-9a-zA-Z]+", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                            if (customField.customFieldId == 90561 && ticket.customFieldValues.Where(c => c.customFieldId == 90561).First().value is not null)
                                fatura.conta_corrente_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 90561).First().value, @"[^0-9a-zA-Z]+", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));

                            if (customField.customFieldId == 131957 && ticket.customFieldValues.Where(c => c.customFieldId == 131957).First().items.Count() > 0)
                                fatura.forma_pagamento_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 131957).First().items.First().customFieldItem;

                            if (customField.customFieldId == 139723 && ticket.customFieldValues.Where(c => c.customFieldId == 139723).First().value is not null)
                                fatura.pix_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 139723).First().value;

                            if (customField.customFieldId == 132100 && ticket.customFieldValues.Where(c => c.customFieldId == 132100).First().value is not null)
                                fatura.numero_documento_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 132100).First().value == String.Empty ? 0 : Convert.ToInt64(Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 132100).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)));

                            if (customField.customFieldId == 132101 && ticket.customFieldValues.Where(c => c.customFieldId == 132101).First().value is not null)
                                fatura.serie_documento_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 132101).First().value == String.Empty ? 0 : Convert.ToInt64(Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 132101).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)));

                            if (customField.customFieldId == 90562 && ticket.customFieldValues.Where(c => c.customFieldId == 90562).First().value is not null && ticket.customFieldValues.Where(c => c.customFieldId == 90562).First().value != "")
                                fatura.data_vencimento_fatura = Convert.ToDateTime(ticket.customFieldValues.Where(c => c.customFieldId == 90562).First().value.Split("T")[0]);

                            if (customField.customFieldId == 124596 && ticket.customFieldValues.Where(c => c.customFieldId == 124596).First().value is not null)
                                fatura.numero_parcela_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 124596).First().value == String.Empty ? 0 : Convert.ToInt32(Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 124596).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)));

                            if (customField.customFieldId == 132102 && ticket.customFieldValues.Where(c => c.customFieldId == 132102).First().value is not null)
                                fatura.total_parcelas_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 132102).First().value == String.Empty ? 0 : Convert.ToInt32(Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 132102).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5)));

                            if (customField.customFieldId == 107789 && ticket.customFieldValues.Where(c => c.customFieldId == 107789).First().value is not null)
                                fatura.valor_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 107789).First().value == String.Empty ? 0 : decimal.Parse(ticket.customFieldValues.Where(c => c.customFieldId == 107789).First().value.Replace(".", ","), new CultureInfo("pt-BR"));

                            if (ticket.serviceFirstLevel == "01 - Misha")
                            {
                                if (customField.customFieldId == 133016 && ticket.customFieldValues.Where(c => c.customFieldId == 133016).First().items.Count() > 0)
                                    fatura.centro_custo_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 133016).First().items.First().customFieldItem;
                            }
                            else if (ticket.serviceFirstLevel == "03 - Open Era")
                            {
                                if (customField.customFieldId == 133017 && ticket.customFieldValues.Where(c => c.customFieldId == 133017).First().items.Count() > 0)
                                    fatura.centro_custo_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 133017).First().items.First().customFieldItem;
                            }
                            else if (ticket.serviceFirstLevel == "04 - Margaux")
                            {
                                if (customField.customFieldId == 133018 && ticket.customFieldValues.Where(c => c.customFieldId == 133018).First().items.Count() > 0)
                                    fatura.centro_custo_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 133018).First().items.First().customFieldItem;
                            }
                            else if (ticket.serviceFirstLevel == "10 - New Bloomers")
                            {
                                if (customField.customFieldId == 133019 && ticket.customFieldValues.Where(c => c.customFieldId == 133019).First().items.Count() > 0)
                                    fatura.centro_custo_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 133019).First().items.First().customFieldItem;
                            }

                            if (customField.customFieldId == 134368 && ticket.customFieldValues.Where(c => c.customFieldId == 134368).First().items.Count() > 0)
                                fatura.historico_contabil_fatura = ticket.customFieldValues.Where(c => c.customFieldId == 134368).First().items.First().customFieldItem;

                            if (customField.customFieldId == 90558 && ticket.customFieldValues.Where(c => c.customFieldId == 90558).First().value is not null)
                                fatura.cnpj_fornecedor = Regex.Replace(ticket.customFieldValues.Where(c => c.customFieldId == 90558).First().value, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                        }
                        catch
                        {
                            throw new Exception(@$"{customField}");
                        }
                    }

                    fatura.nome_empresa = ticket.serviceFirstLevel;
                    fatura.id_ticket = Convert.ToInt32(ticket.id);
                    fatura.data_emissao_fatura = ticket.resolvedIn;
                    fatura.data_lancamento_fatura = ticket.resolvedIn;

                    if (fatura.numero_parcela_fatura == 0)
                        fatura.numero_parcela_fatura = 1;

                    if (fatura.total_parcelas_fatura == 0)
                        fatura.total_parcelas_fatura = 1;

                    if (ticket.serviceFirstLevel == "01 - Misha")
                        fatura.cnpj_empresa = "38367316000199";
                    else if (ticket.serviceFirstLevel == "03 - Open Era")
                        fatura.cnpj_empresa = "42538267000187";
                    else if (ticket.serviceFirstLevel == "04 - Margaux")
                        fatura.cnpj_empresa = "33713331000128";
                    else if (ticket.serviceFirstLevel == "10 - New Bloomers")
                        fatura.cnpj_empresa = "44065634000106";

                    if (fatura.data_vencimento_fatura.ToString() == "01/01/0001 00:00:00" || fatura.data_vencimento_fatura.ToString() == "1/1/0001 12:00:00 AM")
                        fatura.data_vencimento_fatura = DateTime.Now;

                    if (fatura.data_emissao_fatura.ToString() == "01/01/0001 00:00:00" || fatura.data_emissao_fatura.ToString() == "1/1/0001 12:00:00 AM")
                        fatura.data_emissao_fatura = ticket.createdDate;

                    if (fatura.data_lancamento_fatura.ToString() == "01/01/0001 00:00:00" || fatura.data_lancamento_fatura.ToString() == "1/1/0001 12:00:00 AM")
                        fatura.data_lancamento_fatura = ticket.createdDate;

                    fatura.obs = ticket.actions[0].description;

                    listFaturas.Add(fatura);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return listFaturas;
        }
    }
}
