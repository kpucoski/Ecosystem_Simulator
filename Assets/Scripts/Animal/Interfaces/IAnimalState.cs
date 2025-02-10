using Animal.AnimalStates;

namespace Animal.Interfaces {
    public interface IAnimalState {
        void Enter();
        void Execute();
        void Exit();
        States GetState();
    }
}
