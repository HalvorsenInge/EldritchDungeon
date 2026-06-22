using Eldritch.Core.Map;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;

namespace Eldritch.Core.Rendering
{
    public static class AsciiRenderer
    {
        // Renders a simple ASCII buffer: Wall='#', Floor='.', Entity='E'
        public static char[,] Render(Map.Map map, EntityManager manager)
        {
            var w = map.Width;
            var h = map.Height;
            var buf = new char[w, h];
            for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                buf[x, y] = map.Get(x, y) == TileType.Wall ? '#' : '.';
            }

            var entities = manager.QueryByComponent<PositionComponent>();
            foreach (var e in entities)
            {
                var p = e.GetComponent<PositionComponent>();
                if (p != null && p.X >= 0 && p.X < w && p.Y >= 0 && p.Y < h)
                {
                    buf[p.X, p.Y] = 'E';
                }
            }

            return buf;
        }
    }
}
