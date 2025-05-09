namespace Tutorial8.Repositories;

public interface IClientsRepository
{
    Task<bool> DoesClientExist(int clientId);
    
    
}