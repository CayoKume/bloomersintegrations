using System;
using System.Net;
using System.Xml;

namespace BloomersMicrovixIntegrations.LinxMicrovixWsSaida.Infrastructure.Apis
{
    public class APICall : IAPICall
    {
        public string BuildBodyRequest(string parametersList, string? endPointName, string authentication, string key, string? cnpj)
        {
            return @$"<?xml version=""1.0"" encoding=""utf-8""?>
                          <LinxMicrovix>
                              <Authentication user=""{authentication}"" password=""{authentication}"" />
                              <ResponseFormat>xml</ResponseFormat>
                              <Command>
                                  <Name>{endPointName}</Name>
                                  <Parameters>
                                      <Parameter id=""chave"">{key}</Parameter>
                                      <Parameter id=""cnpjEmp"">{cnpj}</Parameter>
                                      {parametersList}
                                  </Parameters>
                              </Command>
                          </LinxMicrovix>";
        }

        public string BuildBodyRequest(string? endPointName, string authentication, string key)
        {
            return @$"<?xml version=""1.0"" encoding=""utf-8""?>
                          <LinxMicrovix>
                              <Authentication user=""{authentication}"" password=""{authentication}"" />
                              <ResponseFormat>xml</ResponseFormat>
                              <Command>
                                  <Name>{endPointName}</Name>
                                  <Parameters>
                                      <Parameter id=""chave"">{key}</Parameter>
                                  </Parameters>
                              </Command>
                          </LinxMicrovix>";
        }

        public async Task<string> CallAPIAsync(string? endPointName, string body)
        {
            try
            {
                var bytes = System.Text.Encoding.ASCII.GetBytes(body);
                var request = CreateClient(endPointName, bytes);

                Stream stream = await request.GetRequestStreamAsync();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                var response = (HttpWebResponse) await request.GetResponseAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    return new StreamReader(response.GetResponseStream()).ReadToEnd().Replace("'", "");
                else
                    return String.Empty;
            }
            catch (Exception ex) when (ex.Message.Contains(" - "))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"{endPointName} - CallAPIAsync - Erro ao consultar end-point {endPointName} na microvix - {ex.Message}");
            }
        }

        public string CallAPINotAsync(string endPointName, string body)
        {
            try
            {
                var bytes = System.Text.Encoding.ASCII.GetBytes(body);
                var request = CreateClient(endPointName, bytes);

                Stream stream = request.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return new StreamReader(response.GetResponseStream()).ReadToEnd().Replace("'", "");
                else
                    return String.Empty;
            }
            catch (Exception ex) when (ex.Message.Contains(" - "))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"{endPointName} - CallAPINotAsync - Erro ao consultar end-point {endPointName} na microvix - {ex.Message}");
            }
        }

        public HttpWebRequest CreateClient(string route, byte[] bytes)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://webapi.microvix.com.br/1.0/api/integracao/{route}");
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                request.Timeout = 120 * 1000;

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception($"{route} - CreateClient - Erro ao criar request para o end-point: {route}, atraves da URI: https://webapi.microvix.com.br/1.0/api/integracao/{route} - {ex.Message}");
            }
        }

        public List<Dictionary<string, string>> DeserializeXML(string response)
        {
            try
            {
                var listRegistros = new List<Dictionary<string, string>>();
                var xml = new XmlDocument();

                if (response != string.Empty)
                {
                    xml.LoadXml(response);

                    if (xml.GetElementsByTagName("ResponseSuccess")[0].ChildNodes[0].InnerText == "False")
                        throw new Exception($"{response} - DeserializeXML - Erro ao deserealizar XML - {$"{xml.GetElementsByTagName("Message")[0].ChildNodes[0].InnerText}"}");

                    if (xml.GetElementsByTagName("R").Count > 0)
                    {
                        Parallel.For(0, xml.GetElementsByTagName("R").Count, row =>
                        {
                            var registro = new Dictionary<string, string>();
                            var c0 = xml.GetElementsByTagName("C")[0];
                            var rrow = xml.GetElementsByTagName("R")[row];
                            for (int col = 0; col < c0.ChildNodes.Count; col++)
                            {
                                string? key = c0.ChildNodes[col].InnerText;
                                string? value = rrow.ChildNodes[col].InnerText.Replace("'", "''");

                                registro.Add(key, value);
                            }
                            listRegistros.Add(registro);
                        });

                        return listRegistros;
                    }
                    else
                        return listRegistros;
                }
                return listRegistros;
            }
            catch (Exception ex) when (ex.Message.Contains(" - "))
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"{response} - DeserializeXML - Erro ao deserealizar XML - {ex.Message}");
            }
        }
    }
}
