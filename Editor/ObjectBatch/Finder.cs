using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CTWCYS.Editor.Batch
{
    public class Finder
    {
        /// <summary>
        /// 根据传入的查找脚本查找·
        /// </summary>
        /// <param name="script">查找脚本 </param>
        /// <param name="scriptClassTag">GameObject应包含的脚本 </param>
        /// <param name="gameObjResults">返回的结果 </param>
        public List<Object> FindSenceAllGameObject(MonoScript script,List<Object> scriptClassTag)
        {

           Type findscriptType = null;
           if (script != null) findscriptType = script.GetClass();
            bool containFindInterface = typeof(FindInterface).IsAssignableFrom(findscriptType);
            if (scriptClassTag.Count <= 0 && containFindInterface)
            {
                scriptClassTag.Add(new GameObject());
                return FindSenceGameObjectsWithFindType(findscriptType, scriptClassTag);               
            }
            else if(scriptClassTag.Count > 0 && containFindInterface)
            {               
                return FindSenceGameObjectsWithFindType(findscriptType, scriptClassTag);             
            }
            else if(scriptClassTag.Count > 0)
            {
                return FindSenceObjects(scriptClassTag);
            }
            else
            {
                return new List<Object>();
            }
             
        }
        /// <summary>
        /// 根据传入的查找脚本，与物体应挂的脚本筛选物体
        /// </summary>
        /// <param name="findscriptType"></param>
        /// <param name="scriptClassTag"></param>
        /// <returns></returns>
        public List<Object> FindSenceGameObjectsWithFindType(Type findscriptType, List<Object> scriptClassTag)
        {
            var gameObjects = FindSenceObjects(scriptClassTag);
            var results = new List<Object>();                 
            FindInterface find = findscriptType.Assembly.CreateInstance(findscriptType.FullName) as FindInterface;
            var list = find.Find(gameObjects);
            foreach (var item in list)
            {
                results.Add(item);
            }          
            return results;           
        }

       /// <summary>
       /// 根据物体所挂的脚本类型筛选物体
       /// </summary>
       /// <param name="scriptObjs"></param>
       /// <returns></returns>
        public List<Object> FindSenceObjects(List<Object> scriptObjs)
        {           
             var results = new List<Object>();
            scriptObjs.RemoveAll(obj => obj == null);
            Object[] cacheResuls = Object.FindObjectsOfType(IdentifyType(scriptObjs[0]));
            for (int caIndex = 0; caIndex < cacheResuls.Length; caIndex++)
            {
                var gameObj = cacheResuls[caIndex] as GameObject;
                if (gameObj == null)
                {
                    var compoent = cacheResuls[caIndex] as Component;
                    if(compoent != null)results.Add(compoent.gameObject);
                }
                else
                {
                    results.Add(cacheResuls[caIndex]);
                }
            }                
            for (int i = 1; i < scriptObjs.Count; i++)
            {
                var monoScript = scriptObjs[i] ;
                for (int index = 0; index < results.Count; index++)
                {
                    
                    var gameObject = results[index] as GameObject;
                    if (gameObject is GameObject)
                    {
                        var specialScript = gameObject.GetComponent(monoScript.GetType());
                        if (specialScript == null)
                        {
                            results[index] = null;                           
                        }
                    }
                    else
                    {
                        var componet = results[index] as Component;
                        if (componet is Component)
                        {
                            var specialScript = componet.GetComponent(monoScript.GetType());
                            if (specialScript == null)
                            {
                                results[index] = null;                              
                            }
                            else
                            {
                                results[index] = componet.gameObject;
                            }
                        }
                    }
                }
                results.RemoveAll(obj => obj == null);
            }            
            return results;
        }
        /// <summary>
        /// 返回传入参数的类型
        /// </summary>
        /// <param name="scriptType"> 可能是Monoscript的类型，也可能是MonoBehavior类型</param>
        /// <returns></returns>
        public Type  IdentifyType(Object scriptType)
        {
           
            var type = scriptType.GetType();
            if (type == typeof(MonoScript))
            {
                return (scriptType as MonoScript).GetClass();
            }
            else
            {
                return type;
            }          
        }
   }
}