namespace Flights.Domain
{
    public partial class Traveler
    {
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}