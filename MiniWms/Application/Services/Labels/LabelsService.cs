using BloomersMiniWmsIntegrations.Domain.Entities.Labels;
using BloomersMiniWmsIntegrations.Infrastructure.Apis.Labels;
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using ZXing.Common;
using ZXing;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class LabelsService : ILabelsService
    {
        private readonly string path = @"C:\printer";
        private readonly string pathLabels = @"C:\printer\labels";
        private readonly string pathOrders = @"C:\printer\orders";
        private readonly string pathExchangeCupouns = @"C:\printer\cupouns";
        private readonly IAPICall _apiCall;
        private readonly ILabelsRepository _labelsRepository;

        public LabelsService(ILabelsRepository labelsRepository, IAPICall apiCall) =>
            (_labelsRepository, _apiCall) = (labelsRepository, apiCall);

        public async Task<string> GetLabelToPrint(string fileName)
        {
            try
            {
                string path = @$"C:\printer\labels\{fileName}";

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

        public async Task<string> GetOrdersToPresent(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _labelsRepository.GetOrdersToPresent(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetOrdersToPrint(string cnpj_emp, string serie, string data_inicial, string data_final)
        {
            var list = await _labelsRepository.GetOrdersToPrint(cnpj_emp, serie, data_inicial, data_final);
            return JsonConvert.SerializeObject(list);
        }

        public async Task<string> GetOrderToPrint(string cnpj_emp, string serie, string nr_pedido)
        {
            var list = await _labelsRepository.GetOrderToPrint(cnpj_emp, serie, nr_pedido);
            return JsonConvert.SerializeObject(list);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validar a compatibilidade da plataforma", Justification = "<Pendente>")]
        public async Task<string> PrintExchangeCupoun(string serializedOrder)
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

                var barcodeBitmap = writer.Write($"{pedido.token.Replace("Token: ", "").Replace("Token do Troca Fácil: ", "")}"); //Código do Pedido
                barcodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                QuestPDF.Settings.License = LicenseType.Community;

                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        var titleStyle = TextStyle.Default.FontSize(18).SemiBold().FontColor(Colors.Black);
                        var normalStyle = TextStyle.Default.FontSize(8).FontColor(Colors.Black);
                        var boldStyle = TextStyle.Default.FontSize(10).SemiBold().FontColor(Colors.Black);
                        var totalStyle = TextStyle.Default.FontSize(16).SemiBold().FontColor(Colors.Black);

                        page.Size(new PageSize((float)226.77, (float)850.39));
                        page.Margin(10);

                        page.Content().Element(x => x.Column(column =>
                        {
                            column.Spacing(1);
                            column.Item().Text($"Cupom de Troca").Style(titleStyle).AlignCenter();//Nome Emp
                            column.Item().Text($"Data: {DateTime.Now}").Style(normalStyle).AlignCenter();//Data em que o comprovante foi impresso

                            column.Item().Text($"{pedido.company.name_company}").Style(boldStyle).AlignLeft();//Nome Emp
                            column.Item().Text($"{pedido.company.address_company}, {pedido.company.street_number_company} ({pedido.company.complement_address_company})").Style(normalStyle).AlignLeft();//Rua, Numero, Complemento, Bairro
                            column.Item().Text($"{pedido.company.zip_code_company} - {pedido.company.neighborhood_company} - {pedido.company.city_company}/{pedido.company.uf_company}").Style(normalStyle).AlignLeft();//CEP, Cidade, UF
                            column.Item().Text($"Tel: {pedido.company.fone_company}").Style(normalStyle).AlignLeft();//Telefone
                            column.Item().Text($"Cnpj: {pedido.company.doc_company} - Insc.Est. {pedido.company.state_registration_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E
                            column.Item().Text($"Email: {pedido.company.email_company}").Style(normalStyle).AlignLeft();//CNPJ, I.E

                            column.Item().PaddingTop(10).PaddingBottom(10).AlignCenter().MinHeight(100).MaxWidth(100).Image(stream.ToArray());
                            column.Item().PaddingBottom(10).AlignCenter().Text($"{pedido.token.Replace("Token: ", "").Replace("Token do Troca Fácil: ", "")}").Style(boldStyle).AlignCenter();//Token do Troca Facil

                            column.Item().Text($"Cliente:").Style(normalStyle).AlignLeft();//Nome Cliente
                            column.Item().Text($"{pedido.client.reason_client}").Style(boldStyle).AlignLeft();

                            column.Item().PaddingTop(10).AlignLeft().Text($"NFe: {pedido.invoice.number_nf}").Style(normalStyle);//NFe
                            column.Item().Text($"Data: {pedido.invoice.date_emission_nf}").Style(normalStyle).AlignLeft();//Data em que a Nota foi Autorizada
                            column.Item().AlignLeft().Text($"Chave NFe:").Style(normalStyle);//Chave NFe
                            column.Item().AlignLeft().Text($"{pedido.invoice.key_nfe_nf}").Style(normalStyle);
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


        public async Task<bool> SendZPLToAPI(string zpl, string nr_pedido, string volume)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (!Directory.Exists(pathLabels))
                    Directory.CreateDirectory(pathLabels);
                if (!Directory.Exists(pathOrders))
                    Directory.CreateDirectory(pathOrders);

                byte[] encodedZpl = Encoding.UTF8.GetBytes(zpl);

                var request = _apiCall.CallAPI(encodedZpl, pathLabels, nr_pedido, true);

                if (request)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateFlagPrinted(string nr_pedido)
        {
            var result = await _labelsRepository.UpdatePrintedFlag(nr_pedido);
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
