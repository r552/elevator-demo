namespace Ellevation.ElevatorDemo.Elevator
{
    internal class RouteNode
    {
        internal RouteNode(
            int floorNumber, 
            MovementDirection direction, 
            CommandAuthority authority)
        {
            FloorNumber = floorNumber;
            CommandAuthority = authority;
            Direction = direction;
        }

        public readonly int FloorNumber;
        public readonly CommandAuthority CommandAuthority;
        public readonly MovementDirection Direction;
    }

    internal class AscendingFloorComparer : IComparer<RouteNode>
    {
        public int Compare(RouteNode? first, RouteNode? second)
        {
            var firstNum = first?.FloorNumber ?? 0;
            var secondNum = second?.FloorNumber ?? 0;
            if(firstNum < secondNum)
                return -1;
            else if(firstNum > secondNum)
                return 1;
            else
                return 0;
        }
    }

    internal class DescendingFloorComparer : IComparer<RouteNode>
    {
        public int Compare(RouteNode? first, RouteNode? second)
        {
            var firstNum = first?.FloorNumber ?? 0;
            var secondNum = second?.FloorNumber ?? 0;
            if(firstNum > secondNum)
                return -1;
            else if(firstNum < secondNum)
                return 1;
            else
                return 0;
        }
    }
}