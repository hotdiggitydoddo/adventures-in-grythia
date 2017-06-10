using System;
using System.Collections.Generic;
using AdventuresInGrythia.Engine.Actions;
using AdventuresInGrythia.Engine.Locations;
using AdventuresInGrythia.Engine.Managers;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine
{
    public class Game
    {
        public static Game Instance;
        
        readonly IEntityManager _entityManager;
        readonly ICommandManager _commandManager;
        Dictionary<int, AiGEntity> _entities;
        Dictionary<int, AiGRoom> _rooms;
        Dictionary<int, AiGRegion> _regions;
        Dictionary<int, AiGZone> _zones;
        Dictionary<int, AiGPortal> _portals;
        Dictionary<int, AiGPortalEntry> _portalEntries;
        public Game(IEntityManager entityMgr, ICommandManager commandMgr)
        {
            Instance = this;
            _entityManager = entityMgr;
            _commandManager = commandMgr;

            _commandManager.Init(this);
        }

        public void DoAction(AiGAction action)
        {

        }
    }
}
