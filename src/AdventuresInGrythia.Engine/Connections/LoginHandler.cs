using System.Collections.Generic;
using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine.Managers;
using MoonSharp.Interpreter;

namespace AdventuresInGrythia.Engine.Connections
{
    public class LoginHandler : ConnectionHandler
    {
        public LoginHandler(Connection c, Account a) : base(c, a)
        {
            try
            {
                _script = new Script();
                _script.DoString(ScriptManager.Instance.GetScript(ScriptType.GameFlow, "login"));
            }
            catch (KeyNotFoundException ex)
            {
                //Add logging
                throw ex;
            }
        }

        public override void Enter(params object[] args)
        {
            Game.Instance.SendMessage(_account.Id, _script.Call(_script.Globals["getTitle"]).String);
            Game.Instance.SendMessage(_account.Id, _script.Call(_script.Globals["motd"]).String);
            //_connection.RemoveHandler();
            //_connection.AddHandler<MainMenuHandler>();
            //_connection.Handler.Enter();
        }

        public override void Handle(string command) { }

        public override void Leave() { }
    }
}