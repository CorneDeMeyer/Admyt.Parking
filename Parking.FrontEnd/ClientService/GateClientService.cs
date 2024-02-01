using Parking.FrontEnd.ClientService.Interface;
using Parking.Domain.Config;
using Parking.Domain.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Parking.FrontEnd.ClientService
{
    public class GateClientService(ClientConfiguration config) : IGateClientService
    {
        private readonly ClientConfiguration _config = config;

        public async Task<GateEventResponse<double>> CalculateFee(string plateText)
        {
            var response = new GateEventResponse<double>();

            try
            {
                using (var client = new HttpClient())
                {
                    var request = await client.GetAsync($"{_config.WebApi}Gate/calculate-fee?plateText={plateText}");

                    if (request.IsSuccessStatusCode)
                    {
                        response.Value = await request.Content.ReadFromJsonAsync<double>();
                    }
                    else
                    {
                        var responseErrors = await request.Content.ReadAsStringAsync();
                        response.Errors.AddRange(responseErrors.Split(";").ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add($"Could not request Parking Fee: {ex.Message}");
            }

            return response;
        }

        public async Task<GateEventResponse<Guid>> ProcessGateRequest(GateEvent gateEvent)
        {
            var response = new GateEventResponse<Guid>();

            try
            {
                using (var http = new HttpClient())
                {
                    using StringContent jsonContent = new(JsonSerializer.Serialize(gateEvent), Encoding.UTF8, "application/json");

                    var request = await http.PostAsync($"{_config.WebApi}Gate/gate-events", jsonContent);

                    if (request.IsSuccessStatusCode)
                    {
                        response.Value = await request.Content.ReadFromJsonAsync<Guid>();
                    }
                    else
                    {
                        var responseErrors = await request.Content.ReadAsStringAsync();
                        response.Errors.AddRange(responseErrors.Split(";").ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add($"Could not request Gate Event: {ex.Message}");
            }

            return response;
        }
    }
}
