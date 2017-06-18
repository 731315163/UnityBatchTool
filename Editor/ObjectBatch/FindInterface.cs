using System.Collections.Generic;
using  UnityEngine;


namespace CTWCYS.Editor.Batch
{
    interface FindInterface
    {
       IEnumerable<Object> Find(List<Object> obj);
    }
}
