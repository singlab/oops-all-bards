package abl.actions;

import game.GameEngine;

import java.awt.Point;
/**
 * Fires a bullet. 
 * 
 * @author Ben Weber 3-7-11
 */
public class Fire extends BaseAction {

	/**
	 * Fires a bullet at the target location.
	 * 
	 * Args:
	 *  - 0: source x position
	 *  - 1: source y position
	 *  - 2: target x position
	 *  - 3: target y position
	 */
	public void execute(Object[] args) {
		GameEngine.getInstance().fireBullet(
				new Point((Integer)args[0], (Integer)args[1]), 
				new Point((Integer)args[2], (Integer)args[3]));
	}
}
