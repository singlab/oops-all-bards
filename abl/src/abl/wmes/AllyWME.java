package abl.wmes;

import wm.WME;

public class AllyWME extends WME {
	/** Whether or not this WME is on an active behavior tree */
	private boolean onTree;
	/** ID of the ally character */
	private int id;
	/** Whether or not the ally is in combat */
	private boolean inCombat;
	/** The x location of the ally */
	private float locationX;
	/** The y location of the ally */
	private float locationY;
	/** The z location of the ally */
	private float locationZ;
	/** The ally character's health */
	private int health;
	/** The ally character's flourish points */
	private int flourish;
	/** The ally character's shield */
	private int shield;
	/** The ally character's ability IDs in array form */
	private int[] abilityIDs;
	/** The ally character's ability costs in array form */
	private int[] abilityCosts;
	/** The ally character's combat ability types in array form */
	private String[] abilityTypes;
	/** Whether this ally owns the current turn */
	private boolean ownsTurn;
	/** The character IDs of the current party members */
	private int[] partyIDs;
	/** The affinity values this ally has for other party members */
	private int[] partyAffinities;
	/** The status IDs of this ally */
	private int[] statusIDs;
	/** The trait IDs of this ally */
	private int[] traitIDs;
	
	/**
	 * Instantiates a working memory element for tracking an ally.
	 */
	public AllyWME(int id, boolean inCombat, float locationX, float locationY, float locationZ, int health, 
			int flourish, int shield, int[] abilityIDs, int[] abilityCosts, String[] abilityTypes, boolean ownsTurn, 
			int[] partyIDs, int[] partyAffinities, int[] statusIDs, int[] traitIDs) {
		this.onTree = false;
		this.id = id;
		this.inCombat = inCombat;
		this.locationX = locationX;
		this.locationY = locationY;
		this.locationZ = locationZ;
		this.health = health;
		this.flourish = flourish;
		this.shield = shield;
		this.abilityIDs = abilityIDs;
		this.abilityCosts = abilityCosts;
		this.abilityTypes = abilityTypes;
		this.ownsTurn = ownsTurn;
		this.partyIDs = partyIDs;
		this.partyAffinities = partyAffinities;
		this.statusIDs = statusIDs;
		this.traitIDs = traitIDs;
	}
	
	public boolean getOnTree() { return onTree; }
	
	public int getID() { return id; }
	
	public boolean getInCombat() { return inCombat; }
	
	public float getLocationX() { return locationX; }
	
	public float getLocationY() { return locationY; }
	
	public float getLocationZ() { return locationZ; }
	
	public int getHealth() { return health; }
	
	public int getFlourish() { return flourish; }
	
	public int getShield() { return shield; }
	
	public int[] getAbilityIDs() { return abilityIDs; }
	
	public int[] getAbilityCosts() { return abilityCosts; }
	
	public String[] getAbilityTypes() { return abilityTypes; }

	public boolean getOwnsTurn() { return ownsTurn; }
	
	public int[] getPartyIDs() { return partyIDs; }
	
	public int[] getPartyAffinities() { return partyAffinities; }
	
	public int[] getStatusIDs() { return statusIDs; }
	
	public int[] getTraitIDs() { return traitIDs; }
	
	public void setOnTree(boolean value) { this.onTree = value; }
}
