using System.Net;

namespace BloomersMiniWmsIntegrations.Infrastructure.Apis.Labels
{
    public class APICall : IAPICall
    {
        public bool CallAPI(byte[] zpl, string path, string number, bool typeLabel)
        {
            HttpWebRequest client;

            if (typeLabel)
                client = CreateClientSL4504x6(zpl.Length);
            else
                client = CreateClientPinaco6288(zpl.Length);

            var requestStream = client.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();

            var response = (HttpWebResponse)client.GetResponse();
            var responseStream = response.GetResponseStream();
            var fileStream = File.Create($@"{path}\{number}.pdf");
            responseStream.CopyTo(fileStream);
            responseStream.Close();
            fileStream.Close();

            return true;
        }

        private HttpWebRequest CreateClientPinaco6288(long zplLength)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.labelary.com/v1/printers/8dpmm/labels/4x5.5/0/");
                request.Method = "POST";
                request.Accept = "application/pdf";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zplLength;

                request.Headers.Add("X-Page-Size", "Letter");
                request.Headers.Add("X-Page-Orientation", "Portrait");
                request.Headers.Add("X-Page-Layout", "2x1");
                request.Headers.Add("X-Page-Align", "Center");
                request.Headers.Add("X-Page-Vertical-Align", "Top");

                request.Timeout = 15 * 1000;

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - CreateClient - Erro ao criar request para, atraves da URI: http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/ - {ex.Message}");
            }
        }

        private HttpWebRequest CreateClientSL4504x6(long zplLength)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
                request.Accept = "application/pdf";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zplLength;
                request.Method = "POST";
                request.Timeout = 15 * 1000;

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception($"MiniWms [Labels] - CreateClient - Erro ao criar request para, atraves da URI: http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/ - {ex.Message}");
            }
        }
    }
}
