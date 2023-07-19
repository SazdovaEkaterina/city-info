namespace CityInfo.API.Service;

public interface IMailService
{
    void Send(string subject, string message);
}