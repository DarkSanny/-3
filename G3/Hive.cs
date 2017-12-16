using System;
using System.Collections.Generic;
using System.Linq;

namespace G3 {

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
	        return obj is Point point && (X == point.X && Y == point.Y);
        }

        public override int GetHashCode() {
            return X ^ 97 + Y;
        }
    }

    public class Hive {

        private readonly bool[][] _hive;
        private readonly Dictionary<Point, Point> _teleports;

        public int Height => _hive.Length;

	    public int Width (int x) {
            if (x < Height)
                return _hive[x].Length;
            return -1;
        }

        public bool IsCorrectPoint(Point point) {
            return point.Y >= 0 && point.Y < _hive.Length 
                && point.X >= 0 && point.X < _hive[point.Y].Length;
        }

        public IEnumerable<Point> AreaOfPoint(int x, int y) {
            for (var i = -1; i <= 1; i++) {
                if (i % 2 != 0) {
                    var startX = x - 1;
                    if (y % 2 != 0) startX++;
                    yield return new Point(startX++, y + i);
                    yield return new Point(startX++, y + i);
                } else
                    for (var j = -1; j <= 1; j++)
                        yield return new Point(x + j, y); 
            }
            if (_teleports.ContainsKey(new Point(x, y)))
                yield return _teleports[new Point(x, y)];
        }

        public bool IsCouldVisited(Point point) {
            if (IsCorrectPoint(point)) return _hive[point.Y][point.X];
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
            _hive = maze
                .Select(i => i
                    .Select(j => j != '#')
                    .ToArray())
                .ToArray();
            _teleports = new Dictionary<Point, Point>();
        }

        public bool AddTeleport(Point from, Point to) {
            if (!IsCorrectPoint(from) || !IsCorrectPoint(to)) return false;
            if (_teleports.ContainsKey(from) || _teleports.ContainsKey(to)) return false;
            _teleports[from] = to;
            _teleports[to] = from;
            return true;
        }

        public void Print() {
            for (int i = 0; i < _hive.Length; i++) {
                for (int j = 0; j < _hive[i].Length; j++) {
                    var symbol = (_hive[i][j] ? '.' : '#');
                    if (_teleports.ContainsKey(new Point(j, i))) symbol = 'O';
                    if (i % 2 != 0) Console.Write(" " + symbol);
                    else Console.Write(symbol + " ");
                }
                Console.WriteLine();
            }
        }

        public void Print(List<Point> path) {
            for (var i = 0; i < _hive.Length; i++) {
                for (var j = 0; j < _hive[i].Length; j++) {
                    Console.ForegroundColor = path.Contains(new Point(j, i)) ? ConsoleColor.Red : ConsoleColor.White;
                    var symbol = (_hive[i][j] ? '.' : '#');
                    if (_teleports.ContainsKey(new Point(j, i))) symbol = 'O';
                    if (i % 2 != 0) Console.Write(" " + symbol);
                    else Console.Write(symbol + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
