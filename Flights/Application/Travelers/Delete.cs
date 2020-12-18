using Flights.Domain;
using MediatR;

namespace Flights.Application.Travelers
{
    public class Delete
    {
        // Input

        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class CommandHandler : RequestHandler<Command>
        {
            private readonly FlightsContext _db;
            private readonly ICache _cache;

            public CommandHandler(FlightsContext db, ICache cache)
            {
                _db = db;
                _cache = cache;
            }

            protected override void Handle(Command message)
            {
                var traveler = _db.Traveler.Find(message.Id);

                _db.Traveler.Remove(traveler);
                _db.SaveChanges();

                _cache.DeleteTraveler(traveler);
            }
        }
    }
}