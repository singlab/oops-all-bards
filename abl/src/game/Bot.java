package game;

import java.awt.Color;
import java.awt.Point;

public class Bot {
 
	public static final int Size = 10;
	
	static int IdCount = 0;
	
	/** Location of the bot */
	private Point location;
	
	/** Trajectory of the bot */
	private Point trajectory;
	
	/** unique bot ID */
	private int id;

	private Color color;
	
	private int size = Size;
	
	public Bot() {
		this.location = new Point(0,0);
		this.trajectory = new Point(0,0);
		this.id = IdCount++;
		this.color = new Color(255, 165, 0);
	}

	public Point getLocation() {
		return location;
	}

	public void setLocation(Point location) {
		this.location = location;
	}

	public Point getTrajectory() {
		return trajectory;
	}

	public void setTrajectory(Point trajectory) {
		this.trajectory = trajectory;
	}
	
	public int getX( ) {
		return this.location.x;
	}
	
	public int getY( ) {
		return this.location.y;
	}
	
	public int getId() {
		return id;
	}

	public int getSize() {
		return size;
	}

	public void setSize(int size) {
		this.size = size;
	}

	public Color getColor() {
		return color;
	}

	public void setColor(Color color) {
		this.color = color;
	}
}
