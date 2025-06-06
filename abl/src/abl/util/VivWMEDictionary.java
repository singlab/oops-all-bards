package abl.util;

import abl.wmes.VivWME;

/**
 * A concrete, non-generic wrapper of WMEDictionary that is specifically
 * for holding VivWME objects. This is needed because the ABL parser cannot
 * handle generic syntax (i.e. "<VivWME>") in its declarations.
 */
public class VivWMEDictionary extends WMEDictionary<VivWME> {
}