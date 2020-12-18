using System.ComponentModel.DataAnnotations;
using System.Linq;
using Flights.Domain;
using MediatR;

namespace Flights.Application.Travelers
{
    public class Edit
    {
        // Query Input

        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class QueryHandler : RequestHandler<Query, Command>
        {
            private readonly FlightsContext _db;

            public QueryHandler(FlightsContext db)
            {
                _db = db;
            }

            protected override Command Handle(Query message)
            {
                var command = new Command();

                var traveler = _db.Traveler.SingleOrDefault(p => p.Id == message.Id);

                if (traveler != null)
                {
                    command.Id = traveler.Id;
                    command.FirstName = traveler.FirstName;
                    command.LastName = traveler.LastName;
                    command.Email = traveler.Email;
                    command.City = traveler.City;
                    command.Country = traveler.Country;
                    command.TotalBookings = traveler.TotalBookings;
                }

                return command;
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "First Name is required")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Last Name is required")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }
            [Required(ErrorMessage = "City is required")]
            public string City { get; set; }
            [Required(ErrorMessage = "Country is required")]
            public string Country { get; set; }
            public int TotalBookings { get; set; }
        }

        // Command

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
                if (message.Id == 0) 
                    InsertTraveler(message);
                else
                    UpdateTraveler(message);
                
            }

            private void InsertTraveler(Command message)
            {
                var traveler = new Domain.Traveler();
                traveler.FirstName = message.FirstName;
                traveler.LastName = message.LastName;
                traveler.Email = message.Email;
                traveler.City = message.City;
                traveler.Country = message.Country;

                _db.Traveler.Add(traveler);
                _db.SaveChanges();

                _cache.AddTraveler(traveler);
            }

            private void UpdateTraveler(Command message)
            {
                var traveler = _db.Traveler.Find(message.Id);

                traveler.FirstName = message.FirstName;
                traveler.LastName = message.LastName;
                traveler.Email = message.Email;
                traveler.City = message.City;
                traveler.Country = message.Country;

                _db.Traveler.Update(traveler);
                _db.SaveChanges();

                _cache.UpdateTraveler(traveler);
            }
        }
    }
}