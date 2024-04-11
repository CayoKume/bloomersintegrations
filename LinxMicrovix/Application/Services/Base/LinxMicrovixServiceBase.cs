namespace BloomersMicrovixIntegrations.LinxMicrovix.Application.Services.Base
{
    public interface ILinxMicrovixServiceBase<TEntity> where TEntity : class, new()
    {
        public Task IntegraRegistrosAsync(string tableName, string procName, string database);
        public void IntegraRegistrosNotAsync(string tableName, string procName, string database);
        public List<TEntity?> DeserializeResponse(List<Dictionary<string, string>> registros);
        public TEntity? TEntityToObject(TEntity entity);
    }
}
