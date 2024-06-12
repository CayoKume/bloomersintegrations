using BloomersMiniWmsIntegrations.Domain.Entities.Picking;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class PickingService : IPickingService
    {
        private readonly string path = @"C:\printer";
        private readonly string pathOrders = @"C:\printer\orders";
        private readonly IPickingRepository _pickingRepository;

        public PickingService(IPickingRepository pickingRepository) =>
            (_pickingRepository) = (pickingRepository);

        public async Task<string> GetShippingCompanys()
        {
            var list = await _pickingRepository.GetShippingCompanys();
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrder(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _pickingRepository.GetUnpickedOrder(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrders(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var list = await _pickingRepository.GetUnpickedOrders(cnpj_emp, serie_pedido, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _pickingRepository.GetUnpickedOrderToPrint(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetUnpickedOrdersToPrint(string cnpj_emp, string serie_pedido, string data_inicial, string data_final)
        {
            var list = await _pickingRepository.GetUnpickedOrdersToPrint(cnpj_emp, serie_pedido, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<bool> UpdateRetorno(string nr_pedido, int volumes, string listProdutos)
        {
            var result = await _pickingRepository.UpdateRetorno(nr_pedido, volumes, listProdutos);

            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> UpdateShippingCompany(string nr_pedido, int cod_transportador)
        {
            var result = await _pickingRepository.UpdateShippingCompany(nr_pedido, cod_transportador);

            if (result > 0)
                return true;
            else
                return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma", Justification = "<Pendente>")]
        public Task<bool> PrintOrder(string serializedOrder)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathOrders))
                    Directory.CreateDirectory(pathOrders);

                var pedido = JsonConvert.DeserializeObject<Order>(serializedOrder);
                var fileName = $@"{pathOrders}\orders{pedido.company.doc_company.Substring(pedido.company.doc_company.Length - 3)} - {DateTime.Now.Date.ToString("yyyy-mm-dd")}.pdf";
                var stream = new MemoryStream();

                ZXing.Windows.Compatibility.BarcodeWriter writer = new ZXing.Windows.Compatibility.BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE, // You can choose any supported barcode format here
                    Options = new EncodingOptions
                    {
                        NoPadding = true,
                        Margin = 0,
                    }
                };

                var barcodeBitmap = writer.Write($"{"Teste"}"); //Código do Pedido
                barcodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                QuestPDF.Settings.License = LicenseType.Community;

                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        var titleStyle = TextStyle.Default.FontSize(18).SemiBold().FontColor(Colors.Black);
                        var normalStyle = TextStyle.Default.FontSize(8).FontColor(Colors.Black);
                        var boldStyle = TextStyle.Default.FontSize(10).SemiBold().FontColor(Colors.Black);

                        page.Size(new PageSize((float)226.77, (float)850.39));
                        page.Margin(10);

                        page.Header().Element(x => x.Row(row =>
                        {
                            row.RelativeItem().AlignTop().Column(column =>
                            {
                                column.Item().Text($"{"Misha - Ecommerce"}").Style(boldStyle).AlignLeft();//Nome Emp
                                column.Item().Text($"{"RUA TABAPUA"}, {"1123"} ({"CONJ 171 172E 173"})").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                                column.Item().Text($"{"04533-014"} - {"ITAIM BIBI"} - {"SÃO PAULO"}/{"SP"}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                                column.Item().Text($"Tel: {"(11)3078-4949"}").Style(normalStyle).AlignLeft();//Telefone
                                column.Item().Text($"Cnpj: {"38.367.316/0001-99"} - Insc.Est. {"129635450110"}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                                column.Item().Text($"Email: {"financeiro@newbloomers.com.br"}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            });
                        }));

                        page.Content().Element(x => x.Column(column =>
                        {
                            column.Spacing(1);
                            column.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().MinHeight(100).MaxWidth(100).Image(stream.ToArray());
                            column.Item().PaddingBottom(10).AlignCenter().Text($"{"Teste"}").Style(titleStyle).AlignCenter();//Código do Pedido

                            column.Item().Text($"{"MNR BLING COMERCIO DE ROUPAS E ACESSORIOS LTDA"}").Style(boldStyle).AlignLeft();//Nome Cliente
                            column.Item().Text($"{"AV DO BATEL"}, {"1868"} {"LOJA 229 ANDAR L-2"}").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                            column.Item().Text($"{"80420-090"} - {"BATEL"} - {"CURITIBA"}/{"PR"}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                            column.Item().Text($"Tel: {"(11)3286-4849"}").Style(normalStyle).AlignLeft();//Telefone
                            column.Item().Text($"Cnpj: {"38.367.316/0009-46"} - Insc.Est. {"9101495202"}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            column.Item().Text($"Email: {"julianavasconcelos4@gmail.com"}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            
                            column.Item().PaddingTop(10).Text($"Transportadora: {"7601"} - {"TEX COURIER S.A"}").Style(normalStyle).AlignLeft();//Código da Transportadora, Transportadora
                            
                            column.Item().PaddingTop(10).Text($"OBS.\n\n{"EXPRESSO VIA TOTAL\r\n\r\nFernanda Vasconcellos (11)93940-4154 - Piso: L2 - Loja: 229"}").Style(normalStyle).AlignLeft();//Obs do Pedido

                            column.Item().Element(container => container.Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30); //nr_pedido
                                    columns.ConstantColumn(140); //nota
                                    columns.ConstantColumn(30); //valor
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Código");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Descrição do Produto");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Qtde");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold().Size(8)).PaddingTop(10).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var item in pedido.itens)
                                {
                                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.cod_product}").Style(normalStyle);
                                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.description_product}").Style(normalStyle);
                                    table.Cell().Element(CellStyle).AlignCenter().Text($"{item.quantity_product}").Style(normalStyle);

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                    }
                                }
                            }));

                            column.Item().PaddingTop(10).Text($"Total do Pedido: {"R$ 594,00"}").Style(normalStyle).AlignRight();//Obs do Pedido
                        }));
                    });
                }).GeneratePdf($@"{fileName}");

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
