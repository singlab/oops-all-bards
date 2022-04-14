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
            _$analysisStepIDs.add("manageAllyAgent(AllyWME)"); // stepID 1
            return _$analysisStepIDs;
         }
         case 1: {
            // allyAgentRoot_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 2: {
            // manageAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("manageCombat(AllyWME)"); // stepID 4
            return _$analysisStepIDs;
         }
         case 3: {
            // manageCombat_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("lookToAssist(AllyWME)"); // stepID 6
            _$analysisStepIDs.add("waitForTurn(AllyWME)"); // stepID 7
            return _$analysisStepIDs;
         }
         case 5: {
            // waitForTurn-4->AnonymousStep10_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("takeTurn(AllyWME)"); // stepID 11
            return _$analysisStepIDs;
         }
         case 4: {
            // waitForTurn_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 7: {
            // lookToAssist-6->AnonymousStep14_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("assistAlly(AllyWME, AllyWME)"); // stepID 15
            return _$analysisStepIDs;
         }
         case 6: {
            // lookToAssist_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 8: {
            // takeTurn_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 9: {
            // assistAlly_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 10: {
            // GameAgent_RootCollectionBehavior
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("lookForAllyAgent()"); // stepID 19
            return _$analysisStepIDs;
         }
         case 11: {
            // __$defaultMemoryExecuteBehavior_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
