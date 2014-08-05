using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    public delegate IEnumerator CoRoutine(System.Object obj);


    public interface ICoRoutineRunner
    {
        void RunCoRoutine(CoRoutine routine);

        void RunCoRoutine(CoRoutine routine, Object Obj);
    }

}
