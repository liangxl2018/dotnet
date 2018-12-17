using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;
using PrintStudioRule;


namespace PrintStudioDataFunction
{
    public class GetRandomNumber : IDataFunction
    {
        public string DataParseFuntion(List<PrintItemModel> templetModels, List<int> indexs)
        {
            try
            {
                if (indexs == null || indexs.Count < 1)
                {
                    throw new Exception(string.Format("请正确设置数据条目中使用FunctionName=\"{0}\"的ProviderIndexs值.", this.GetType().Name));
                }
                string reValue = string.Empty;
                int index = indexs[0];

                PrintItemModel temp = templetModels.FirstOrDefault(p =>
                {
                    return p.Index == index;
                });
                if (temp == null)
                {
                    throw new Exception(string.Format("未查询到Index={0}的打印条目", index));
                }

                return reValue = RandomStringHelper.GetRandomString(temp.FunctionData.RandomType, temp.FunctionData.RandomLength);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("执行{0}异常:{1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
