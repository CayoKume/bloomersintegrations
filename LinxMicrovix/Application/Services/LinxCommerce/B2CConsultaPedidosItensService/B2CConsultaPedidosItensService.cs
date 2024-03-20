using BloomersMicrovixIntegrations.Repositorys.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Core.Biz;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Models.Ecommerce;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Repositorys.Interfaces;
using BloomersMicrovixIntegrations.Saida.Ecommerce.Services.Interfaces;

namespace BloomersMicrovixIntegrations.Saida.Ecommerce.Services
{
    public class B2CConsultaPedidosItensService<T1> : IB2CConsultaPedidosItensService<T1> where T1 : B2CConsultaPedidosItens, new()
    {
        private string PARAMETERS = string.Empty;
        private string CHAVE = LinxAPIAttributes.TypeEnum.chaveB2C.ToName();
        private string AUTENTIFICACAO = LinxAPIAttributes.TypeEnum.authenticationB2C.ToName();
        private readonly IB2CConsultaPedidosItensRepository<B2CConsultaPedidosItens> _b2CConsultaPedidosItensRepository;

        public B2CConsultaPedidosItensService(IB2CConsultaPedidosItensRepository<B2CConsultaPedidosItens> b2CConsultaPedidosItensRepository) =>
            _b2CConsultaPedidosItensRepository = b2CConsultaPedidosItensRepository;

        public List<T1?> DeserializeResponse(List<Dictionary<string, string>> registros)
        {
            decimal vl_unitario;
            long timestamp, codigoproduto;
            int id_pedido_item, id_pedido, quantidade, portal;

            var list = new List<T1>();

            for (int i = 0; i < registros.Count(); i++)
            {
                try
                {
                    if (long.TryParse(registros[i].Where(pair => pair.Key == "timestamp").Select(pair => pair.Value).First(), out long result))
                        timestamp = result;
                    else
                        timestamp = 0;

                    if (long.TryParse(registros[i].Where(pair => pair.Key == "codigoproduto").Select(pair => pair.Value).First(), out long result_0))
                        codigoproduto = result_0;
                    else
                        codigoproduto = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First(), out int result_1))
                        id_pedido_item = result_1;
                    else
                        id_pedido_item = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "id_pedido").Select(pair => pair.Value).First(), out int result_2))
                        id_pedido = result_2;
                    else
                        id_pedido = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "quantidade").Select(pair => pair.Value).First(), out int result_3))
                        quantidade = result_3;
                    else
                        quantidade = 0;

                    if (int.TryParse(registros[i].Where(pair => pair.Key == "portal").Select(pair => pair.Value).First(), out int result_4))
                        portal = result_4;
                    else
                        portal = 0;

                    if (decimal.TryParse(registros[i].Where(pair => pair.Key == "vl_unitario").Select(pair => pair.Value).First(), out decimal result_5))
                        vl_unitario = result_5;
                    else
                        vl_unitario = 0;

                    list.Add(new T1
                    {
                        lastupdateon = DateTime.Now,
                        id_pedido_item = id_pedido_item,
                        id_pedido = id_pedido,
                        codigoproduto = codigoproduto,
                        quantidade = quantidade,
                        vl_unitario = vl_unitario,
                        timestamp = timestamp,
                        portal = portal
                    });
                }
                catch (Exception ex)
                {
                    var registroComErro = registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First() == String.Empty ? "0" : registros[i].Where(pair => pair.Key == "id_pedido_item").Select(pair => pair.Value).First();
                    throw new Exception($"B2CConsultaPedidosItens - DeserializeResponse - Erro ao deserealizar registro: {registroComErro} - {ex.Message}");
                }
            }

            return list;
        }

        public async Task IntegraRegistros(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosItensRepository.GetParameters(tableName, "parameters_lastday");

                var timestamp = await _b2CConsultaPedidosItensRepository.GetLastTimestampPedidosItens();
                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    var _listResults = listResults.ConvertAll(new Converter<T1, B2CConsultaPedidosItens>(T1ToObject));
                    var __listResults = await _b2CConsultaPedidosItensRepository.GetRegistersExists(_listResults, tableName, database);

                    for (int i = 0; i < __listResults.Count; i++)
                    {
                        _listResults.Remove(_listResults.Where(r => r.id_pedido_item == Convert.ToInt64(__listResults[i].id_pedido_item) && r.timestamp == __listResults[i].timestamp).FirstOrDefault());
                    }

                    if (_listResults.Count() > 0)
                    {
                        _b2CConsultaPedidosItensRepository.BulkInsertIntoTableRaw(_listResults, tableName, database);
                        //await _b2CConsultaPedidosItensRepository.CallDbProcMerge(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void IntegraRegistrosSync(string tableName, string procName, string database)
        {
            try
            {
                PARAMETERS = _b2CConsultaPedidosItensRepository.GetParametersSync(tableName, "parameters_lastday");

                var timestamp = _b2CConsultaPedidosItensRepository.GetLastTimestampPedidosItensSync();
                var response = APICaller.CallLinxAPI(PARAMETERS.Replace("[0]", timestamp), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);

                if (registros.Count() > 0)
                {
                    var listResults = DeserializeResponse(registros);
                    if (listResults.Count() > 0)
                    {
                        var list = listResults.ConvertAll(new Converter<T1, B2CConsultaPedidosItens>(T1ToObject));
                        _b2CConsultaPedidosItensRepository.BulkInsertIntoTableRaw(list, tableName, database);
                        //_b2CConsultaPedidosItensRepository.CallDbProcMergeSync(procName, tableName, database);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IntegraRegistrosIndividual(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = await _b2CConsultaPedidosItensRepository.GetParameters(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro is not null)
                {
                    await _b2CConsultaPedidosItensRepository.InsereRegistroIndividual(registro[0], tableName, database);
                    //await _b2CConsultaPedidosItensRepository.CallDbProcMerge(procName, tableName, database);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public bool IntegraRegistrosIndividualSync(string tableName, string procName, string database, string identificador)
        {
            try
            {
                PARAMETERS = _b2CConsultaPedidosItensRepository.GetParametersSync(tableName, "parameters_manual");

                string response = APICaller.CallLinxAPI(PARAMETERS.Replace("[id_pedido]", $"{identificador}").Replace("[0]", "0"), tableName, AUTENTIFICACAO, CHAVE, "38367316000199");
                var registros = APICaller.DeserializeXML(response);
                var registro = DeserializeResponse(registros);

                if (registro is not null)
                {
                    _b2CConsultaPedidosItensRepository.InsereRegistroIndividualSync(registro[0], tableName, database);
                    //_b2CConsultaPedidosItensRepository.CallDbProcMergeSync(procName, tableName, database);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public T1? T1ToObject(T1 t1)
        {
            try
            {
                return new T1
                {
                    lastupdateon = t1.lastupdateon,
                    id_pedido_item = t1.id_pedido_item,
                    id_pedido = t1.id_pedido,
                    codigoproduto = t1.codigoproduto,
                    quantidade = t1.quantidade,
                    vl_unitario = t1.vl_unitario,
                    timestamp = t1.timestamp,
                    portal = t1.portal
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"B2CConsultaPedidosItens - T1ToObject - Erro ao converter registro: {t1.id_pedido_item} para objeto - {ex.Message}");
            }
        }  
    }
}
