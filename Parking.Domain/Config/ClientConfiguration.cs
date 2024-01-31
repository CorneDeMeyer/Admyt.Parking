namespace Parking.Domain.Config
{
    public class ClientConfiguration(string webApi, string signalR_Url)
    {
        public string WebApi { get; private set; } = webApi;
        public string SignalRUrl { get; private set; } = signalR_Url;
    }
}
