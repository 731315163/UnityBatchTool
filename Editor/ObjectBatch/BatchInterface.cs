using UnityEngine;

namespace CTWCYS.Editor.Batch
{
    /// <summary>
    /// 继承的子类请写显示初始化函数
    /// </summary>
    [SerializeField]
    interface BatchInterface
    {
       void  Batch(Object obj);
    }
}
