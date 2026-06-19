using System;
using System.Collections.Generic;
using System.Linq;

namespace Eldritch.Core.Map
{
    internal record Room(int X, int Y, int W, int H)
    {
        public int CenterX => X + W/2;
        public int CenterY => Y + H/2;
        public bool Intersects(Room other) => !(other.X + other.W <= X || other.X >= X + W || other.Y + other.H <= Y || other.Y >= Y + H);
    }

    public class RandomDungeonGenerator
    {
        private readonly Random _rand;
        public RandomDungeonGenerator(int seed) => _rand = new Random(seed);

        public Map Generate(int width, int height, int roomAttempts = 50, int minRoom=4, int maxRoom=10)
        {
            var map = new Map(width, height);
            var rooms = new List<Room>();

            for (int i=0;i<roomAttempts;i++)
            {
                var rw = _rand.Next(minRoom, maxRoom+1);
                var rh = _rand.Next(minRoom, maxRoom+1);
                var rx = _rand.Next(1, Math.Max(1, width - rw - 1));
                var ry = _rand.Next(1, Math.Max(1, height - rh - 1));
                var room = new Room(rx, ry, rw, rh);
                if (rooms.Any(r => r.Intersects(room))) continue;
                rooms.Add(room);
                CarveRoom(map, room);
            }

            if (rooms.Count == 0)
            {
                // fallback: carve a small central room
                var room = new Room(width/2 -2, height/2 -2, 4,4);
                rooms.Add(room);
                CarveRoom(map, room);
            }

            // connect rooms by straight corridors between centers
            var ordered = rooms.OrderBy(r => r.CenterX).ToList();
            for (int i=1;i<ordered.Count;i++)
            {
                var a = ordered[i-1];
                var b = ordered[i];
                CarveCorridor(map, a.CenterX, a.CenterY, b.CenterX, b.CenterY);
            }

            return map;
        }

        private void CarveRoom(Map map, Room r)
        {
            for (int x = r.X; x < r.X + r.W; x++)
                for (int y = r.Y; y < r.Y + r.H; y++)
                    map.Set(x,y, TileType.Floor);
        }

        private void CarveCorridor(Map map, int x1, int y1, int x2, int y2)
        {
            var x = x1; var y = y1;
            while (x != x2)
            {
                map.Set(x,y, TileType.Floor);
                x += Math.Sign(x2 - x);
            }
            while (y != y2)
            {
                map.Set(x,y, TileType.Floor);
                y += Math.Sign(y2 - y);
            }
        }
    }
}