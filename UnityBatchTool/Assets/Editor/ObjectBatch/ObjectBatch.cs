using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 查找场景中含有该MonoBehavior子类的所有对象
/// </summary>
namespace CTWCYS.Editor.Batch
{

    public class ObjectBatchWindow : EditorWindow
    {
       /// <summary>
       ///需要查找的脚本的类型
       /// </summary>
        List<Object> scriptObj = new List<Object>();
      /// <summary>
      /// 存放找到的东西的列表
      /// </summary>
        List<Object> gameObjResults = new List<Object>();
        /// <summary>
        /// 滑动条的位置
        /// </summary>
        Vector2 scrollPosition = Vector2.zero;

        /// <summary>
        /// 查找的脚本
        /// </summary>
        private MonoScript finder ;
        /// <summary>
        /// 处理的脚本
        /// </summary>
        private MonoScript batch ;
       
    
        [MenuItem("Finder/MonoFinder")]
        static void Init()
        {
           EditorWindow.GetWindow<ObjectBatchWindow>();
        }

       
        void OnGUI()
        {
            
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);            

            GUILayout.Label("拖拽一个继承FindInterface的脚本用于查找,可为空");
            finder = EditorGUILayout.ObjectField(finder,typeof(MonoScript),true) as MonoScript ;
            GUILayout.Label("拖拽一个继承BtachInterface的脚本批处理，可为空");
            batch =  EditorGUILayout.ObjectField(batch, typeof(MonoScript), true) as MonoScript;

            if (GUILayout.Button("添加新脚本。将会按该脚本的类型查找场景中的物体"))
            {
                 scriptObj.Add(new Object());                       
            }
            GUILayout.Label("添加脚本类型:");
            for (int i = 0; i < scriptObj.Count; i++)
            {              
                scriptObj[i] = EditorGUILayout.ObjectField(scriptObj[i], typeof(Object), true);
            }         
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("开始查找"))
            {
                gameObjResults.Clear();                              
               gameObjResults = new Finder().FindSenceAllGameObject(finder,scriptObj);
                Debug.Log("查找.完毕");
            }
            if (GUILayout.Button("执行批处理"))
            {
                 new Batch().Handle(batch,gameObjResults);            
                 Debug.Log("执行完毕");
            }
            GUILayout.EndHorizontal();
            if (gameObjResults.Count > 0)
            {
                for (int i = 0; i < gameObjResults.Count; i++)
                {
                    EditorGUILayout.ObjectField(gameObjResults[i], typeof(GameObject), false);
                }
                
            }
            else
            {
                GUILayout.Label("无数据");
            }
            GUILayout.EndScrollView();
        }

      
    }
}