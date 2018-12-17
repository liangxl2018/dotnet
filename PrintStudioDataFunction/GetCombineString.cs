using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;

namespace PrintStudioDataFunction
{
    public class GetCombineString : IDataFunction
    {
        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <param name="templetModels"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public string DataParseFuntion(List<PrintItemModel> templetModels, List<int> indexs)
        {
            try
            {
                if (indexs == null || indexs.Count < 1)
                {
                    throw new Exception(string.Format("请正确设置使用FunctionName=\"{0}\"条目的FunctionIndexs值.", this.GetType().Name));
                } 
                string value = string.Empty;
                string reValue = string.Empty;
                foreach (int item in indexs)
                {
                    PrintItemModel temp = templetModels.Where(p => { return p.Index == item; }).ElementAt(0);
                    if (temp.DataSourceType != 0)
                    {
                        if (temp.PrintKeyValue.Contains("{0}"))
                        {
                            throw new Exception(string.Format("请先放置{0}标签,再放置合并后的标签.", temp.Index));
                        }
                    }
                    reValue += temp.PrintKeyValue;
                }
                return reValue;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
