package abl.util;

import java.util.*;
import wm.WME;

public class WMEDictionary<T extends WME> {

	private HashMap<Integer, T> map;

	public WMEDictionary() {
		this.setMap(new HashMap<Integer, T>());
	}

	public HashMap<Integer, T> getMap() {
		return map;
	}

	public void setMap(HashMap<Integer, T> map) {
		this.map = map;
	}

	public void addCharacter(int key, T value) {
		this.map.put(key, value);
	}

	public void deleteCharacter(int key) {
		this.map.remove(key);
	}

	public T getCharacter(int key) {
		return this.map.get(key);
	}

	public Integer[] getCharacters() {
		Integer arr[] = new Integer[this.map.size()];
		this.map.keySet().toArray(arr);
		return arr;
	}

	public boolean containsKey(int key) {
		return this.map.containsKey(key);
	}

	public boolean isEmpty() {
		return this.map.isEmpty();
	}
}
