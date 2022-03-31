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
public class GameAgent_SuccessTests {
   static public boolean successTest0(int __$stepID, final Object[] __$behaviorFrame, final BehavingEntity __$thisEntity) {
      switch (__$stepID) {
         case 0: {
            // lookForAllyAgent_1Step1
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupWME("AllyWME");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  AllyWME wme__0 = (AllyWME)wmeIter0.next();
                  __$behaviorFrame[0] = wme__0;
                  if (
                     ( wme__0.getOnTree() == false )
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
         case 1: {
            // lookForAllyAgent_1Step2
               List wmeList0;
               ListIterator wmeIter0;
               wmeList0 = BehavingEntity.getBehavingEntity().lookupReflectionWMEBySignature("ParallelBehaviorWME", "ManageAllyAgent()");
               wmeIter0 = wmeList0.listIterator();
               while(wmeIter0.hasNext()) {
                  ParallelBehaviorWME wme__0 = (ParallelBehaviorWME)wmeIter0.next();
                  __$behaviorFrame[1] = wme__0;
                  if (
                     (wme__0.getSignature().equals("ManageAllyAgent()"))
                  )

                  {
                     return true;
                  }

               }


            return false;
         }
      default:
         throw new AblRuntimeError("Unexpected stepID " + __$stepID);
      }
   }
}
