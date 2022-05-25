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
import abl.util.*;
public class GameAgent_Analysis {
   static public List<String> analysis0(int __$behaviorID) {
      switch (__$behaviorID) {
         case 1: {
            // lookForAllyAgent-0->ConditionalStep3_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("manageAllyAgent(AllyWME)"); // stepID 5
            return _$analysisStepIDs;
         }
         case 0: {
            // lookForAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 2: {
            // allyAgentRoot_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 3: {
            // manageAllyAgent_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("manageCombat(AllyWME)"); // stepID 8
            _$analysisStepIDs.add("manageNoncombat(AllyWME)"); // stepID 9
            return _$analysisStepIDs;
         }
         case 4: {
            // manageCombat_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("lookToAssist(AllyWME)"); // stepID 12
            _$analysisStepIDs.add("waitForTurn(AllyWME)"); // stepID 13
            return _$analysisStepIDs;
         }
         case 5: {
            // waitForTurn_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("takeTurn(AllyWME)"); // stepID 16
            return _$analysisStepIDs;
         }
         case 6: {
            // lookToAssist_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("assistAlly(AllyWME, AllyWME)"); // stepID 19
            return _$analysisStepIDs;
         }
         case 7: {
            // takeTurn_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 8: {
            // assistAlly_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 9: {
            // manageNoncombat_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("lookForQuip(AllyWME)"); // stepID 25
            _$analysisStepIDs.add("waitForTurn(AllyWME)"); // stepID 26
            return _$analysisStepIDs;
         }
         case 10: {
            // lookForQuip_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 11: {
            // GameAgent_RootCollectionBehavior
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("allyAgentRoot()"); // stepID 31
            _$analysisStepIDs.add("lookForAllyAgent()"); // stepID 32
            return _$analysisStepIDs;
         }
         case 12: {
            // __$defaultMemoryExecuteBehavior_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
