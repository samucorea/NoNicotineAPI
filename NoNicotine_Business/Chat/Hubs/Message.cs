
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoNicotine_Data.Context;
using Microsoft.EntityFrameworkCore;

namespace NoNicotine_Business.Chat.Hubs
{

  public class Message
  {
    public string ID { get; set; }

    public string Content { get; set; }
    public Message(string message)
    {
      ID = Guid.NewGuid().ToString();
      Content = message;
    }
  }

}