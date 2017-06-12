namespace AdventuresInGrythia.Domain.Contracts
{
    public interface IConnectionHandler
    {
        void Enter(params object[] args);
        void Leave();
        void Handle(string command);
    }
}