using Ellevation.ElevatorDemo.Elevator;

namespace Ellevation.ElevatorDemo
{
    public class SimPassenger
    {
        public SimPassenger(int destinationFloor, CommandAuthority authority)
        {
            Authority = authority;
            DestinationFloor = destinationFloor;
        }

        public readonly CommandAuthority Authority;
        public readonly int DestinationFloor;
    }
}