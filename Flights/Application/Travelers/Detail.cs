using Flights.Domain;
using MediatR;
using System.Linq;

namespace Flights.Application.Travelers
{
    public class Detail
    {
        // Input 

        public class Query : IRequest<Result>
        {
            public int? Id { get; set; }
        }

        // Output

        public class Result
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public int TotalBookings { get; set; }
        }

        public class QueryHandler : RequestHandler<Query, Result>
        {
            private readonly FlightsContext _db;

            public QueryHandler(FlightsContext db)
            {
                _db = db;
            }

            protected override Result Handle(Query query)
            {
                var result = new Result();
                var traveler = _db.Traveler.SingleOrDefault(p => p.Id == query.Id);

                if (traveler != null)
                {
                    result.Id = traveler.Id;
                    result.Name = traveler.Name;
                    result.FirstName = traveler.FirstName;
                    result.LastName = traveler.LastName;
                    result.Email = traveler.Email;
                    result.City = traveler.City;
                    result.Country = traveler.Country;
                    result.TotalBookings = traveler.TotalBookings;
                }

                return result;
            }
        }
    }
}
