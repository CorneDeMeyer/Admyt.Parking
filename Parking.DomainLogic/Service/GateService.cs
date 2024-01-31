using Parking.DomainLogic.Interface;
using Parking.Repository.Interface;
using Microsoft.Extensions.Logging;
using Parking.Domain.Models;
using Parking.Domain.Enum;

namespace Parking.DomainLogic.Service
{
    public class GateService(IGateEventRepository gateEventRepository,
        IParkingSessionRepository parkingSessionRepository,
        IGateRepository gateRepository,
        IZoneRepository zoneRepository,
        ILoggerFactory loggerFactory) : IGateService
    {
        private readonly IZoneRepository _zoneRepository = zoneRepository;
        private readonly IGateRepository _gateRepository = gateRepository;
        private readonly ILogger _logger = loggerFactory.CreateLogger<GateService>();
        private readonly IGateEventRepository _gateEventRepository = gateEventRepository;
        private readonly IParkingSessionRepository _parkingSessionRepository = parkingSessionRepository;

        /// <summary>
        /// Calculate Parking Fee
        /// </summary>
        /// <param name="plateText">Vehicle's Plate</param>
        /// <returns>Amount Payable</returns>
        public async Task<GateEventResponse<double>> CalculateFee(string plateText)
        {
            var response = new GateEventResponse<double>();

            try
            {
                var gateEventsForPlate = await _gateEventRepository.GetByPlate(plateText);

                if (gateEventsForPlate != null && gateEventsForPlate.Any())
                {
                    var gates = await _gateRepository.GetAll();
                    // Getting the latest Entry Date ! Does not pay for more than 1 parking zone (ONLY pays for zone latest parked zone)
                    var latestEntry = gateEventsForPlate
                        .OrderByDescending(r => r.TimeStamp)
                        .FirstOrDefault(
                            gatesEvent => gates.Any(g => g.Id == gatesEvent.GateId && g.Type == GateType.Entry));

                    if (latestEntry != null)
                    {
                        // Get Latest Session
                        var session = await _parkingSessionRepository.GetByPlate(plateText);
                        if (session != null)
                        {
                            // Get Gate Entry to determine latest 
                            var entryGate = gates.Where(gate => gate.Id == latestEntry.Id).First();
                            var exitAfterLatestEntry = gateEventsForPlate
                            .OrderByDescending(r => r.TimeStamp)
                            .FirstOrDefault(
                                gatesEvent => gatesEvent.TimeStamp >= latestEntry.TimeStamp
                                           && gates.Any(g => g.Id == gatesEvent.GateId && g.Type == GateType.Exit));

                            var timeSpent = ((exitAfterLatestEntry != null
                                              ? exitAfterLatestEntry.TimeStamp
                                              : DateTime.UtcNow) - latestEntry.TimeStamp);
                            if (timeSpent.TotalMinutes <= 15)
                            {
                                response.Value = 0.00f;
                            }
                            else
                            {
                                // Calculate Parking Fee, Ceilling to closest hour
                                var zone = await _zoneRepository.GetById(entryGate.ZoneId);

                                if (zone != null)
                                {
                                    response.Value = zone.Rate * Math.Ceiling(timeSpent.TotalHours); // Ceilling the hour
                                }
                            }
                        }
                        else
                        {
                            // No Session found, parking is free
                            response.Value = 0.00f;
                        }
                    }
                }
                else
                {
                    response.Errors.Add($"No Events found for plate: {plateText}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Attempting to Calculate Parking Fee.");
                response.Errors.Add($"Error calculating parking fee: {ex.Message}");
            }

            return response;
        }

        public async Task<GateEventResponse<Guid>> ProcessGateRequest(GateEvent gateEvent)
        {
            var response = new GateEventResponse<Guid>();

            try
            {
                if (gateEvent != null)
                {
                    var gate = await _gateRepository.GetById(gateEvent.GateId);
                    if (gate != null)
                    {
                        switch (gate.Type)
                        {
                            case GateType.Entry:
                                return await ManageVehicleEntry(gateEvent);
                            case GateType.Exit:
                                return await ManageVehicleExit(gateEvent);
                            default:
                                response.Errors.Add($"Gate Id {gateEvent.GateId.ToString()} type ({gate.Type.ToString()}) is not valid.");
                                break;
                        }
                    }
                    else
                    {
                        response.Errors.Add($"Gate Id {gateEvent.GateId.ToString()} is not valid.");
                    }
                }
                else
                {
                    response.Errors.Add($"Invalid Gate Event detected, please submit valid event");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Attempting to Calculate Parking Fee.");
                response.Errors.Add($"Error calculating parking fee: {ex.Message}");
            }

            return response;
        }

        private async Task<GateEventResponse<Guid>> ManageVehicleEntry(GateEvent gateEvent)
        {
            var response = new GateEventResponse<Guid>();

            try
            {
                // Create Session
                var sessionId = await _parkingSessionRepository.Create(new ParkingSession
                {
                    PlateText = gateEvent.PlateText,
                    Status = SessionStatus.Active
                });

                // Set Session Id and Create GateEvent
                gateEvent.ParkingSessionId = sessionId;
                var eventId = await _gateEventRepository.Create(gateEvent);

                response.Value = sessionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Managing Vehicle Entry: {gateEvent.PlateText} ata gate: {gateEvent.GateId.ToString()}.");
                response.Errors.Add($"Error Managing Vehicle Entry: {ex.Message}");
            }

            
            return response;
        }

        private async Task<GateEventResponse<Guid>> ManageVehicleExit(GateEvent gateEvent)
        {
            var response = new GateEventResponse<Guid>();

            try
            {
                var session = await _parkingSessionRepository.GetByPlate(gateEvent.PlateText);
                if (session != null)
                {
                    // Update Session Parking to be completed
                    response.Value = session.Id;
                    session.Status = SessionStatus.Completed;
                    await _parkingSessionRepository.Update(session);

                    // Create GateEvent with Session Id
                    gateEvent.ParkingSessionId = session.Id;
                    var eventId = await _gateEventRepository.Create(gateEvent);
                }
                else
                {
                    response.Errors.Add($"No Current Session for Plate: {gateEvent.PlateText}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Managing Vehicle Exit: {gateEvent.PlateText} ata gate: {gateEvent.GateId.ToString()}.");
                response.Errors.Add($"Error Managing Vehicle Exit: {ex.Message}");
            }

            return response;
        }
    }
}
