
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoNicotine_Data.Context;
using Microsoft.EntityFrameworkCore;

namespace NoNicotine_Business.Chat.Hubs
{
  [Authorize(Roles = "patient,therapist")]
  public class ChatHub : Hub
  {
    private readonly AppDbContext _dbContext;

    public ChatHub(AppDbContext dbContext)
    {
      _dbContext = dbContext;

    }
    private static readonly Dictionary<string, string> connections = new Dictionary<string, string>();
    private static readonly Dictionary<string, string> entities = new Dictionary<string, string>();
    public void Subscribe(string recieverUserId)
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == null)
      {
        return;
      }

      if (connections.Any(connection => connection.Key == userId || connection.Value == userId))
      {
        return;
      }

      connections.Add(userId, recieverUserId);
    }
    private bool IsAllowedToSendMessage(string senderEntityId, string recieverEntityId)
    {
      return connections.Any(connection => (connection.Key == senderEntityId && connection.Value == recieverEntityId)
      || (connection.Key == recieverEntityId && connection.Value == senderEntityId));
    }

    public Task SendPrivateMessage(string user, string message)
    {
      string senderUserId = Context.User?.FindFirst("UserId")?.Value!;
      if (!IsAllowedToSendMessage(senderUserId, user))
      {
        return Task.CompletedTask;
      }

      return Clients.User(user).SendAsync("ReceiveMessage", message);
    }
  }
}