
using BloomersMiniWmsIntegrations.Infrastructure.Repositorys;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BloomersMiniWmsIntegrations.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository) =>
            (_attendanceRepository) = (attendanceRepository);

        public async Task<string> GetOrdersToContact(string serie, string doc_company)
        {
            try
            {
                var orders = await _attendanceRepository.GetOrdersToContact(serie, doc_company);
                return JsonConvert.SerializeObject(orders);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateDateContacted(string number, string atendente, string obs)
        {
            try
            {
                return await _attendanceRepository.UpdateDateContacted(number, atendente, obs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
