namespace Flights.Domain
{
    public partial class User
    {
        public string Name { get { return FirstName + " " + LastName; } }
    }
}
