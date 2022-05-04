package abl.util;

import java.util.*;
import abl.wmes.AllyWME;

public class WMEDictionary {
	private HashMap<Integer, AllyWME> map;
	
	public WMEDictionary() {
		this.setMap(new HashMap<Integer, AllyWME>());
	}

	public HashMap<Integer, AllyWME> getMap() {
		return map;
	}

	public void setMap(HashMap<Integer, AllyWME> map) {
		this.map = map;
	}
	
	public void addCharacter(int key, AllyWME value) {
		this.map.putIfAbsent(key, value);
	}
	
	public void deleteCharacter(int key) {
		this.map.remove(key);
	}
	
	public void updateCharacter(int key, AllyWME value) {
		this.map.replace(key, value);
	}
	
	public AllyWME getCharacter(int key) {
		return this.map.get(key);
	}
}
