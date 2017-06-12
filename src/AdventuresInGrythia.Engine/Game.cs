using System;
using System.Collections.Generic;
using System.Threading;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Engine.Actions;
using AdventuresInGrythia.Engine.Locations;
using AdventuresInGrythia.Engine.Managers;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine
{
    public class Game
    {
        public static Game Instance;

        //Managers
        readonly IEntityManager _entityManager;
        readonly ICommandManager _commandManager;
        IMessageHandler _msgHandler;
        IOutputFormatter _formatter;

        //In-memory caches
        Dictionary<int, AiGEntity> _entities;
        Dictionary<int, AiGRoom> _rooms;
        Dictionary<int, AiGRegion> _regions;
        Dictionary<int, AiGZone> _zones;
        Dictionary<int, AiGPortal> _portals;
        Dictionary<int, AiGPortalEntry> _portalEntries;

        //Timing
        long _lastTime;
        Timer _timer;
        bool _updating;

        public long TimeRunning { get; private set; }
        public long CurrentTime { get; private set; }

        public Game(IEntityManager entityMgr, ICommandManager commandMgr, IOutputFormatter formatter)
        {
            Instance = this;
            _entityManager = entityMgr;
            _commandManager = commandMgr;
            _commandManager.Init(this);
            _formatter = formatter;

            _entities = new Dictionary<int, AiGEntity>();
            _rooms = new Dictionary<int, AiGRoom>();
            _regions = new Dictionary<int, AiGRegion>();
            _zones = new Dictionary<int, AiGZone>();
            _portals = new Dictionary<int, AiGPortal>();
            _portalEntries = new Dictionary<int, AiGPortalEntry>();

            ScriptManager.Instance.RefreshScripts(ScriptType.Command);
            ScriptManager.Instance.RefreshScripts(ScriptType.Component);
            ScriptManager.Instance.RefreshScripts(ScriptType.GameFlow);
            //ScriptManager.Instance.RefreshScripts(ScriptType.ActionRunner);
        }
        public void Init(IMessageHandler handler)
        {
            _msgHandler = handler;
        }

        public void Start()
        {
            TimeRunning = 0;
            _lastTime = DateTime.UtcNow.Ticks;
            _timer = new Timer(OnTimerElapsed, null, TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(1000 / 60));
        }

        private void Tick(long elapsedTime)
        {
            //_timerRegistry.Dispatch();
        }

        private void OnTimerElapsed(object state)
        {
            if (_updating) return;
            _updating = true;

            CurrentTime = DateTime.UtcNow.Ticks;

            var elapsed = (CurrentTime - _lastTime);

            Tick(elapsed);

            TimeRunning += elapsed;
            _lastTime = CurrentTime;

            _updating = false;
        }
        public void LoadCommandsSet()
        {
            _commandManager.LoadCommandsSet();
        }
        public void SendMessage(int accountId, string message)
        {
            _msgHandler.SendToAccount(accountId, _formatter.FormatMessage(message));
        }
        public void BroadcastMessage(string message)
        {
            _msgHandler.SendToAll(_formatter.FormatMessage(message));
        }

        public void DoAction(AiGAction action)
        {

        }

        public AiGEntity CreatePlayerCharacter(int accountId, AiGEntity model)
        {
            return _entityManager.CreatePlayerCharacter(accountId, model);
        }
         public List<AiGEntity> LoadPlayerCharacters(int accountId)
        {
            return _entityManager.LoadCharactersFromAccount(accountId);
        }
    }
}
