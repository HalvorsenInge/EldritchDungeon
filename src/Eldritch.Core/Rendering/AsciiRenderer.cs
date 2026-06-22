using Eldritch.Core.Map;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;

namespace Eldritch.Core.Rendering
{
    public static class AsciiRenderer
    {
        // Renders the full map by delegating to the viewport renderer
        public static char[,] Render(Map.Map map, EntityManager manager)
        {
            return RenderViewport(map, manager, 0, 0, map.Width, map.Height);
        }

        // Renders a viewport region [vx..vx+vw) x [vy..vy+vh)
        public static char[,] RenderViewport(Map.Map map, EntityManager manager, int vx, int vy, int vw, int vh)
        {
            var buf = new char[vw, vh];
            for (int x = 0; x < vw; x++)
            for (int y = 0; y < vh; y++)
            {
                int mx = vx + x;
                int my = vy + y;
                if (mx >= 0 && mx < map.Width && my >= 0 && my < map.Height)
                    buf[x, y] = map.Get(mx, my) == TileType.Wall ? '#' : '.';
                else
                    buf[x, y] = ' ';
            }

            var entities = manager.QueryByComponent<PositionComponent>();
            foreach (var e in entities)
            {
                var p = e.GetComponent<PositionComponent>();
                if (p != null)
                {
                    int rx = p.X - vx;
                    int ry = p.Y - vy;
                    if (rx >= 0 && rx < vw && ry >= 0 && ry < vh)
                    {
                        buf[rx, ry] = 'E';
                    }
                }
            }

            return buf;
        }
    }
}
