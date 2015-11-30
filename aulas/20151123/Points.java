import java.io.*;
import java.util.*;

public class Points {
	
	public static class Point {
		
		private int x;
		private int y;
		
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
		
		public int getX() { return x; }
		public int getY() { return y; }
		
		public void printTo(PrintStream out) {
			out.printf("{ x: %d; y: %d }\n", x, y);
		}
		
		public void translate(int dx, int dy) {
			x += dx;
			y += dy;
		}
	}
	
	public static class ImmPoint {
		
		private final int x;
		private final int y;
		
		public ImmPoint(int x, int y) {
			this.x = x;
			this.y = y;
		}
		
		public int getX() { return x; }
		public int getY() { return y; }
		
		public void printTo(PrintStream out) {
			out.printf("{ x: %d; y: %d }\n", x, y);
		}
		
		public ImmPoint translate(int dx, int dy) {
			return new ImmPoint(x + dx, y + dy);
		}
	}
	
	public static class Path {
		
		private Point[] points;
		// ...
		
		public Path(Point[] path) {
			points = path;
		}
		
		public Point[] getPoints() {
			return points;
		}
		
		// ...
		
		public void addPoint(Point p) {
			Point[] newPoints = Arrays.copyOf(points, points.length + 1);
			newPoints[points.length] = p;
			points = newPoints;
		}
	}
	
	public static class ImmPath {
		
		private final ImmPoint[] points;
		// ...
		
		public ImmPath(ImmPoint[] path) {
			points = path.clone();
		}
		
		public ImmPoint[] getPoints() {
			return points.clone();
		}
		
		public int getPathLength() {
			return points.length;
		}
	
		public ImmPoint getPoint(int idx) {
			return points[idx];
		}
		
		// ...
		
		public ImmPath addPoint(ImmPoint p) {
			ImmPoint[] newPoints = Arrays.copyOf(points, points.length + 1);
			newPoints[points.length] = p;
			return new ImmPath(newPoints);
		}
	}
}
