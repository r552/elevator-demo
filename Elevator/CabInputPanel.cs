namespace Ellevation.ElevatorDemo.Elevator
{
    /* The floor buttons and open/close door buttons inside the cab itself */
    public class CabInputPanel
    {
        internal CabInputPanel(CabController controller, int floorCount)
        {
            _controller = controller;
        }

        private readonly CabController _controller;

        public void HandleFloorButtonPressed(CommandAuthority authority, int floorNumber)
        {
            _controller.AddRouteNode(
                new RouteNode(
                    floorNumber, 
                    floorNumber < _controller.CurrentFloor ? MovementDirection.Down : MovementDirection.Up,
                    authority
                )
            );
            Console.WriteLine($"   [ACTION]: Cab button {floorNumber} was pressed");
        }

        public void HandleDoorOpenButtonPressed(CommandAuthority authority)
        {
            _controller.HandleDoorCommand(authority, DoorState.Opened);
            Console.WriteLine($"Cab button 'open door' was pressed");
        }

        public void HandleDoorClosedButtonPressed(CommandAuthority authority)
        {
            _controller.HandleDoorCommand(authority, DoorState.Closed);
            Console.WriteLine($"Cab button 'close door' was pressed");
        }
    }
}