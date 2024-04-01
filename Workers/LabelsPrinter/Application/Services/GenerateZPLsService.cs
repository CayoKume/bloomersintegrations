using BloomersWorkers.LabelsPrinter.Domain.Entities;
using System.Text;

namespace BloomersWorkers.LabelsPrinter.Application.Services
{
    public class GenerateZPLsService : IGenerateZPLsService
    {
        public List<byte[]> GenerateDanfeBodyRequest(Order order)
        {
            try
            {
                List<byte[]> requests = new List<byte[]>();

                for (int i = 0; i < order.volumes; i++)
                {
                    string zpl = @$"^XA
                                    ~TA000
                                    ~JSN
                                    ^LT0
                                    ^MNW
                                    ^MTT
                                    ^PON
                                    ^PMN
                                    ^LH0,0
                                    ^JMA
                                    ^PR4,4
                                    ~SD30
                                    ^JUS
                                    ^LRN
                                    ^CI27
                                    ^PA0,1,1,0
                                    ^XZ
                                    ^XA
                                    ^MMT
                                    ^PW799
                                    ^LL1180
                                    ^LS0

                                    ^FX DRAWLINES
                                    ^FO25,90^GB751,0,3^FS
                                    ^FO31,451^GB745,0,3^FS
                                    ^FO151,453^GB0,57,3^FS
                                    ^FO31,508^GB745,0,3^FS
                                    ^FO272,453^GB0,60,3^FS
                                    ^FO413,453^GB0,57,3^FS
                                    ^FO592,453^GB0,57,3^FS
                                    ^FO31,584^GB745,0,3^FS
                                    ^FO31,840^GB745,0,3^FS
                                    ^FO31,1085^GB745,0,3^FS
                                    ^FO25,292^GB751,0,3^FS
                                    ^FO31,925^GB745,0,3^FS

                                    ^FX HEADER
                                    ^FT209,59^A0N,25,25^FH\^CI28^FDDANFE SIMPLIFICADO - MISHA^FS^CI27
                                    ^FO25,114^GFA,1321,7128,44,:Z64:eJztWEuu4jAQ7FggWVkFadhHs3rKKXIEkOA+LVaIU6BZRTnluH+2CTEEDWg29JM/cZtKpV67bRlg3s4IWHD9m7lx+dxqv3QmAoy4HPi4cJ4LpVsOC9uF8zy8xncpLtkrfDev4OLyuUv5tvAZvoT7CX0J9618feo+1XeVuk/5KlgLC/hmJJ/ijgn3qb7H/mVcslf4PtVXdfDwZn1daj6y3vBD+QHfwdcjVef0zN2gr4114xCdfawym+HbQU3UHNSE7q5cXCsOR2Okdk1tRbQqsKoUv+wMMiIhBTziNGobynhC6YO8K9ghcqXeNvR2wHHBuL8MeMe49L8fLAC6K8NQv6Ngq20uv4C3scaqjVJrlHplc3nGSBoKRuBXX+XbQzDQK3jMg75U+DKtNZWeG9rcmO+6V1ye0bUlXGrp+wUXTcZNxE06C98mF57DlH8VMKnUissON5gTQbfdyG3Kt9qJ5kKcf8Xf7BRXP7uLuFd9qRARbj0XC4mN4W56dQotZlMPia+fxRW+Rsn4wjxfMFzjizKXHRG3i3wFQ8otrum7jvrWub66tiQ/DOq8EO7hju9uwpct8b3RVw9mUQfmPadvOphFfcGcM/pqMuiML5lP+q4zvtXx8JBvrq8baTFn+gJnjyvk8VupjNtNxrcxvlHGXF8YRx6L+rKbce/0hfWRd7kl+hLhAXJ9zXEfv+Fpv7vRtyrqGyomnPQdoRS/NEaEE99dUV/QPB71Zc6Jbx6/VG+bZfHLg5YfIH7DvL70fCivtxt9WQnM1rG99D5+ubvP84M55/SFlCd544i4d/HLFgSOfA9xxpy+/AFRX15/hfhVOWP8/jLnvL7URn0T31l9qS3xnepL/ahv4jsTv5DycMCVpfFA34CX9I24c/HLMMa3eqavS/q6Cd9J/HJr8Sspohy/hJv0jbjz+lav6IsTfdN+MY1famP87s1Z0LfOcalzKeu7UkdlC2/CV3WwY8jNenOT+F1bUoDsvNOotCl+OZPXskfyuSSG7SjvGyQeeqvWmguo39vYQbToia9jSNlyByk/IrW8a+SbCIc0Uu3p55Jwd1JUYRnb69cQOO9mIeGCJjHuXAZ1hIyJ2k0n4K997Wtf+9rXvvZZ8w5/uOPOCKD3RgVbxYuX1YNZYrUhOb75cw8nVw+9ap7v0WqPHjB0nA8jPtS4ELfE+Tezc4KDdIN2AnT+AeOqkpsnPvgW2f+BM534EL0n4u4MJ8RQn2fm9hOWxrW5nxoEbQn3ii1Ai6Sv8G0fECagBiYXZxM7B1nDme+CP470rU/h8ezCXxn3vxq+F+4vgfk6hA==:9633
                                    ^FT379,137^A0N,23,15^FH\^CI28^FD{order.company.reason_company.ToUpper()}^FS^CI27
                                    ^FT379,166^A0N,23,15^FH\^CI28^FDCNPJ: {Convert.ToUInt64(order.company.doc_company)}^FS^CI27
                                    ^FT379,195^A0N,23,15^FH\^CI28^FDI.E: {Convert.ToUInt64(order.company.state_registration_company).ToString(@"00\.000\.00\-0")}^FS^CI27
                                    ^FT379,224^A0N,23,15^FH\^CI28^FD{order.company.address_company.ToUpper()}, {order.company.street_number_company.ToUpper()} - {order.company.complement_address_company.ToUpper()} - {order.company.neighborhood_company.ToUpper()}^FS^CI27
                                    ^FT379,253^A0N,23,15^FH\^CI28^FDCEP: {order.company.zip_code_company} - {order.company.city_company.ToUpper()} - {order.company.uf_company.ToUpper()}^FS^CI27
                                    ^FX
                                    ^BY2,3,10
                                    ^FO115,315,0^BCN,100,Y,N,N,A^FD{order.invoice.key_nfe_nf}^FS

                                    ^FX
                                    ^FT39,472^A0N,14,15^FH\^CI28^FDOPERAÇÃO^FS^CI27
                                    ^FT44,499^A0N,23,23^FH\^CI28^FD 1-SAÍDA ^FS^CI27

                                    ^FX
                                    ^FT160,472^A0N,14,15^FH\^CI28^FDSÉRIE^FS^CI27
                                    ^FT213,499^A0N,23,23^FH\^CI28^FD{order.invoice.serie_nf}^FS^CI27

                                    ^FX
                                    ^FT283,472^A0N,14,15^FH\^CI28^FDNÚMERO^FS^CI27
                                    ^FT315,499^A0N,23,23^FH\^CI28^FD{order.invoice.number_nf}^FS^CI27

                                    ^FX
                                    ^FT603,472^A0N,14,15^FH\^CI28^FDVALOR TOTAL^FS^CI27
                                    ^FT636,499^A0N,23,23^FH\^CI28^FD{order.invoice.amount_nf.ToString("C")}^FS^CI27

                                    ^FX
                                    ^FT39,533^A0N,14,15^FH\^CI28^FDPROTOCOLO DE AUTORIZAÇÃO DE USO^FS^CI27
                                    ^FT445,499^A0N,23,23^FH\^CI28^FD{order.dateProt_order.Date.ToString("dd/MM/yyyy")}^FS^CI27
                                    ^FT160,567^A0N,23,23^FH\^CI28^FD{order.nProt_order}^FS^CI27
                                    ^FT518,567^A0N,23,23^FH\^CI28^FD{order.dateProt_order.TimeOfDay}^FS^CI27

                                    ^FX
                                    ^FT39,606^A0N,14,15^FH\^CI28^FDDESTINATÁRIO^FS^CI27
                                    ^FT39,639^A0N,23,23^FH\^CI28^FD{order.client.reason_client.ToUpper()}^FS^CI27
                                    ^FT39,668^A0N,23,23^FH\^CI28^FDCNPJ/CPF: {order.client.doc_client}^FS^CI27
                                    ^FT39,697^A0N,23,23^FH\^CI28^FDI.E: {order.client.state_registration_client}^FS^CI27
                                    ^FT39,726^A0N,23,23^FH\^CI28^FD{order.client.address_client.ToUpper()}, {order.client.street_number_client} - {order.client.complement_address_client.ToUpper()}^FS^CI27
                                    ^FT39,755^A0N,23,23^FH\^CI28^FD{order.client.neighborhood_client.ToUpper()}^FS^CI27
                                    ^FT39,784^A0N,23,23^FH\^CI28^FDCEP: {Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000")}^FS^CI27
                                    ^FT39,813^A0N,23,23^FH\^CI28^FD{order.client.city_client.ToUpper()} - {order.client.uf_client.ToUpper()}^FS^CI27

                                    ^FX
                                    ^FT39,866^A0N,14,15^FH\^CI28^FDTRANSPORTADORA^FS^CI27
                                    ^FT39,904^A0N,23,25^FH\^CI28^FD{order.shippingCompany.reason_shippingCompany.ToUpper()}^FS^CI27

                                    ^FX
                                    ^FT39,953^A0N,14,15^FH\^CI28^FDDADOS ADICIONAIS^FS^CI27
                                    ^FT39,993^A0N,25,25^FH\^CI28^FDPedido: {order.number.ToUpper()}^FS^CI27
                                    ^FT39,1024^A0N,25,25^FH\^CI28^FDVolume: {i + 1}/{order.volumes}^FS^CI27
                                    ^FT39,1055^A0N,25,25^FH\^CI28^FDContato: {order.client.reason_client} - Telefone: {order.client.fone_client}^FS^CI27

                                    ^FT424,472^A0N,14,15^FH\^CI28^FDEMISSÃO^FS^CI27
                                    ^FT369,567^A0N,23,23^FH\^CI28^FD{order.dateProt_order.Date.ToString("dd/MM/yyyy")}^FS^CI27

                                    ^PQ1,0,1,Y
                                    ^XZ";
                    byte[] danfe = Encoding.UTF8.GetBytes(zpl);
                    order.zpl.Add(zpl);
                    requests.Add(danfe);
                }
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception($@"GenerateDanfeBodyRequest - Erro ao gerar etiqueta danfe do pedido: {order.number.Trim()} atraves do zpl - {ex.Message}");
            }
        }

        public List<byte[]> GenerateAWBBodyRequest(Order order)
        {
            try
            {
                List<byte[]> requests = new List<byte[]>();

                for (int i = 0; i < order.awb.Count(); i++)
                {
                    string zpl = @$"^XA
                                    ~TA000
                                    ~JSN
                                    ^LT0
                                    ^MNW
                                    ^MTT
                                    ^PON
                                    ^PMN
                                    ^LH0,0
                                    ^JMA
                                    ^PR4,4
                                    ~SD30
                                    ^JUS
                                    ^LRN
                                    ^CI27
                                    ^PA0,1,1,0
                                    ^XZ
                                    ^XA
                                    ^MMT
                                    ^PW799
                                    ^LL1180
                                    ^LS0

                                    ^FX DRAWLINES
                                    ^FO589,64^GB174,67,2^FS
                                    ^FO13,462^GB769,0,3^FS
                                    ^FO13,696^GB769,0,3^FS
                                    ^FO13,973^GB769,0,3^FS
                                    ^FO589,195^GB174,67,2^FS

                                    ^FX HEADER
                                    ^FT589,49^A0N,25,25^FH\^CI28^FDTOTAL EXPRESS^FS^CI27
                                    ^FT633,115^A0N,51,51^FH\^CI28^FD^FS^CI27
                                    ^FT633,115^A0N,51,51^FH\^CI28^FD{order.shippingCompany.metodo_shippingCompany}^FS^CI27

                                    ^FT18,47^A0N,23,23^FH\^CI28^FDRemetente^FS^CI27
                                    ^FT30,93^A0N,23,20^FH\^CI28^FD{order.company.reason_company.ToUpper()}^FS^CI27
                                    ^FT30,122^A0N,23,20^FH\^CI28^FDCNPJ: {Convert.ToUInt64(order.company.doc_company).ToString(@"00\.000\.000\/0000\-00")}^FS^CI27
                                    ^FT30,151^A0N,23,20^FH\^CI28^FDI.E: {Convert.ToUInt64(order.company.state_registration_company).ToString(@"00\.000\.00\-0")}^FS^CI27
                                    ^FT30,180^A0N,23,20^FH\^CI28^FD{order.company.address_company.ToUpper()}, {order.company.street_number_company.ToUpper()} - {order.company.complement_address_company.ToUpper()} - {order.company.neighborhood_company.ToUpper()}^FS^CI27
                                    ^FT30,209^A0N,23,20^FH\^CI28^FD{order.company.zip_code_company} - {order.company.city_company.ToUpper()} - {order.company.uf_company.ToUpper()}^FS^CI27

                                    ^FT18,259^A0N,23,23^FH\^CI28^FDDestinatário^FS^CI27
                                    ^FT30,300^A0N,23,20^FH\^CI28^FD{order.client.reason_client.ToUpper()}^FS^CI27
                                    ^FT30,329^A0N,23,20^FH\^CI28^FDCNPJ/CPF: {order.client.doc_client}^FS^CI27
                                    ^FT30,358^A0N,23,20^FH\^CI28^FDI.E: {order.client.state_registration_client}^FS^CI27
                                    ^FT30,387^A0N,23,20^FH\^CI28^FD{order.client.address_client.ToUpper()}, {order.client.street_number_client} - {order.client.complement_address_client.ToUpper()}^FS^CI27
                                    ^FT30,416^A0N,23,20^FH\^CI28^FD{order.client.neighborhood_client.ToUpper()}^FS^CI27
                                    ^FT30,445^A0N,23,20^FH\^CI28^FDCEP: {Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000")} - {order.client.city_client.ToUpper()} - {order.client.uf_client.ToUpper()}^FS^CI27

                                    ^FX
                                    ^BY3,3,139^FT30,628^BCN,,N,N
                                    ^FH\^FD>;{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000").Substring(0, 4)}>6{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000").Substring(5)}^FS
                                    ^FT111,675^A0N,42,43^FH\^CI28^FD{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000")}^FS^CI27
                                    ^FT473,673^A0N,34,23^FH\^CI28^FD{order.rote}^FS^CI27

                                    ^FX
                                    ^BY4,3,10
                                    ^FO42,715,0^BCN,200,Y,N,N,A^FD{order.awb[i]}^FS

                                    ^FX
                                    ^BY3,4,10
                                    ^FO190,1005,0^BCN,100,Y,N,N,A^FD{order.number.ToUpper()}^FS

                                    ^FX
                                    ^FT589,176^A0N,25,25^FH\^CI28^FDVOLUMES^FS^CI27
                                    ^FT629,246^A0N,51,51^FH\^CI28^FD{i + 1}/{order.awb.Count()}^FS^CI27

                                    ^FT553,649^BQN,2,7
                                    ^FH\^FDLA,{order.awb[i]}^FS

                                    ^PQ1,0,1,Y
                                    ^XZ";

                    byte[] awb = Encoding.UTF8.GetBytes(zpl);
                    order.zpl.Add(zpl);
                    requests.Add(awb);
                }
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception(@$"GenerateAWBBodyRequest - Erro ao gerar etiqueta awb do pedido: {order.number.Trim()} atraves do zpl - {ex.Message}");
            }
        }

        public List<byte[]> GenerateAWBAWRBodyRequest(Order order)
        {
            try
            {
                List<byte[]> requests = new List<byte[]>();

                for (int i = 0; i < order.volumes; i++)
                {
                    string zpl = @$"^XA
                                    ~TA000
                                    ~JSN
                                    ^LT0
                                    ^MNW
                                    ^MTT
                                    ^PON
                                    ^PMN
                                    ^LH0,0
                                    ^JMA
                                    ^PR4,4
                                    ~SD30
                                    ^JUS
                                    ^LRN
                                    ^CI27
                                    ^PA0,1,1,0
                                    ^XZ
                                    ^XA
                                    ^MMT
                                    ^PW799
                                    ^LL1180
                                    ^LS0

                                    ^FX DRAWLINES
                                    ^FO589,64^GB174,67,2^FS
                                    ^FO13,462^GB769,0,3^FS
                                    ^FO13,696^GB769,0,3^FS
                                    ^FO13,973^GB769,0,3^FS
                                    ^FO589,195^GB174,67,2^FS

                                    ^FX HEADER
                                    ^FT589,49^A0N,25,25^FH\^CI28^FDAWR^FS^CI27
                                    ^FT633,115^A0N,51,51^FH\^CI28^FD^FS^CI27
                                    ^FT620,115^A0N,51,51^FH\^CI28^FD{order.shippingCompany.metodo_shippingCompany}^FS^CI27

                                    ^FT18,47^A0N,23,23^FH\^CI28^FDRemetente^FS^CI27
                                    ^FT30,93^A0N,23,20^FH\^CI28^FD{order.company.reason_company.ToUpper()}^FS^CI27
                                    ^FT30,122^A0N,23,20^FH\^CI28^FDCNPJ: {Convert.ToUInt64(order.company.doc_company).ToString(@"00\.000\.000\/0000\-00")}^FS^CI27
                                    ^FT30,151^A0N,23,20^FH\^CI28^FDI.E: {Convert.ToUInt64(order.company.state_registration_company).ToString(@"00\.000\.00\-0")}^FS^CI27
                                    ^FT30,180^A0N,23,20^FH\^CI28^FD{order.company.address_company.ToUpper()}, {order.company.street_number_company.ToUpper()} - {order.company.complement_address_company.ToUpper()} - {order.company.neighborhood_company.ToUpper()}^FS^CI27
                                    ^FT30,209^A0N,23,20^FH\^CI28^FD{order.company.zip_code_company} - {order.company.city_company.ToUpper()} - {order.company.uf_company.ToUpper()}^FS^CI27

                                    ^FT18,259^A0N,23,23^FH\^CI28^FDDestinatário^FS^CI27
                                    ^FT30,300^A0N,23,20^FH\^CI28^FD{order.client.reason_client.ToUpper()}^FS^CI27
                                    ^FT30,329^A0N,23,20^FH\^CI28^FDCNPJ/CPF: {order.client.doc_client}^FS^CI27
                                    ^FT30,358^A0N,23,20^FH\^CI28^FDI.E: {order.client.state_registration_client}^FS^CI27
                                    ^FT30,387^A0N,23,20^FH\^CI28^FD{order.client.address_client.ToUpper()}, {order.client.street_number_client} - {order.client.complement_address_client.ToUpper()}^FS^CI27
                                    ^FT30,416^A0N,23,20^FH\^CI28^FD{order.client.neighborhood_client.ToUpper()}^FS^CI27
                                    ^FT30,445^A0N,23,20^FH\^CI28^FDCEP: {Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000")} - {order.client.city_client.ToUpper()} - {order.client.uf_client.ToUpper()}^FS^CI27

                                    ^FX
                                    ^BY3,3,139^FT30,628^BCN,,N,N
                                    ^FH\^FD>;{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000").Substring(0, 4)}>6{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000").Substring(5)}^FS
                                    ^FT111,675^A0N,42,43^FH\^CI28^FD{Convert.ToInt64(order.client.zip_code_client).ToString(@"00000\-000")}^FS^CI27

                                    ^FX
                                    ^BY4,3,10
                                    ^FO120,715,0^BCN,200,Y,N,N,A^FD000000000000000^FS

                                    ^FX
                                    ^BY3,4,10
                                    ^FO190,1005,0^BCN,100,Y,N,N,A^FD{order.number.ToUpper()}^FS

                                    ^FX
                                    ^FT589,176^A0N,25,25^FH\^CI28^FDVOLUMES^FS^CI27
                                    ^FT629,246^A0N,51,51^FH\^CI28^FD{i + 1}/{order.volumes}^FS^CI27

                                    ^FT553,715^BQN,2,7
                                    ^FH\^FDLA,(0)00-000-00-000-[000]^FS

                                    ^PQ1,0,1,Y
                                    ^XZ";

                    byte[] awb = Encoding.UTF8.GetBytes(zpl);
                    order.zpl.Add(zpl);
                    requests.Add(awb);
                }
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception($"GenerateAWBAWRBodyRequest - Erro ao gerar etiqueta awb do pedido: {order.number.Trim()} atraves do zpl - {ex.Message}");
            }
        }
    }
}
