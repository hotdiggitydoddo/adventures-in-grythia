using System.Collections.Generic;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine.Locations;

namespace AdventuresInGrythia.Engine.Managers
{
    public class LocationManager
    {
        readonly IRepository<Room> _rooms;
        public int SaveRoom(AiGRoom room)
        {
            var existing = _rooms.GetById(room.Id, x => x.Traits, x => x.Components, x => x.Entities);
            if (existing != null)
            {
                //Update Traits
                foreach (var dbTrait in existing.Traits)
                {
                    if (!room.Traits.Has(dbTrait.Name))
                        existing.Traits.Remove(dbTrait);
                    else
                        dbTrait.Value = room.Traits.Get(dbTrait.Name).Value;
                }

                foreach (var roomTrait in room.Traits.GetAll())
                {
                    if (!existing.Traits.Exists(x => x.Name == roomTrait.Name))
                        existing.Traits.Add(new Trait { EntityId = existing.Id, Name = roomTrait.Name, Value = roomTrait.Value });
                }

                //update Components
                foreach (var dbComp in existing.Components)
                {
                    if (!room.Components.Has(dbComp.Component))
                        existing.Components.Remove(dbComp);
                }

                foreach (var roomComp in room.Components.GetAll())
                {
                    if (!existing.Components.Exists(x => x.Component == roomComp.Name))
                        existing.Components.Add(new Entity_Component { EntityId = existing.Id, Component = roomComp.Name });
                }

                //Update Entities
                foreach (var dbEntity in existing.Entities)
                {
                    if (!room.Entities.Contains(dbEntity.EntityId))
                        existing.Entities.Remove(dbEntity);
                }

                foreach (var roomEntity in room.Entities)
                {
                    if (!existing.Entities.Exists(x => x.EntityId == roomEntity))
                        existing.Entities.Add(new Entity_Room { EntityId = roomEntity, RoomId = existing.Id });
                }

                existing.RegionId = room.RegionId;
                _rooms.Update(existing);
                return room.Id;
            }
            
            var newRoom = new Room { Name = room.Name, RegionId = room.RegionId };
            newRoom.Traits = new List<Trait>();
            newRoom.Components = new List<Entity_Component>();
            newRoom.Entities = new List<Entity_Room>();

            foreach (var t in room.Traits.GetAll())
                newRoom.Traits.Add(new Trait { Name = t.Name, Value = t.Value });
            foreach (var c in room.Components.GetAll())
                newRoom.Components.Add(new Entity_Component { EntityId = room.Id, Component = c.Name });
            
            _rooms.Add(newRoom);

            foreach (var e in room.Entities)
                newRoom.Entities.Add(new Entity_Room { EntityId = e, RoomId = newRoom.Id });
            
            _rooms.Update(newRoom);
            return newRoom.Id;
        }
    }
}