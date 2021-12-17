namespace Ellevation.ElevatorDemo.Elevator
{
    public interface IController
    {
        /* Advances time by one tick. In a 'real' controller, this method would be called within a 
         * main control loop and would be responsible for checking sensors, engaging motors, handling
         * lights, etc. */
        void AdvanceTimeUnit();
    }
}