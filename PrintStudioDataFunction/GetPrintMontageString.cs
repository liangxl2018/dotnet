using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintStudioModel;

namespace PrintStudioDataFunction
{
    public class GetPrintMontageString : IDataFunction
    {
        /// <summary>
        /// 剪裁 拼接
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
                string reValue = string.Empty;
                foreach (int item in indexs)
                {
                    string value = string.Empty;
                    PrintItemModel temp = templetModels.Where(p => { return p.Index == item; }).ElementAt(0);
                    if (temp.FunctionData.MontageLength > 0)
                    {
                        if (string.IsNullOrWhiteSpace(temp.PrintKeyValue))
                        {
                            throw new Exception(string.Format("{0}(ID={1})的Value为空,不能被剪裁.", temp.PrintCaption, temp.Index));
                        }
                        if (temp.PrintKeyValue.Length < temp.FunctionData.MontageLength)
                        {
                            throw new Exception(string.Format("{0}(ID={1})的Value长度小于被剪裁长度,不能被剪裁.", temp.PrintCaption, temp.Index));
                        }
                        if (temp.FunctionData.IsHeadMontage)
                        {
                            value = temp.PrintKeyValue.Substring(0, temp.FunctionData.MontageLength);
                        }
                        else
                        {
                            value = temp.PrintKeyValue.Substring(temp.PrintKeyValue.Length - temp.FunctionData.MontageLength);
                        }
                        reValue += value;
                    }
                    else if (temp.FunctionData.MontageLength < 0)
                    {
                        reValue += temp.PrintKeyValue;
                    }
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
