namespace Ellevation.ElevatorDemo.Elevator
{
    /* A 'Floor input panel' represents the up and down buttons which exist on each floor */
    public class FloorInputPanel
    {
        public FloorInputPanel(CabController controller, int floorNumber)
        {
            _controller = controller;
            FloorNumber = floorNumber;
        }

        private readonly CabController _controller;
        public readonly int FloorNumber;
        
        /* Realistically, these should be handling a signal/event (from a button) instead
         * of creating one. I have to draw the line somewhere and I don't want to simulate buttons,
         * but I kept this naming convention for discussion purposes. 
         * 
         * There are also definitely ways of abstracting these, as they're essentially identical methods.
         * Since an elevator can only ever go up and down, it's arguably a waste. What's definitely a 
         * waste is how much time I'm spending dwelling on this... it's just a demo. Moving on. */
        public void HandleUpButtonPressed(CommandAuthority authority)
        {
            _controller.AddRouteNode(
                new RouteNode(FloorNumber, MovementDirection.Up, authority)
            );
            Console.WriteLine($"   [ACTION]: Floor { FloorNumber } UP button pressed");
        }
        public void HandleDownButtonPressed(CommandAuthority authority)
        {
             _controller.AddRouteNode(
                new RouteNode(FloorNumber, MovementDirection.Down, authority)
            );
            Console.WriteLine($"   [ACTION]: Floor { FloorNumber } DOWN button pressed");
        }
    }
}