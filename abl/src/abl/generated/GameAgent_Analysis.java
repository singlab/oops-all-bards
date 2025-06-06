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
         case 0: {
            // vivAgentRoot_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 2: {
            // lookForVivCommands-1->ConditionalStep4_IF_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("manageVivCharacter(int)"); // stepID 5
            return _$analysisStepIDs;
         }
         case 1: {
            // lookForVivCommands_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("processSpawnGoals(int)"); // stepID 7
            return _$analysisStepIDs;
         }
         case 3: {
            // manageVivCharacter_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 5: {
            // processSpawnGoals-4->ConditionalStep11_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("investigateSuspiciousActivity(int, int)"); // stepID 13
            return _$analysisStepIDs;
         }
         case 6: {
            // processSpawnGoals-4->ConditionalStep14_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("neutralizeThreat(int, int)"); // stepID 16
            return _$analysisStepIDs;
         }
         case 7: {
            // processSpawnGoals-4->ConditionalStep17_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("gatherInformation(int, int)"); // stepID 19
            return _$analysisStepIDs;
         }
         case 8: {
            // processSpawnGoals-4->ConditionalStep20_IF_MentalStep_GoalStep_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("maintainGuildSecrecy(int, int)"); // stepID 22
            return _$analysisStepIDs;
         }
         case 4: {
            // processSpawnGoals_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 9: {
            // investigateSuspiciousActivity_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 10: {
            // neutralizeThreat_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 11: {
            // gatherInformation_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 12: {
            // maintainGuildSecrecy_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
         case 13: {
            // GameAgent_RootCollectionBehavior
            List<String> _$analysisStepIDs = new ArrayList<String>();
            _$analysisStepIDs.add("vivAgentRoot()"); // stepID 38
            _$analysisStepIDs.add("lookForVivCommands()"); // stepID 39
            return _$analysisStepIDs;
         }
         case 14: {
            // __$defaultMemoryExecuteBehavior_1
            List<String> _$analysisStepIDs = new ArrayList<String>();
            return _$analysisStepIDs;
         }
      default:
         throw new AblRuntimeError("Unexpected behaviorID " + __$behaviorID);
      }
   }
}
