
using BloomersIntegrationsCore.Domain.Entities;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class DeliveryListService : IDeliveryListService
    {
        private readonly string path = @"C:\printer";
        private readonly string pathDeliveryLists = @"C:\printer\deliverylists";
        private readonly IDeliveryListRepository _deliveryListRepository;

        public DeliveryListService(IDeliveryListRepository deliveryListRepository) =>
            (_deliveryListRepository) = (deliveryListRepository);

        public async Task<string> GetDeliveryListToPrint(string fileName)
        {
            try
            {
                string path = @$"C:\printer\deliverylists\{fileName}";

                using (var fileInput = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();
                    await fileInput.CopyToAsync(memoryStream);

                    var buffer = memoryStream.ToArray();
                    return Convert.ToBase64String(buffer);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetOrderShipped(string nr_pedido, string serie, string cnpj_emp, string transportadora)
        {
            var list = _deliveryListRepository.GetOrderShipped(nr_pedido, serie, cnpj_emp, transportadora);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetOrdersShipped(string cod_transportadora, string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var list = await _deliveryListRepository.GetOrdersShipped(cod_transportadora, cnpj_emp, serie_pedido, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<bool> PrintOrder(string serializePedidosList)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathDeliveryLists))
                    Directory.CreateDirectory(pathDeliveryLists);

                var pedidosList = JsonConvert.DeserializeObject<List<Order>>(serializePedidosList);
                var volumes = pedidosList.GroupBy(x => x.volumes).Select(g => new { soma = g.Sum(x => x.volumes) }).First();
                var fileName = $@"{pathDeliveryLists}\deliverylists{pedidosList.First().company.doc_company.Substring(pedidosList.First().company.doc_company.Length - 3)} - {DateTime.Now.Date.ToString("yyyy-mm-dd")}.pdf";

                QuestPDF.Settings.License = LicenseType.Community;

                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(50);

                        page.Header().Element(x => x.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text($"Romaneio de Embarque").Style(titleStyle);

                                column.Item().Text(text =>
                                {
                                    text.Span("Total de Notas: ").SemiBold();
                                    text.Span($"{pedidosList.Count()}");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Total de Volumes: ").SemiBold();
                                    text.Span($"{volumes.soma}");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Data: ").SemiBold();
                                    text.Span($"{DateTime.Now.Date.ToString("yyyy-MM-dd")}");
                                });
                            });

                            row.ConstantItem(100).Height(50).Placeholder();
                        }));

                        page.Content().Element(x => x.PaddingVertical(40).Column(column =>
                        {
                            column.Spacing(11);

                            column.Item().Element(container => container.Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(80); //nr_pedido
                                    columns.ConstantColumn(50); //nota
                                    columns.ConstantColumn(80); //valor
                                    columns.ConstantColumn(100); //nome
                                    columns.ConstantColumn(80); //doc
                                    columns.ConstantColumn(80); //endereco
                                    columns.ConstantColumn(40); //numero
                                    columns.ConstantColumn(50); //bairro
                                    columns.ConstantColumn(70); //cidade
                                    columns.ConstantColumn(50); //uf
                                    columns.ConstantColumn(60); //cep
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).AlignCenter().Text("pedido");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("nota");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("valor");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("cliente");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("doc");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("endereco");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("num");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("bairro");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("cidade");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("uf");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("cep");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var pedido in pedidosList)
                                {
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.number);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.invoice.number_nf);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(String.Format("{0:C}", pedido.invoice.amount_nf));
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.reason_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.doc_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.address_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.street_number_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.neighborhood_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.city_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.uf_client);
                                    table.Cell().Element(CellStyle).AlignCenter().Text(pedido.client.zip_code_client);

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                    }
                                }
                            }));
                        }));

                        page.Footer().Element(x => x.Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Column(column =>
                            {
                                column.Item().Text(text =>
                                {
                                    text.Span("_______________________________________").SemiBold();
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span($"Expedido por: NATÁLIA BARBOSA").SemiBold();
                                });
                            });

                            row.RelativeItem().AlignCenter().Column(column =>
                            {
                                column.Item().Text(text =>
                                {
                                    text.CurrentPageNumber();
                                    text.Span(" / ");
                                    text.TotalPages();
                                });
                            });

                            row.RelativeItem().AlignRight().Column(column =>
                            {
                                column.Item().Text(text =>
                                {
                                    text.Span("Transportadora: ").SemiBold();
                                    text.Span($"{pedidosList.First().shippingCompany.reason_shippingCompany}");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Motorista: ").SemiBold();
                                    text.Span("_______________________");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Documento: ").SemiBold();
                                    text.Span("_______________________");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Placa do Veiculo: ").SemiBold();
                                    text.Span("_______________________");
                                });
                            });
                        }));
                    });
                }).GeneratePdf($@"{fileName}");

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
