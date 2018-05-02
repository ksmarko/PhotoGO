using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Models;

namespace Web.Hubs
{
    [HubName("postHub")]
    public class PostHub : Hub
    {
        public Task Like(int likes)
        {
            return Clients.All.updateLikeCount(likes);
        }
    }
}