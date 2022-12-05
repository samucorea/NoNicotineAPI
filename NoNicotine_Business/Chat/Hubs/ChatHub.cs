
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

    private static readonly Dictionary<string, List<Message>> messageQueue = new Dictionary<string, List<Message>>();
    private static readonly Dictionary<string, string> entities = new Dictionary<string, string>();

    public  override Task OnConnectedAsync(){
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == null)
      {
        return Task.CompletedTask;
      }
      var missingMessages = messageQueue.TryGetValue(userId, out var userMessageQueue);
      if(!missingMessages || userMessageQueue == null){
        messageQueue.Add(userId, new List<Message>());
        return Task.CompletedTask;
      }

      userMessageQueue.ForEach(async message => {
        await Clients.User(userId).SendAsync("ReceiveMessage", message);
        Thread.Sleep(50);
      });


      return Task.CompletedTask;
    }
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

    public void AckMessage(string messageId)
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == null)
      {
        return;
      }

      messageQueue.TryGetValue(userId, out var userMessageQueue);

      userMessageQueue?.RemoveAll(message => message.ID == messageId);

    }

    public Task SendPrivateMessage(string user, string message)
    {
      string senderUserId = Context.User?.FindFirst("UserId")?.Value!;
      if (!IsAllowedToSendMessage(senderUserId, user))
      {
        return Task.CompletedTask;
      }

      var newMessage = new Message(message);

      var hasMessageQueue = messageQueue.TryGetValue(user, out var userMessageQueue);
      if(!hasMessageQueue || userMessageQueue == null){
        messageQueue.Add(senderUserId, new List<Message>());
        return Task.CompletedTask;
      }

      userMessageQueue.Add(newMessage);

      return Clients.User(user).SendAsync("ReceiveMessage", newMessage);
    }


  }
}