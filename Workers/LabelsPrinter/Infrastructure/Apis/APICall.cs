using System.Net;

namespace BloomersWorkers.LabelsPrinter.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        public async Task<bool> SendRequest(byte[] zpl, string path, string nr_pedido)
        {
            try
            {
                var request = CreateClient(zpl, "4", "6");
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                var fileStream = File.Create($@"{path}\{nr_pedido}.pdf");
                responseStream.CopyTo(fileStream);
                responseStream.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private HttpWebRequest CreateClient(byte[] zpl, string labelWidth, string labelHeigth)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://api.labelary.com/v1/printers/8dpmm/labels/{labelWidth}x{labelHeigth}/0/");
            request.Method = "POST";
            request.Accept = "application/pdf";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(zpl, 0, zpl.Length);
            requestStream.Close();
            return request;
        }
    }
}
