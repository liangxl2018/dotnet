using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PrintStudioModel
{
    /// <summary>
    /// 可根据实际情况添加其他属性,如将打印份数或打印名称放于配置文件中.暂不实现.
    /// </summary>
    [Serializable]
    public class PrintFactoryModel : ICloneable
    {
        /// <summary>
        /// 标签宽
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 标签高
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 图片资源 如Hello.png;Gril.pcx;
        /// </summary>
        public string PicturResource { get; set; }

        /// <summary>
        /// 打印条目集合
        /// </summary>
        public List<PrintItemModel> PrintItems { get; set; }

        #region ICloneable 成员

        object ICloneable.Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                object CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
                CloneObject = bf.Deserialize(ms);
                // 关闭流 
                ms.Close();
                return CloneObject;
            }
        }

        public PrintFactoryModel Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PrintFactoryModel CloneObject;
                BinaryFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                // 反序列化至另一个对象(即创建了一个原对象的深表副本) 
                CloneObject = (PrintFactoryModel)bf.Deserialize(ms);
                // 关闭流 
                ms.Close();
                return CloneObject;
            }
        }

        #endregion
    }
}
