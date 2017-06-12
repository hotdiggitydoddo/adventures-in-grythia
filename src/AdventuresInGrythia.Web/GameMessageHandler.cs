using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine;
//using AdventuresInGrythia.Engine.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
//using AdventuresInGrythia.Engine.UI;
using System.Threading;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Engine.Connections;
using AdventuresInGrythia.Engine.Managers;
using System.Collections.Concurrent;
//using AdventuresInGrythia.Engine.World;

namespace AdventuresInGrythia.Web
{
    public class GameMessageHandler : WebSocketHandler, IMessageHandler
    {
        ConcurrentDictionary<string, Connection> _connections;
        private Thread _gameThread;
        public GameMessageHandler(WebSocketConnectionManager webSocketConnectionManager)
            : base(webSocketConnectionManager)
        {
            Game.Instance.Init(this);
            _gameThread = new Thread(Game.Instance.Start);
            _gameThread.Start();
            _connections = new ConcurrentDictionary<string, Connection>();
        }
        
        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var connectionId = WebSocketConnectionManager.GetId(socket);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            // var connectionId = WebSocketConnectionManager.GetId(socket);

            // var conn = _connections.Single(x => x.Id == connectionId);
            // if (conn.Handler != null)
            // {
            //     conn.Handler.Leave();
            // }

            

            // _connections.RemoveAll(x => x.Id == connectionId);

            await base.OnDisconnected(socket);
        }

        public async void SendToAll(string message, params string[] args)
        {
            await InvokeClientMethodToAllAsync("receiveMessage", message, args);
        }

        public async void SendToAccount(int accountId, string message, params string[] args)
        {
            var conn = _connections.Values.Single(x => x.Account.Id == accountId);
            await InvokeClientMethodAsync(conn.Id, "receiveMessage", new[] { message });
        }

        public async Task ReceiveMessage(string connectionId, string message)
        {
            var conn = _connections[connectionId];
            if (conn.Handler != null)
            {
                // if (message == "create")
                // {
                //     var e = _entityService.Create();
                //     var t = new Dictionary<string, string>();
                //     t.Add("Health", "155");
                //     var e1 = _entityService.Create(t);
                //     var e2 = 
                // }
                //get a IConnectionHandler result or null - if the result
                if (message == "#")
                {
                    ScriptManager.Instance.RefreshScripts(ScriptType.GameFlow);
                    ScriptManager.Instance.RefreshScripts(ScriptType.Command);
                    Game.Instance.LoadCommandsSet();
                    await InvokeClientMethodAsync(conn.Id, "receiveMessage", new[] { "DEBUG: refreshed game scripts." });
                }
                else
                    conn.Handler.Handle(message.ToLower());
            }
            else
            {
                //pass to game
            }
        }

        public async void Login(string connectionId, Account account)
        {
            if (_connections.Values.Any(x => x.Account.Id == account.Id))
            {
               await InvokeClientMethodAsync(connectionId, "receiveMessage", new[] { "<span style=\"color:red;\">You are already connected to the game in another browser window.  Please close this window and return to the original one.</span>"});
               return;
            }

            var connection = new Connection(connectionId, account);
            _connections.TryAdd(connectionId, connection);
            connection.AddHandler<LoginHandler>();
            connection.Handler.Enter();
        }

        public void Quit(string connectionId)
        {
            var conn = _connections[connectionId];
            if (conn.Handler != null)
            {
                conn.Handler.Leave();
            }

            _connections.TryRemove(connectionId, out var c);
        }

        public void Logout(int accountId)
        {
            var conn = _connections.SingleOrDefault(x => x.Value.Account.Id == accountId).Value;
            if (conn.Handler == null) return;

            conn.RemoveHandler();
            //conn.AddHandler<MainMenuHandler>();
            //conn.Handler.Enter();
        }
    }
}
