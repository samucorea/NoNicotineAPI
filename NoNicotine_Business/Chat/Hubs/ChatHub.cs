
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoNicotine_Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using NoNicotine_Business.Services;

namespace NoNicotine_Business.Chat.Hubs
{
  [Authorize(Roles = "patient,therapist")]
  public class ChatHub : Hub
  {
    private static readonly Dictionary<string, string> patientConnections = new Dictionary<string, string>();
    private static readonly Dictionary<string, List<string>> therapistConnections = new Dictionary<string, List<string>>();

    private static readonly Dictionary<string, List<Message>> messageQueue = new Dictionary<string, List<Message>>();
    public override Task OnConnectedAsync()
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == null)
      {
        return Task.CompletedTask;
      }
      var missingMessages = messageQueue.TryGetValue(userId, out var userMessageQueue);


      // userMessageQueue?.ForEach(async message =>
      // {
      //   await Clients.User(userId).SendAsync("ReceiveMessage", message);
      //   Thread.Sleep(50);
      // });


      return Task.CompletedTask;
    }

    public void Subscribe(string recieverUserId)
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == "")
      {
        return;
      }

      var role = Context?.User?.FindFirst(ClaimTypes.Role)?.Value!;

      switch (role)
      {
        case "patient":
          {
            SubscribePatientToTherapist(recieverUserId);
            break;
          }
        case "therapist":
          {
            SubscribeTherapistToPatient(recieverUserId);
            break;
          }
      }
    }
    private void SubscribePatientToTherapist(string therapistUserId)
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == "")
      {
        return;
      }

      patientConnections.TryAdd(userId, therapistUserId);
    }

    private void SubscribeTherapistToPatient(string recieverUserId)
    {
      var userId = Context?.User?.FindFirst("UserId")?.Value!;
      if (userId == "" || recieverUserId == "")
      {
        return;
      }

      var found = therapistConnections.TryGetValue(userId, out var connectedPatients);
      if (!found || connectedPatients == null)
      {
        connectedPatients = new List<string>();
        connectedPatients.Add(recieverUserId);
        therapistConnections.Add(userId, connectedPatients);
        return;
      }

      if (connectedPatients.Any(patientId => patientId == recieverUserId))
      {
        return;
      }

      connectedPatients.Add(recieverUserId);
    }

    private bool IsAllowedToSendMessage(string role, string senderEntityId, string recieverEntityId)
    {
      switch (role)
      {
        case "patient":
          {
            return patientConnections.Any(connection => (connection.Key == senderEntityId
            && connection.Value == recieverEntityId));
          }
        case "therapist":
          {
            return therapistConnections.Any(connection => connection.Key == senderEntityId
            && connection.Value.Any(connection => connection == recieverEntityId));
          }
      }

      return false;

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
      var role = Context?.User?.FindFirst(ClaimTypes.Role)?.Value!;

      // if (!IsAllowedToSendMessage(role, senderUserId, user))
      // {
      //   return Task.CompletedTask;
      // }

      var newMessage = new Message(message, senderUserId);

      var hasMessageQueue = messageQueue.TryGetValue(user, out var userMessageQueue);
      if (!hasMessageQueue || userMessageQueue == null)
      {
        messageQueue.Add(user, new List<Message>());
        userMessageQueue = messageQueue[user];
      }

      userMessageQueue.Add(newMessage);


      return Clients.User(user).SendAsync("ReceiveMessage", newMessage);
    }


  }
}