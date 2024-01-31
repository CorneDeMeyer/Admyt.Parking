﻿namespace Parking.Domain.Config
{
    public class AzureServiceBusConfiguration(string connectionString, string queueName)
    {
        public string ConnectionString { get; private set; } = connectionString;
        public string QueueName { get; private set; } = queueName;
        public bool Enabled { get; private set; } = true;   
    }
}
