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
        private readonly string pathOrderToCupouns = @"C:\printer\orders";
        private readonly string pathExchangeCupouns = @"C:\printer\cupouns";
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
        public async Task<string> PrintOrderToCupoun(string serializedOrder)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathOrderToCupouns))
                    Directory.CreateDirectory(pathOrderToCupouns);

                var pedido = JsonConvert.DeserializeObject<Order>(serializedOrder);
                var fileName = $@"{pathOrderToCupouns}\{pedido.number}.pdf";
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

                var barcodeBitmap = writer.Write($"{pedido.number}"); //Código do Pedido
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
                                column.Item().Text($"{pedido.company.name_company}").Style(boldStyle).AlignLeft();//Nome Emp
                                column.Item().Text($"{pedido.company.address_company}, {pedido.company.street_number_company} ({pedido.company.complement_address_company})").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                                column.Item().Text($"{pedido.company.zip_code_company} - {pedido.company.neighborhood_company} - {pedido.company.city_company}/{pedido.company.uf_company}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                                column.Item().Text($"Tel: {pedido.company.fone_company}").Style(normalStyle).AlignLeft();//Telefone
                                column.Item().Text($"Cnpj: {pedido.company.doc_company} - Insc.Est. {pedido.company.state_registration_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                                column.Item().Text($"Email: {pedido.company.email_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            });
                        }));

                        page.Content().Element(x => x.Column(column =>
                        {
                            column.Spacing(1);
                            column.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().MinHeight(100).MaxWidth(100).Image(stream.ToArray());
                            column.Item().PaddingBottom(10).AlignCenter().Text($"{pedido.number}").Style(titleStyle).AlignCenter();//Código do Pedido

                            column.Item().Text($"{pedido.client.reason_client}").Style(boldStyle).AlignLeft();//Nome Cliente
                            column.Item().Text($"{pedido.client.address_client}, {pedido.client.street_number_client} {pedido.client.complement_address_client}").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                            column.Item().Text($"{pedido.client.zip_code_client} - {pedido.client.neighborhood_client} - {"CURITIBA"}/{"PR"}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                            column.Item().Text($"Tel: {pedido.client.fone_client}").Style(normalStyle).AlignLeft();//Telefone
                            column.Item().Text($"Doc: {pedido.client.doc_client} - Insc.Est. {pedido.client.state_registration_client}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            column.Item().Text($"Email: {pedido.client.email_client}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            
                            column.Item().PaddingTop(10).Text($"Transportadora: {pedido.shippingCompany.cod_shippingCompany} - {pedido.shippingCompany.reason_shippingCompany}").Style(normalStyle).AlignLeft();//Código da Transportadora, Transportadora
                            
                            column.Item().PaddingTop(10).Text($"OBS.\n{pedido.obs}").Style(normalStyle).AlignLeft();//Obs do Pedido

                            column.Item().PaddingTop(5).Element(container => container.Table(table =>
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

                            column.Item().PaddingTop(10).Text($"Total do Pedido: {Convert.ToDouble(pedido.amount.Replace(".", ",")).ToString("C")}").Style(normalStyle).AlignRight();//Obs do Pedido
                        }));
                    });
                }).GeneratePdf($@"{fileName}");

                using (var fileInput = new FileStream($@"{pathOrderToCupouns}\{pedido.number}.pdf", FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();
                    await fileInput.CopyToAsync(memoryStream);

                    var buffer = memoryStream.ToArray();
                    return Convert.ToBase64String(buffer);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma", Justification = "<Pendente>")]
        public async Task<string> PrintExchangeCupounToPrint(string serializedOrder)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathExchangeCupouns))
                    Directory.CreateDirectory(pathExchangeCupouns);

                var pedido = JsonConvert.DeserializeObject<Order>(serializedOrder);
                var fileName = $@"{pathExchangeCupouns}\{pedido.number}.pdf";
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

                var barcodeBitmap = writer.Write($"{pedido.number}"); //Código do Pedido
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
                                column.Item().Text($"{pedido.company.name_company}").Style(boldStyle).AlignLeft();//Nome Emp
                                column.Item().Text($"{pedido.company.address_company}, {pedido.company.street_number_company} ({pedido.company.complement_address_company})").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                                column.Item().Text($"{pedido.company.zip_code_company} - {pedido.company.neighborhood_company} - {pedido.company.city_company}/{pedido.company.uf_company}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                                column.Item().Text($"Tel: {pedido.company.fone_company}").Style(normalStyle).AlignLeft();//Telefone
                                column.Item().Text($"Cnpj: {pedido.company.doc_company} - Insc.Est. {pedido.company.state_registration_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                                column.Item().Text($"Email: {pedido.company.email_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            });
                        }));

                        page.Content().Element(x => x.Column(column =>
                        {
                            column.Spacing(1);
                            column.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().MinHeight(100).MaxWidth(100).Image(stream.ToArray());
                            column.Item().PaddingBottom(10).AlignCenter().Text($"{pedido.number}").Style(titleStyle).AlignCenter();//Código do Pedido

                            column.Item().Text($"{pedido.client.reason_client}").Style(boldStyle).AlignLeft();//Nome Cliente
                            column.Item().Text($"{pedido.client.address_client}, {pedido.client.street_number_client} {pedido.client.complement_address_client}").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                            column.Item().Text($"{pedido.client.zip_code_client} - {pedido.client.neighborhood_client} - {"CURITIBA"}/{"PR"}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                            column.Item().Text($"Tel: {pedido.client.fone_client}").Style(normalStyle).AlignLeft();//Telefone
                            column.Item().Text($"Doc: {pedido.client.doc_client} - Insc.Est. {pedido.client.state_registration_client}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            column.Item().Text($"Email: {pedido.client.email_client}").Style(normalStyle).AlignLeft();//CNPJ, I.E

                            column.Item().PaddingTop(10).Text($"Transportadora: {pedido.shippingCompany.cod_shippingCompany} - {pedido.shippingCompany.reason_shippingCompany}").Style(normalStyle).AlignLeft();//Código da Transportadora, Transportadora

                            column.Item().PaddingTop(10).Text($"OBS.\n{pedido.obs}").Style(normalStyle).AlignLeft();//Obs do Pedido

                            column.Item().PaddingTop(5).Element(container => container.Table(table =>
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

                            column.Item().PaddingTop(10).Text($"Total do Pedido: {Convert.ToDouble(pedido.amount.Replace(".", ",")).ToString("C")}").Style(normalStyle).AlignRight();//Obs do Pedido
                        }));
                    });
                }).GeneratePdf($@"{fileName}");

                using (var fileInput = new FileStream($@"{pathExchangeCupouns}\{pedido.number}.pdf", FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();
                    await fileInput.CopyToAsync(memoryStream);

                    var buffer = memoryStream.ToArray();
                    return Convert.ToBase64String(buffer);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
