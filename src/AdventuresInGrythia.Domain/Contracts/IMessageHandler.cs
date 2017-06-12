namespace AdventuresInGrythia.Domain.Contracts
{
    public interface IMessageHandler
    {
        void SendToAll(string message, params string[] args);
        void SendToAccount(int accountId, string message, params string[] args);
        void Logout(int accountId);
    }
}