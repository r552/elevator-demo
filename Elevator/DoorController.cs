namespace Ellevation.ElevatorDemo.Elevator
{
    /* Represents a signal-controllable door.
     * For this demo, I assume that elevator doors have their own logic and motor controllers 
     * (rather than wiring them all up to the central controller) */
    public class DoorController : IController
    {
        internal DoorController(string doorDescription)
        { 
            _doorDescription = doorDescription;
        }

        private readonly string _doorDescription;
        private DoorState _desiredDoorState = DoorState.Closed;
        public DoorState DoorState { get; private set; } = DoorState.Closed;

        public void AdvanceTimeUnit()
        {
            if(DoorState == DoorState.Closed && _desiredDoorState == DoorState.Closed)
                return;
            
            if(_desiredDoorState == DoorState.Opened)
            {
                if(DoorState == DoorState.Opening)
                {
                    DoorState = DoorState.Opened;
                    Console.WriteLine($"   Door ({ _doorDescription }) finished opening");
                    _desiredDoorState = DoorState.Closed;
                }
                else
                {
                    DoorState = DoorState.Opening;
                    Console.WriteLine($"   Door ({ _doorDescription }) started opening");
                }
            }
            else if(_desiredDoorState == DoorState.Closed)
            {
                if(DoorState == DoorState.Closing)
                {
                    DoorState = DoorState.Closed;
                    Console.WriteLine($"   Door ({ _doorDescription }) finished closing");
                }
                else
                {
                    DoorState = DoorState.Closing;
                    Console.WriteLine($"   Door ({ _doorDescription }) started closing");
                }
            }
        }

        /* Tell the door to work toward (or maintain) the 'Opened' or 'Closed' state.
           Only the 'Opened' or 'Closed' states are valid. */
        public void SetDesiredDoorState(DoorState doorState)
        {
            if(doorState == DoorState.Opening || doorState == DoorState.Closing)
            {
                throw new InvalidOperationException("'Opening' and 'Closing' are transitional states only. " +
                    "Use 'Opened' or 'Closed' instead.");
            }

            _desiredDoorState = doorState;
        }
    }
}