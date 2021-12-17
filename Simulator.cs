using Ellevation.ElevatorDemo.Elevator;

namespace Ellevation.ElevatorDemo
{
    public class Simulator
    {
        public Simulator(CabController cabController, CabInputPanel cabInputPanel, IDictionary<int, FloorInputPanel> floorInputPanels)
        {
            CabController = cabController;
            CabInputPanel = cabInputPanel;
            FloorInputPanels = floorInputPanels;
        }
        
        public readonly CabController CabController;
        public readonly CabInputPanel CabInputPanel;
        public readonly IDictionary<int, FloorInputPanel> FloorInputPanels;

        private readonly Dictionary<int, List<Action<Simulator>>> _eventsByTick 
            = new Dictionary<int, List<Action<Simulator>>>();

        private readonly Dictionary<int, List<SimPassenger>> _passengers
            = new Dictionary<int, List<SimPassenger>>();

        public void AddEventOnTick(int tickNum, Action<Simulator> ev)
        {
            if(!_eventsByTick.ContainsKey(tickNum))
            {
                _eventsByTick[tickNum] = new List<Action<Simulator>>();
            }

            _eventsByTick[tickNum].Add(ev);
        }

        public void AddPassengerOnFloor(int floor, SimPassenger passenger)
        {
            if(!_passengers.ContainsKey(floor))
            {
                _passengers[floor] = new List<SimPassenger>();
            }
            _passengers[floor].Add(passenger);
        }

        public void Run(int timeUnits)
        {
            for(int i = 0; i < timeUnits; i++)
            {
                Console.WriteLine($"-- Time: { i } --");
                if(_eventsByTick.ContainsKey(i))
                {
                    foreach(var ev in _eventsByTick[i])
                    {
                        ev.Invoke(this);
                    }
                }

                if(CabController.IsDoorOpen() && _passengers.ContainsKey(CabController.CurrentFloor))
                {
                    /* For simplicity's sake, I'm making the passengers just always board if the door opens and
                     * it's their floor. */
                    foreach(var passenger in _passengers[CabController.CurrentFloor])
                    {
                        CabInputPanel.HandleFloorButtonPressed(passenger.Authority, passenger.DestinationFloor);
                    }
                    _passengers[CabController.CurrentFloor].Clear();
                }

                CabController.AdvanceTimeUnit();
            }
        }
    }
}