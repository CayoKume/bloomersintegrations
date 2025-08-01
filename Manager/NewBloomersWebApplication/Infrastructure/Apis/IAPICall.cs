﻿namespace NewBloomersWebApplication.Infrastructure.Apis
{
    public interface IAPICall
    {
        public Task<string> GetAsync(string route, string encodedParameters);
        public Task<string> GetAsync(string route);
        public Task<bool> PutAsync(string route, string jsonContent);
        public Task<string> PostAsync(string route, string jsonContent);
    }
}
