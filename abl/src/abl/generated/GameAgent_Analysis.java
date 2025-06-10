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
import java.util.HashMap;
public class GameAgent_Analysis {
   static public List<String> analysis0(int __$behaviorID) {
      switch (__$behaviorID) {
         case 1: {
            // lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("investigateSuspiciousActivity(int, int)"); // stepID 6
            return _$analysisStepIDs;
         }
         case 2: {
            // lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("neutralizeThreat(int, int)"); // stepID 9
            return _$analysisStepIDs;
         }
         case 3: {
            // lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("gatherInformation(int, int)"); // stepID 12
            return _$analysisStepIDs;
         }
         case 4: {
            // lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("maintainGuildSecrecy(int, int)"); // stepID 15
            return _$analysisStepIDs;
         }
         case 0: {
            // lookForVivCommands_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 5: {
            // vivAgentRoot_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 6: {
            // investigateSuspiciousActivity_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 7: {
            // neutralizeThreat_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 8: {
            // gatherInformation_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 9: {
            // maintainGuildSecrecy_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 10: {
            // GameAgent_RootCollectionBehavior
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("vivAgentRoot()"); // stepID 32
            _$analysisStepIDs.add("lookForVivCommands()"); // stepID 33
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
