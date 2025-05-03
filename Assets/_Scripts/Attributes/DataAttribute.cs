using System;

namespace Demo.Core
{
    /// <summary>
    /// 用于标记某个数据类是可导入的
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataImportable : Attribute
    {
        public string Tag { get; }

        public DataImportable(string tag)
        {
            Tag = tag.ToLower();
        }
    }
}