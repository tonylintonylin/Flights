using Flights.Domain;
using MediatR;
using System.Collections.Generic;

namespace Flights.Application.Travelers
{
    public class List
    {
        // Input

        public class Query : IRequest<Result> { }

        // Output

        public class Result
        {
            public List<Traveler> Travelers { get; set; } = new List<Traveler>();

            public class Traveler
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Email { get; set; }
                public string City { get; set; }
                public string Country { get; set; }
                public int TotalBookings { get; set; }
            }
        }

        // Process

        public class QueryHandler : RequestHandler<Query, Result>
        {
            private readonly FlightsContext _db;

            public QueryHandler(FlightsContext db)
            {
                _db = db;
            }

            protected override Result Handle(Query request)
            {
                var result = new Result();

                foreach (var traveler in _db.Traveler)
                {
                    result.Travelers.Add(new Result.Traveler
                    {
                        Id = traveler.Id,
                        Name = traveler.Name,
                        FirstName = traveler.FirstName,
                        LastName = traveler.LastName,
                        Email = traveler.Email,
                        City = traveler.City,
                        Country = traveler.Country,
                        TotalBookings = traveler.TotalBookings
                    });
                }

                return result;
            }
        }
    }
}
