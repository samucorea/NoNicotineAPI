using Microsoft.AspNetCore.SignalR;


namespace NoNicotine_Business.Chat
{
  public class UserIdProvider : IUserIdProvider
  {
    public virtual string GetUserId(HubConnectionContext connection)
    {
      return connection.User?.FindFirst("UserId")?.Value!;
    }

  }
}
