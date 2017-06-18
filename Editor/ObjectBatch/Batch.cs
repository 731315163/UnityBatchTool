using System;
using System.Collections.Generic;
using UnityEditor;
using  UnityEngine;
using  Object = UnityEngine.Object;

namespace CTWCYS.Editor.Batch
{
    public class Batch
    {
        
        public void Handle(MonoScript batchscript,List<Object> gameObjects)
        {
             var batchscriptType = batchscript.GetClass();
            BatchInterface batch = batchscriptType.Assembly.CreateInstance(batchscriptType.FullName) as BatchInterface;

            if (batch == null)
            {
                Debug.LogError("传入的不是继承了接口的批处理脚本,报错...");
            }
            
            for (int i = 0; i < gameObjects.Count; i++)
            {            
               batch.Batch(gameObjects[i]);
            }
          
        }
       
    }
}
