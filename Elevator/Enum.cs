namespace Ellevation.ElevatorDemo.Elevator
{

    public enum CommandAuthority
    {
        Standard,
        Override
    }

    public enum DoorState
    {
        Closed,
        Closing,
        Opened,
        Opening
    }

    public enum MovementDirection
    {
        None,
        Down,
        Up
    }
}