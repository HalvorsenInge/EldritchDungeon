using System.Linq;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;
using Eldritch.Core.Input;
using Eldritch.Core.Map;

namespace Eldritch.Core.Systems
{
    public class UpdateEngine
    {
        // Resolve one game turn: process intents deterministically by entity id
        // If a map is provided, block movement into walls or out-of-bounds tiles.
        public void ResolveTurn(EntityManager manager, Map.Map? map = null)
        {
            var entities = manager.QueryByComponent<IntentComponent>().OrderBy(e => e.Id).ToList();
            foreach(var e in entities)
            {
                var intent = e.GetComponent<IntentComponent>();
                var pos = e.GetComponent<PositionComponent>();
                if (intent == null) continue;

                if (pos != null)
                {
                    int targetX = pos.X, targetY = pos.Y;
                    switch (intent.Command.Name)
                    {
                        case "MoveUp": targetY -= 1; break;
                        case "MoveDown": targetY += 1; break;
                        case "MoveLeft": targetX -= 1; break;
                        case "MoveRight": targetX += 1; break;
                        case "Wait": break;
                        default: break;
                    }

                    // If a map is provided, ensure target is inside bounds and floor
                    var allowMove = true;
                    if (map != null)
                    {
                        if (targetX < 0 || targetY < 0 || targetX >= map.Width || targetY >= map.Height)
                        {
                            allowMove = false;
                        }
                        else
                        {
                            allowMove = map.Get(targetX, targetY) == TileType.Floor;
                        }
                    }

                    if (allowMove)
                    {
                        pos.X = targetX;
                        pos.Y = targetY;
                    }
                }

                // remove intent after processing
                e.RemoveComponent<IntentComponent>();
            }
        }
    }
}