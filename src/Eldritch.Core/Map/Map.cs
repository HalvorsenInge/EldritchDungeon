using System;
using System.Collections.Generic;

namespace Eldritch.Core.Map
{
    public enum TileType { Wall, Floor }

    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        private readonly TileType[,] _tiles;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = new TileType[width, height];
            for (int x = 0; x < width; x++) for (int y = 0; y < height; y++) _tiles[x,y] = TileType.Wall;
        }

        public TileType Get(int x, int y) => _tiles[x,y];
        public void Set(int x, int y, TileType t) => _tiles[x,y] = t;

        public IEnumerable<(int x,int y)> Neighbors(int x, int y)
        {
            var dirs = new (int dx,int dy)[] { (1,0),(-1,0),(0,1),(0,-1) };
            foreach(var (dx,dy) in dirs)
            {
                var nx = x+dx; var ny = y+dy;
                if(nx>=0 && nx<Width && ny>=0 && ny<Height) yield return (nx,ny);
            }
        }
    }
}