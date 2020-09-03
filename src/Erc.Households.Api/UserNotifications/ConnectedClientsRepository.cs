using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.UserNotifications
{
    public class ConnectedClientsRepository
    {
        readonly IDictionary<string, string> _store = new ConcurrentDictionary<string,string>();

        public void Add(string connectionId, string userId) => _store.Add(connectionId, userId);
        
        public void Remove(string connectionId) => _store.Remove(connectionId);

        public string GetConnectionId(string userId) => _store.FirstOrDefault(s => string.Equals(s.Value, userId, StringComparison.InvariantCultureIgnoreCase)).Key;
    }
}
