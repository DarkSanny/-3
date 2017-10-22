using System;
using System.Collections.Generic;
using System.Linq;

namespace G3
{

    public class Point {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) {
            X = x;
            Y = y;
        }

        public override string ToString() {
            return X + ":" + Y;
        }

        public override bool Equals(object obj) {
            if (!(obj is Point point)) return false;
            return X == point.X && Y == point.Y;
        }

        public override int GetHashCode() {
            return X ^ 97 + Y;
        }
    }

    public class Hive {

        private bool[][] hive;
        private Dictionary<Point, Point> teleports;

        public int Height => hive.Length;

        public int Width (int x) {
            if (x < Height)
                return hive[x].Length;
            return -1;
        }

        public bool IsCorrectPoint(Point point) {
            return point.Y >= 0 && point.Y < hive.Length 
                && point.X >= 0 && point.X < hive[point.Y].Length;
        }

        public IEnumerable<Point> AreaOfPoint(int x, int y) {
            for (var i = -1; i <= 1; i++) {
                if (i % 2 != 0) {
                    var startX = x - 1;
                    if (y % 2 != 0) startX++;
                    yield return new Point(startX, y + i);
                    yield return new Point(startX+1, y + i);
                } else
                    for (var j = -1; j <= 1; j++)
                        yield return new Point(x + j, y); 
            }
            if (teleports.ContainsKey(new Point(x, y)))
                yield return teleports[new Point(x, y)];
        }

        public bool IsCouldVisited(Point point) {
            if (IsCorrectPoint(point)) return hive[point.Y][point.X];
            return false;
        }

        public Hive() {
            var maze = new[] {
               "############",
               "#.........#",
               "#..........#",
               "#.........#",
               "#..........#",
               "#.........#",
               "#..........#",
               "###########"
               };
            hive = maze
                .Select(i => i
                    .Select(j => j != '#')
                    .ToArray())
                .ToArray();
            teleports = new Dictionary<Point, Point>();
        }

        public bool AddTeleport(Point from, Point to) {
            if (!IsCorrectPoint(from) || !IsCorrectPoint(to)) return false;
            if (teleports.ContainsKey(from) || teleports.ContainsKey(to)) return false;
            teleports[from] = to;
            teleports[to] = from;
            return true;
        }

        public void Print() {
            for (int i = 0; i < hive.Length; i++) {
                for (int j = 0; j < hive[i].Length; j++) {
                    var symbol = (hive[i][j] ? '.' : '#');
                    if (teleports.ContainsKey(new Point(j, i))) symbol = 'O';
                    if (i % 2 != 0) Console.Write(" " + symbol);
                    else Console.Write(symbol + " ");
                }
                Console.WriteLine();
            }
        }

        public void Print(List<Point> path) {
            for (var i = 0; i < hive.Length; i++) {
                for (var j = 0; j < hive[i].Length; j++) {
                    Console.ForegroundColor = path.Contains(new Point(j, i)) ? ConsoleColor.Red : ConsoleColor.White;
                    var symbol = (hive[i][j] ? '.' : '#');
                    if (teleports.ContainsKey(new Point(j, i))) symbol = 'O';
                    if (i % 2 != 0) Console.Write(" " + symbol);
                    else Console.Write(symbol + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
