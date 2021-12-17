using Ellevation.ElevatorDemo.Elevator;
namespace Ellevation.ElevatorDemo
{
    /* I initially wanted to build out a test project with a few examples, but honestly there's a limit to 
     * how much time I feel justified in giving to a take-home test, and the elevator routing was tricky. */
    class Program
    {
        static void Main(string[] args)
        {
            /* Elevator configuration setup */
            var floorCount = 10;
            var cabController = new CabController(floorCount);
            var cabInputPanel = new CabInputPanel(cabController, floorCount);
            var floorInputPanels = new Dictionary<int, FloorInputPanel>();
            for(var i = 1; i <= floorCount; i++)
            {
                floorInputPanels[i] = new FloorInputPanel(cabController, i);
            }

            /* Simulator setup */
            var runDurationTimeUnits = 35; // Should be long enough for most short demos
            var simulator = new Simulator(cabController, cabInputPanel, floorInputPanels);

            /* Simulation event setup (this could go into test cases instead) */
            simulator.AddEventOnTick(0, s => {
                s.FloorInputPanels[3].HandleDownButtonPressed(CommandAuthority.Standard);
                s.AddPassengerOnFloor(3, new SimPassenger(2, CommandAuthority.Standard));
            });
            simulator.AddEventOnTick(1, s => {
                s.FloorInputPanels[10].HandleDownButtonPressed(CommandAuthority.Standard);
                s.AddPassengerOnFloor(10, new SimPassenger(1, CommandAuthority.Standard));
            });

            simulator.Run(runDurationTimeUnits);
        }
    }
}