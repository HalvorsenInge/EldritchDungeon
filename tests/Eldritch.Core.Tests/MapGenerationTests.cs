using Xunit;
using Eldritch.Core.Map;
using System.Collections.Generic;
using System.Linq;

namespace Eldritch.Core.Tests
{
    public class MapGenerationTests
    {
        [Fact]
        public void SameSeed_ProducesSameMap()
        {
            var g1 = new RandomDungeonGenerator(12345);
            var g2 = new RandomDungeonGenerator(12345);
            var m1 = g1.Generate(50,30);
            var m2 = g2.Generate(50,30);

            bool equal = true;
            for(int x=0;x<50 && equal;x++) for(int y=0;y<30 && equal;y++) if(m1.Get(x,y)!=m2.Get(x,y)) equal = false;
            Assert.True(equal, "Maps with same seed must be identical");
        }

        [Fact]
        public void Map_IsConnectedFromFirstRoomCenter()
        {
            var g = new RandomDungeonGenerator(999);
            var map = g.Generate(60,40);

            // find any floor tile as start
            (int sx,int sy) = (-1,-1);
            for(int x=0;x<map.Width && sx==-1;x++) for(int y=0;y<map.Height && sx==-1;y++) if(map.Get(x,y)==TileType.Floor){ sx=x; sy=y; }
            Assert.True(sx!=-1, "No floor tile found");

            // BFS to count reachable floors
            var visited = new bool[map.Width, map.Height];
            var q = new Queue<(int x,int y)>();
            q.Enqueue((sx,sy)); visited[sx,sy]=true; int count=0;
            while(q.Count>0){ var (x,y)=q.Dequeue(); count++; foreach(var n in map.Neighbors(x,y)){ if(!visited[n.x,n.y] && map.Get(n.x,n.y)==TileType.Floor){ visited[n.x,n.y]=true; q.Enqueue(n); } } }

            // number of floor tiles should be >0 and majority connected (simple heuristic)
            int totalFloors=0; for(int x=0;x<map.Width;x++) for(int y=0;y<map.Height;y++) if(map.Get(x,y)==TileType.Floor) totalFloors++;
            Assert.True(totalFloors>0, "No floors carved");
            Assert.True(count >= totalFloors * 0.6, $"Connected component too small: {count}/{totalFloors}");
        }
    }
}