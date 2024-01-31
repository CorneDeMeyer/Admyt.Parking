using Parking.DomainLogic.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Parking.DomainLogic.Service;
using Azure.Messaging.ServiceBus;
using Parking.DomainLogic.Hubs;
using Parking.Domain.Config;
using Parking.Domain.Models;

namespace Parking.DomainLogic.ServiceBus
{
    public class GateEventSubscriptionBackgroundService : BackgroundService
    {
        private readonly IHubContext<CommunicationHub> _signalRCommunication;
        private AzureServiceBusConfiguration _config;
        private readonly IGateService _gateService;
        private readonly ILogger _logger;

        private const string hubMethod = "gate-events-hub-response";
        private ServiceBusProcessor? processor = null;
        private ServiceBusClient? client = null;

        public GateEventSubscriptionBackgroundService(AzureServiceBusConfiguration config, IGateService gateService, 
            IHubContext<CommunicationHub> signalRCommunication, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GateService>();
            _signalRCommunication = signalRCommunication;
            _gateService = gateService;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient(_config.ConnectionString, clientOptions);
            processor = client.CreateProcessor(_config.QueueName, new ServiceBusProcessorOptions());

            try
            {
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // Start Processing 
                await processor.StartProcessingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Gate Event Service Bus Background Service: ");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, $"GateEventSubscriptionBackgroundService.ErrorHandler: {args.ErrorSource.ToString()}");
            return Task.CompletedTask;
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            // Ensure we not processing Cancellation Request Token
            if (!args.CancellationToken.IsCancellationRequested)
            {
                switch (args.Message.Subject)
                {
                    case "gate-events":
                        var request = args.Message.Body.ToObjectFromJson<GateEvent>();
                        var response = await _gateService.ProcessGateRequest(request);
                        if (response.Errors.Count > 0)
                        {
                            LogGateEventErrors(request, response);
                        }

                        await _signalRCommunication.Clients.All.SendAsync(hubMethod,
                                new GateEventHubModel(request, response.Value, response.Errors));

                        break;
                    default:
                        break;
                }
            }
        }
        private void LogGateEventErrors(GateEvent request, GateEventResponse<Guid> response)
        {
            _logger.LogInformation($"Request: (Gate={request.GateId.ToString()}; Plate={request.PlateText}); Errors: ", 
                response.Errors);
        }
    }
}
