using System.Collections.Generic;
using System.Linq;

namespace G3 {
    public class Program {
        public static void Main(string[] args) {
            var hive = new Hive();
            hive.AddTeleport(new Point(1, 1), new Point(10, 6));
            hive.Print();
            var list = FindPath(hive, new Point(3, 2), new Point(9, 4));
            hive.Print(list);
        }

        public static List<Point> FindPath(Hive hive, Point from, Point to) {
            var path = new Dictionary<Point, Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(to);
            path[to] = null;
            while (queue.Count != 0) {
                var point = queue.Dequeue();
                var nextPoints = hive
                    .AreaOfPoint(point.X, point.Y)
                    .Where(hive.IsCouldVisited)
                    .Where(p => !path.ContainsKey(p));
                foreach (var nextPoint in nextPoints) {
                    queue.Enqueue(nextPoint);
                    path[nextPoint] = point;
                }
                if (path.ContainsKey(from)) break;
            }
            var list = new List<Point>() { from };
            while (from != null) 
                list.Add(from = path[from]);
            return list;
        }
    }
}
