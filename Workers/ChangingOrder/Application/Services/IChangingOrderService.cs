﻿namespace BloomersWorkers.ChangingOrder.Application.Services
{
    public interface IChangingOrderService
    {
        public Task ChangingOrder();
        public Task ChangingOrder(string workerName);
    }
}
