package abl.generated;

import abl.runtime.*;
import wm.WME;
import wm.WorkingMemorySet;
import wm.WMEIndex;
import wm.TrackedWorkingMemory;
import java.util.*;
import java.lang.reflect.Method;
import java.lang.reflect.Field;
import abl.learning.*;
import abl.wmes.*;
import abl.actions.*;
import abl.sensors.*;
public class GameAgent_Analysis {
   static public List<String> analysis0(int __$behaviorID) {
      switch (__$behaviorID) {
         case 0: {
            // lookForAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("createAllyAgent(AllyWME)"); // stepID 2
            return _$analysisStepIDs;
         }
         case 1: {
            // createAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 2: {
            // manageAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 3: {
            // GameAgent_RootCollectionBehavior
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("lookForAllyAgent()"); // stepID 5
            _$analysisStepIDs.add("manageAllyAgent()"); // stepID 6
            return _$analysisStepIDs;
         }
         case 4: {
            // __$defaultMemoryExecuteBehavior_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
