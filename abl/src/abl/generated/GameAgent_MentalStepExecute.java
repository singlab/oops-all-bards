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
public class GameAgent_MentalStepExecute {
   static public void mentalExecute0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity, MentalStep __$thisStep) {
      switch (__$stepID) {
         case 0: {
            // lookForVivCommands_1Step1
            System.out.println("[ABL DEBUG] lookForVivCommands: Starting and waiting for a new command...");
            break;
         }
         case 2: {
            // lookForVivCommands_1Step3
            ((__ValueTypes.IntVar)__$behaviorFrame[2]).i = ((VivWME)__$behaviorFrame[0]).getID();
            System.out.println("[ABL DEBUG] lookForVivCommands: Found VivWME for character ID " + ((__ValueTypes.IntVar)__$behaviorFrame[2]).i + ". Marking as OnTree=true.");
            ((VivWME)__$behaviorFrame[0]).setOnTree(true);
            break;
         }
         case 3: {
            // lookForVivCommands_1Step4
            System.out.println("[ABL DEBUG] lookForVivCommands: Updating dictionary for character " + ((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            ((GameAgent)__$thisEntity).dict.addCharacter(((__ValueTypes.IntVar)__$behaviorFrame[2]).i , ((VivWME)__$behaviorFrame[0]));
            break;
         }
         case 5: {
            // lookForVivCommands-0->ConditionalStep4_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[3]).i = ((VivWME)__$behaviorFrame[0]).getTargetIdForGoal("investigateSuspiciousActivity");
            System.out.println("[ABL DEBUG] processSpawnGoals: Spawning 'investigateSuspiciousActivity' with target " + ((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            break;
         }
         case 8: {
            // lookForVivCommands-0->ConditionalStep7_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[3]).i = ((VivWME)__$behaviorFrame[0]).getTargetIdForGoal("neutralizeThreat");
            System.out.println("[ABL DEBUG] processSpawnGoals: Spawning 'neutralizeThreat' with target " + ((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            break;
         }
         case 11: {
            // lookForVivCommands-0->ConditionalStep10_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[3]).i = ((VivWME)__$behaviorFrame[0]).getTargetIdForGoal("gatherInformation");
            System.out.println("[ABL DEBUG] processSpawnGoals: Spawning 'gatherInformation' with target " + ((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            break;
         }
         case 14: {
            // lookForVivCommands-0->ConditionalStep13_IF_MentalStep_GoalStep_1Step1
            ((__ValueTypes.IntVar)__$behaviorFrame[3]).i = ((VivWME)__$behaviorFrame[0]).getTargetIdForGoal("maintainGuildSecrecy");
            System.out.println("[ABL DEBUG] processSpawnGoals: Spawning 'maintainGuildSecrecy' with target " + ((__ValueTypes.IntVar)__$behaviorFrame[3]).i);
            break;
         }
         case 16: {
            // lookForVivCommands_1Step9
            System.out.println("ABL: Finished processing command for character " + ((__ValueTypes.IntVar)__$behaviorFrame[2]).i + ", removing from dictionary.");
            ((GameAgent)__$thisEntity).dict.deleteCharacter(((__ValueTypes.IntVar)__$behaviorFrame[2]).i);
            break;
         }
         case 18: {
            // investigateSuspiciousActivity_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is investigating " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 23: {
            // neutralizeThreat_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is neutralizing threat " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 26: {
            // gatherInformation_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is gathering info on " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
         case 29: {
            // maintainGuildSecrecy_1Step1
            System.out.println("ABL Tactic: Character " + ((__ValueTypes.IntVar)__$behaviorFrame[0]).i + " is maintaining secrecy from " + ((__ValueTypes.IntVar)__$behaviorFrame[1]).i);
            break;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
