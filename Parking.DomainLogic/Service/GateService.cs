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
                var latestParkingSessionForPlate = await _parkingSessionRepository.GetByPlate(plateText);

                if (latestParkingSessionForPlate != null)
                {
                    var gateEventsForSession = await _gateEventRepository.GetByParkingSessionId(latestParkingSessionForPlate.Id);

                    if (gateEventsForSession != null && gateEventsForSession.Any())
                    {
                        var gateEventsInOrder = gateEventsForSession.OrderBy(e => e.TimeStamp).ToArray();

                        for (var ge = 0; ge < gateEventsInOrder.Length; ge++)
                        {
                            var currentGateEvent = gateEventsInOrder[ge];
                            var nextGateEvent = ge + 1 < gateEventsInOrder.Length 
                                ? gateEventsInOrder[ge + 1]
                                : null;

                            // Get Out of loop when session has been completed
                            if (nextGateEvent == null && latestParkingSessionForPlate.Status == SessionStatus.Completed)
                            {
                                break;
                            }

                            var gate = await _gateRepository.GetById(currentGateEvent.GateId);
                            if (gate != null)
                            {
                                var zone = await _zoneRepository.GetById(gate.ZoneId);
                                if (zone != null)
                                {
                                    response.Value += CalculateSessionFee(currentGateEvent.TimeStamp, nextGateEvent?.TimeStamp ?? DateTime.UtcNow, zone.Rate, zone.Depth);
                                }
                                else
                                {
                                    response.Errors.Add($"No Zone found for Id: {gate.ZoneId.ToString()}");
                                }
                            }
                            else
                            {
                                response.Errors.Add($"No Gate found for Id: {currentGateEvent.GateId.ToString()}");
                            }
                        }
                    }
                    else
                    {
                        response.Errors.Add($"No Events found for plate: {plateText}");
                    }
                }
                else
                {
                    response.Errors.Add($"No Active Session found for plate: {plateText}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Attempting to Calculate Parking Fee.");
                response.Errors.Add($"Error calculating parking fee: {ex.Message}");
            }

            return response;
        }

        private double CalculateSessionFee(DateTime startTime, DateTime? endTime, double feePerHour, int level)
        {
            var timeSpent = (endTime.HasValue
                                ? endTime.Value
                                : DateTime.UtcNow) - startTime;

            // The first 15 minutes are free for sessions completed in depth 0.
            if (timeSpent.TotalMinutes < 15 && level == 0)
            {
                return 0;
            }
            // A vehicle is allowed to spend maximum of 5 minutes to 'pass through' a zone before the fee for the zone becomes applicable.
            else if (timeSpent.TotalMinutes < 5 && level == 0)
            {
                return 0;
            }
                                                  // A parking zone has a rate per hour, charge is rounded up to the nearest hour.
            return (double)((decimal)feePerHour * Math.Ceiling((decimal)timeSpent.TotalHours));
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
                        if (gate.IsActive)
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
                            response.Errors.Add($"Gate Id {gateEvent.GateId.ToString()} is not currently active.");
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
                var gate = await _gateRepository.GetById(gateEvent.GateId);
                
                if (gate != null)
                {
                    if (gate.IsActive)
                    {
                        var zone = await _zoneRepository.GetById(gate.ZoneId);
                        Guid sessionId;
                        if (zone != null && zone.Depth == 0)
                        {
                            // Create Session
                            sessionId = await _parkingSessionRepository.Create(new ParkingSession
                            {
                                PlateText = gateEvent.PlateText,
                                Status = SessionStatus.Active
                            });
                        }
                        else
                        {
                            // If car currently in parking lot we attach event to same session
                            var currentSession = await _parkingSessionRepository.GetByPlate(gateEvent.PlateText);
                            sessionId = currentSession.Id;
                        }

                        // Set Session Id and Create GateEvent
                        gateEvent.ParkingSessionId = sessionId;
                        var eventId = await _gateEventRepository.Create(gateEvent);

                        response.Value = sessionId;
                    }
                    else
                    {
                        response.Errors.Add($"Gate Id {gateEvent.GateId.ToString()} is not currently active.");
                    }
                }
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
                    var gate = await _gateRepository.GetById(gateEvent.GateId);
                    if (gate != null)
                    {
                        var zone = await _zoneRepository.GetById(gate.ZoneId);
                        if (zone != null && zone.Depth == 0)
                        {
                            // A parking session is considered completed if the last gate event was an exit event at depth 0.
                            response.Value = session.Id;
                            session.Status = SessionStatus.Completed;
                            await _parkingSessionRepository.Update(session);
                        }
                    }

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
