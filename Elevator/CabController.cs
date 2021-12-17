namespace Ellevation.ElevatorDemo.Elevator
{
    /* The central controller/router for the elevator. Floor requests are sent here and 
     * then filtered into an 'ascending' or 'descending' queue based on whether they were a
     * floor or a cab button press and the elevator's current movement direction.
     *
     * Honestly - I'm not a big fan of my implementation here! It could use some refinement before 
     * I'd be totally happy with it. It isn't the cleanest code I've written in my life and probably has bugs, 
     * but should at least have enough to warrant discussion. */
    public class CabController : IController
    {
        public CabController(int floorCount)
        {
            _floorCount = floorCount;
            for(int i = 1; i <= floorCount; i++) 
            {
                _floorDoorControllers[i] = new DoorController($"floor { i }");
            }
        }

        private readonly DoorController _cabDoorController = new DoorController("cab");
        private readonly int _floorCount;
        private readonly Dictionary<int, DoorController> _floorDoorControllers = new Dictionary<int, DoorController>(); 
        public int CurrentFloor { get; private set; } = 1;
        public MovementDirection MovementDirection { get; private set; } = MovementDirection.None;

        private SortedSet<RouteNode> _ascendingNodes 
            = new SortedSet<RouteNode>(new AscendingFloorComparer());

        private SortedSet<RouteNode> _descendingNodes
            = new SortedSet<RouteNode>(new DescendingFloorComparer());

        public void AdvanceTimeUnit()
        {
            var activeRouteNode = GetNextRouteNode();

            if(activeRouteNode != null)
            {
                if(activeRouteNode.CommandAuthority == CommandAuthority.Override || AreAllDoorsClosed())
                {
                    if(activeRouteNode.FloorNumber < CurrentFloor)
                    {
                        MovementDirection = Elevator.MovementDirection.Down;
                        CurrentFloor--;
                        Console.WriteLine($"   Moved down to floor {CurrentFloor}");
                    }
                    else if(activeRouteNode.FloorNumber > CurrentFloor)
                    {
                        MovementDirection = Elevator.MovementDirection.Up;
                        CurrentFloor++;
                        Console.WriteLine($"   Moved up to floor {CurrentFloor}");
                    }
                    else
                    {
                        var isStopping = false;
                        if((MovementDirection == MovementDirection.Up && activeRouteNode.Direction == MovementDirection.Up)
                            || (MovementDirection == MovementDirection.Down && activeRouteNode == _ascendingNodes.FirstOrDefault()))
                        {
                            _ascendingNodes.Remove(activeRouteNode);
                            isStopping = true;
                        }
                        else if((MovementDirection == MovementDirection.Down && activeRouteNode.Direction == MovementDirection.Down)
                            || (MovementDirection == MovementDirection.Up && activeRouteNode == _descendingNodes.FirstOrDefault()))
                        {
                            _descendingNodes.Remove(activeRouteNode);
                            isStopping = true;
                        }
                        
                        if(isStopping)
                        {
                            Console.WriteLine($"   Stopped on floor {CurrentFloor}");
                            _cabDoorController.SetDesiredDoorState(DoorState.Opened);
                            _floorDoorControllers[CurrentFloor].SetDesiredDoorState(DoorState.Opened);
                        }
                    }
                }
            }
            
            /* This is not algorithmically efficient, but is more accurate in a simulation sense
             * as the 'clock' signal would reach each door regardless of whether it was expected
             * to do anything. That'd be async though; this is not. */
            _cabDoorController.AdvanceTimeUnit();
            foreach(var doorController in _floorDoorControllers.Values) 
            {
                doorController.AdvanceTimeUnit();
            }
        }

        private RouteNode? GetNextRouteNode()
        {
            // This could be more efficient if I didn't re-calc every time, but I've been at this long enough.
            RouteNode? activeRouteNode = null;
            if(MovementDirection != MovementDirection.Down)
            {
                activeRouteNode = _ascendingNodes.FirstOrDefault(
                    n => n.FloorNumber >= CurrentFloor
                );

                if(activeRouteNode == null) 
                {
                    activeRouteNode = _descendingNodes.FirstOrDefault();
                    if(activeRouteNode == null)
                    {
                        activeRouteNode = _ascendingNodes.FirstOrDefault();
                    }
                }
            }
            else
            {
                activeRouteNode = _descendingNodes.FirstOrDefault(
                    n => n.FloorNumber <= CurrentFloor
                );

                if(activeRouteNode == null) 
                {
                    activeRouteNode = _ascendingNodes.FirstOrDefault();
                    if(activeRouteNode == null)
                    {
                        activeRouteNode = _descendingNodes.FirstOrDefault();
                    }
                }
            }

            return activeRouteNode;
        }

        internal void HandleDoorCommand(CommandAuthority authority, DoorState desiredDoorState)
        {
            if(CanAcceptDoorCommand(authority))
            {
                _cabDoorController.SetDesiredDoorState(desiredDoorState);
                _floorDoorControllers[CurrentFloor].SetDesiredDoorState(desiredDoorState);
            }
        }

        public bool IsDoorOpen()
        {
            return _cabDoorController.DoorState == DoorState.Opened 
                && _floorDoorControllers[CurrentFloor].DoorState == DoorState.Opened;
        }

        internal void AddRouteNode(RouteNode routeNode)
        {
            if(routeNode.CommandAuthority == CommandAuthority.Override)
            {
                _ascendingNodes.Clear();
                _descendingNodes.Clear();
            }
            
            if(routeNode.FloorNumber >= 1 && routeNode.FloorNumber <= _floorCount)
            {
                if(routeNode.Direction == MovementDirection.Up)
                    _ascendingNodes.Add(routeNode);
                else
                    _descendingNodes.Add(routeNode);
            }
        }

        /* Generally, I like to keep boolean logic segmented into methods like this when sensible -
         * I think it keeps the code self-documenting and readable */
        private bool AreAllDoorsClosed()
        {
            return _cabDoorController.DoorState == DoorState.Closed &&
                _floorDoorControllers.Values.All(door => door.DoorState == DoorState.Closed);
        }

        private bool CanAcceptDoorCommand(CommandAuthority authority)
        {
            return authority == CommandAuthority.Override 
                || MovementDirection == Elevator.MovementDirection.None;
        }
    }
}