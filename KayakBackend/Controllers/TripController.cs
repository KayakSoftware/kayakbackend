using System;
using System.Threading.Tasks;
using KayakBackend.Persistence.Entities;
using KayakBackend.Persistence.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KayakBackend.Controllers
{
    [Route("trips")]
    public class TripController
    {
        private readonly ITripRepository _tripRepository;

        public TripController(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTrips(int page = 0, int pageSize = 100)
        {
            var tripsPaged = await _tripRepository.GetPaged(page, pageSize);
            return new ObjectResult(tripsPaged);
        }

        [HttpGet]
        [Route("{tripId}")]
        public async Task<IActionResult> GetTrip(Guid tripId)
        {
            var trip = await _tripRepository.GetById(tripId);
            if (trip == null)
            {
                return new NotFoundResult();
            }
            return new ObjectResult(trip);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTrip()
        {
            var createResult = await _tripRepository.Insert(
                new Trip
                {
                    Id = Guid.NewGuid(),
                    StartTime = DateTime.UtcNow
                });

            var trip = await _tripRepository.GetById(createResult);
            return new ObjectResult(trip);
        }
    }
}