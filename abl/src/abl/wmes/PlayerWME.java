package abl.wmes;

import java.awt.Point;

import wm.WME;
/**
 * Stores information about the player character.
 * 
 * In ABL: WME = Working memory element. WMEs are used to provide the agent with game state .
 * 
 * Note: ABL code can access WME properties using standard Java Bean naming conventions 
 * Example: locationX in ABL invoked getLocationX() 
 * 
 * @author Ben Weber 3-7-11
 */
public class PlayerWME extends WME {

	/** Location of the player */
	private Point location;
	
	/** Trajectory of the player */
	private Point trajactory;
	
	/**
	 * Instantiates a working memory element for tracking the player character.
	 */
	public PlayerWME(Point location, Point trajectory) {
		this.location = location;
		this.trajactory = trajectory;
	}
	
	/**
	 * Returns the x location of the player. 
	 */
	public int getLocationX() {
		return location.x;
	}
	
	/**
	 * Returns the y location of the player. 
	 */
	public int getLocationY() {
		return location.y;
	}
	
	/**
	 * Returns the x direction of the player. 
	 */
	public int getTrajectoryX() {
		return trajactory.x;
	}
	
	/**
	 * Returns the y direction of the player. 
	 */
	public int getTrajectoryY() {
		return trajactory.y;
	}
}
