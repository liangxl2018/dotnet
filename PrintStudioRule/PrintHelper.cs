using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using PrintStudioRule;
using PrintStudioModel;
using System.IO;

namespace PrintStudioRule
{
    public class PrintHelper
    {
        private static CommonPrintHelper commonPrintHelper = null;

        /// <summary>
        /// 斑马打印
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="printFuntionNameSpace"></param>
        /// <param name="dataFuntionNameSpace"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        /// <param name="XDeviation"></param>
        /// <param name="YDeviation"></param>
        public static void ZebraStartPrintForPES(Dictionary<string, string> parameters, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInforForPES(parameters, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            ZebraStartPrint(commonPrintModel.PrintItems, printName, printCount);
        }

        /// <summary>
        /// 斑马打印
        /// </summary>
        /// <param name="element"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="printFuntionNameSpace"></param>
        /// <param name="dataFuntionNameSpace"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        /// <param name="XDeviation"></param>
        /// <param name="YDeviation"></param>
        public static void ZebraStartPrint(FrameworkElement element, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInfo(element, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            ZebraStartPrint(commonPrintModel.PrintItems, printName, printCount);
        }


        /// <summary>
        /// 斑马打印
        /// </summary>
        /// <param name="PrintItems"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        public static void ZebraStartPrint(List<PrintItemModel> PrintItems, string printName, int printCount)
        {
            if (PrintItems == null || PrintItems.Count < 1)
            {
                throw new Exception("打印条目集合不能为空.");
            }
            if (string.IsNullOrWhiteSpace(printName))
            {
                throw new Exception("打印机名不能为空.");
            }
            if (printCount < 1)
            {
                throw new Exception("打印数量必须大于0.");
            }
            ZebraPrinterHelper.Open(printName);
            ZebraPrinterHelper.DeleteImage("*.GRF");
            PrintItems.ForEach(item =>
            {
                if (item.IsValid)
                {
                    if (item.ParseFuntion == null)
                    {
                        throw new Exception(string.Format("打印条目<{0}>的ParseFuntion方法不能为空"));
                    }
                    item.ParseFuntion.PrintParseFuntion(item);
                }
            });
            ZebraPrinterHelper.PTK_PrintLabel(printCount);
        }

        /// <summary>
        /// 通用打印For PES
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="printFuntionNameSpace"></param>
        /// <param name="dataFuntionNameSpace"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        /// <param name="XDeviation"></param>
        /// <param name="YDeviation"></param>
        public static void CommonStartPrintForPES(Dictionary<string, string> parameters, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInforForPES(parameters, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            if (commonPrintHelper == null)
            {
                commonPrintHelper = new CommonPrintHelper();
            }
            commonPrintHelper.StartPrint(commonPrintModel, printName, printCount);
        }

        public static void CommonStartPrint(FrameworkElement element, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInfo(element, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            if (commonPrintHelper == null)
            {
                commonPrintHelper = new CommonPrintHelper();
            }
            commonPrintHelper.StartPrint(commonPrintModel, printName, printCount);
        }

        /// <summary>
        /// PES打印方法
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="commonPrintModel">打印模型</param>
        /// <param name="printFuntionNameSpace">打印方法命名空间</param>
        /// <param name="dataFuntionNameSpace">取值方法命名空间</param>
        /// <param name="printName">打印机名</param>
        /// <param name="printCount">打印份数</param>
        /// <param name="XDeviation">X偏移</param>
        /// <param name="YDeviation">Y偏移</param>
        public static void StartPrintForPES(Dictionary<string, string> parameters, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInforForPES(parameters, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            StartPrint(commonPrintModel.PrintItems, printName, printCount);
        }

        /// <summary>
        /// 开始打印
        /// </summary>
        /// <param name="element"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="parseFuntionNameSpace"></param>
        public static void StartPrint(FrameworkElement element, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, string printName, int printCount, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInfo(element, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            StartPrint(commonPrintModel.PrintItems, printName, printCount);
        }

        /// <summary>
        /// 开始打印
        /// </summary>
        /// <param name="element"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="parseFuntionNameSpace"></param>
        public static void StartPrint(FrameworkElement element, PrintFactoryModel commonPrintModel, string printFuntionNameSpace, string dataFuntionNameSpace, int XDeviation, int YDeviation)
        {
            DeleteFilesByPath("TempImageConvertDirectory");
            ParsePrintTempletInfo(element, commonPrintModel, printFuntionNameSpace, dataFuntionNameSpace, XDeviation, YDeviation);
            StartPrint(commonPrintModel.PrintItems);
        }

        /// <summary>
        /// 部分打印
        /// </summary>
        /// <param name="PrintItems"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        public static void StartPrint(List<PrintItemModel> PrintItems)
        {
            if (PrintItems == null || PrintItems.Count < 1)
            {
                throw new Exception("打印条目集合不能为空.");
            }
            foreach (PrintItemModel item in PrintItems)
            {
                if (item.IsValid)
                {
                    if (item.ParseFuntion == null)
                    {
                        throw new Exception(string.Format("打印条目<{0}>的ParseFuntion方法不能为空"));
                    }
                    item.ParseFuntion.PrintParseFuntion(item);
                }
            }
        }

        /// <summary>
        /// 完整打印
        /// </summary>
        /// <param name="PrintItems"></param>
        /// <param name="printName"></param>
        /// <param name="printCount"></param>
        public static void StartPrint(List<PrintItemModel> PrintItems, string printName, int printCount)
        {
            if (PrintItems == null || PrintItems.Count < 1)
            {
                throw new Exception("打印条目集合不能为空.");
            }
            if (string.IsNullOrWhiteSpace(printName))
            {
                throw new Exception("打印机名不能为空.");
            }
            if (printCount < 1)
            {
                throw new Exception("打印数量必须大于0.");
            }
            PrintRuleBase.ClosePort();
            PrintRuleBase.OpenPort(printName);
            PrintRuleBase.PTK_ClearBuffer();
            PrintRuleBase.PTK_BinGraphicsDel("*");
            PrintRuleBase.PTK_SetDirection('T');
            foreach (PrintItemModel item in PrintItems)
            {
                if (item.IsValid)
                {
                    if (item.ParseFuntion == null)
                    {
                        throw new Exception(string.Format("打印条目<{0}>的ParseFuntion方法不能为空"));
                    }
                    item.ParseFuntion.PrintParseFuntion(item);
                }
            }
            PrintRuleBase.PTK_PrintLabel(1, printCount);
            PrintRuleBase.ClosePort();
        }

        /// <summary>
        /// PES解析方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="commonPrintModel"></param>
        /// <param name="parseFuntionNameSpace"></param>
        /// <param name="dataFuntionNameSpace"></param>
        /// <param name="XDeviation"></param>
        /// <param name="YDeviation"></param>
        public static void ParsePrintTempletInforForPES(Dictionary<string, string> parameters, PrintFactoryModel commonPrintModel, string parseFuntionNameSpace, string dataFuntionNameSpace, int XDeviation, int YDeviation)
        {
            if (commonPrintModel.PrintItems != null)
            {
                foreach (PrintItemModel item in commonPrintModel.PrintItems)
                {
                    item.XDeviation = XDeviation;
                    item.YDeviation = YDeviation;
                    if (item.DataSourceType == 1)
                    {
                        if (item.ConttrolData == null || string.IsNullOrWhiteSpace(item.ConttrolData.ControlName))
                        {
                            throw new Exception(string.Format("请正确设置打印条目<{0}>的ConttrolDataDataSource属性值.", item.PrintCaption));
                        }
                        if (!parameters.ContainsKey(item.ConttrolData.ControlName))
                        {
                            throw new Exception(string.Format("未查询到{0}【{1}】键值对.", item.ConttrolData.ControlCaption, item.ConttrolData.ControlName));
                        }
                        string value = parameters[item.ConttrolData.ControlName];
                        item.ConttrolData.ValueContainer = string.Format(item.ConttrolData.ValueContainer, value);
                        item.PrintKeyValue = string.Format(item.PrintKeyValue, item.ConttrolData.ValueContainer);
                    }
                    else if (item.DataSourceType == 2)
                    {
                        if (item.FunctionData == null || string.IsNullOrWhiteSpace(item.FunctionData.FunctionName))
                        {
                            throw new Exception(string.Format("请正确设置打印条目<{0}>的FunctionDataSource属性值.", item.PrintCaption));
                        }
                        item.FunctionData.ParseFuntion = CommonRule.LoadClassInstance<IDataFunction>(dataFuntionNameSpace, item.FunctionData.FunctionName);
                        item.FunctionData.ValueContainer = string.Format(item.FunctionData.ValueContainer, item.FunctionData.ParseFuntion.DataParseFuntion(commonPrintModel.PrintItems, item.FunctionData.FunctionIndexs));
                        item.PrintKeyValue = string.Format(item.PrintKeyValue, item.FunctionData.ValueContainer);
                    }
                    if (string.IsNullOrWhiteSpace(item.PrintFunctionName))
                    {
                        throw new Exception(string.Format("请设置打印条目<{0}>的PrintFunctionName属性值.", item.PrintCaption));
                    }
                    item.ParseFuntion = CommonRule.LoadClassInstance<IPrintFunction>(parseFuntionNameSpace, item.PrintFunctionName);
                    item.DefaultParameters = DefaultPropertyValueTable.PrintFunctionDefaultParameters.FirstOrDefault(p =>
                    {
                        return p.Key == item.PrintFunctionName;
                    }).Value;
                    if (string.IsNullOrWhiteSpace(item.PrintKeyValue))
                    {
                        item.PrintKeyValue = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 解析打印模板 
        /// </summary>
        /// <param name="commonPrintModel"></param>
        /// <param name="parseFuntionNameSpace"></param>
        public static void ParsePrintTempletInfo(FrameworkElement element, PrintFactoryModel commonPrintModel, string parseFuntionNameSpace, string dataFuntionNameSpace, int XDeviation, int YDeviation)
        {
            if (commonPrintModel.PrintItems != null)
            {
                foreach (PrintItemModel item in commonPrintModel.PrintItems)
                {
                    item.XDeviation = XDeviation;
                    item.YDeviation = YDeviation;
                    if (item.DataSourceType == 1)
                    {
                        if (item.ConttrolData == null)
                        {
                            throw new Exception(string.Format("请正确设置打印条目<{0}>的ConttrolData值.", item.PrintCaption));
                        }
                        DependencyObject dependencyObject = DependencyHelper.FindVisualChildByName(element, item.ConttrolData.ControlName);
                        if (dependencyObject == null)
                        {
                            throw new Exception(string.Format("未从界面获取到<{0},{1}>控件.", item.ConttrolData.ControlCaption, item.ConttrolData.ControlName));
                        }
                        if (string.IsNullOrWhiteSpace(item.ConttrolData.ControlName))
                        {
                            throw new Exception(string.Format("请正确设置打印条目<{0}>的ConttrolData属性的PropertyName属性值.", item.PrintCaption));
                        }
                        object value = CommonRule.GetPropertyValue(dependencyObject, item.ConttrolData.PropertyName);
                        if (!string.IsNullOrWhiteSpace(item.ConttrolData.ChildPropertyName))
                        {
                            if (value == null)
                            {
                                throw new Exception(string.Format("未从界面获取到<{0}>的{1}值.", item.ConttrolData.ControlCaption, item.ConttrolData.PropertyName));
                            }
                            value = CommonRule.GetPropertyValue(value, item.ConttrolData.ChildPropertyName);
                        }
                        item.ConttrolData.ValueContainer = string.Format(item.ConttrolData.ValueContainer, value);
                        item.PrintKeyValue = string.Format(item.PrintKeyValue, item.ConttrolData.ValueContainer);
                    }
                    else if (item.DataSourceType == 2)
                    {
                        if (item.FunctionData == null || string.IsNullOrWhiteSpace(item.FunctionData.FunctionName))
                        {
                            throw new Exception(string.Format("请正确设置打印条目<{0}>的ConttrolDataSource属性值.", item.PrintCaption));
                        }
                        item.FunctionData.ParseFuntion = CommonRule.LoadClassInstance<IDataFunction>(dataFuntionNameSpace, item.FunctionData.FunctionName);
                        item.FunctionData.ValueContainer = string.Format(item.FunctionData.ValueContainer, item.FunctionData.ParseFuntion.DataParseFuntion(commonPrintModel.PrintItems, item.FunctionData.FunctionIndexs));
                        item.PrintKeyValue = string.Format(item.PrintKeyValue, item.FunctionData.ValueContainer);
                    }
                    if (string.IsNullOrWhiteSpace(item.PrintFunctionName))
                    {
                        throw new Exception(string.Format("请设置打印条目<{0}>的PrintFunctionName属性值.", item.PrintCaption));
                    }
                    item.ParseFuntion = CommonRule.LoadClassInstance<IPrintFunction>(parseFuntionNameSpace, item.PrintFunctionName);
                    item.DefaultParameters = DefaultPropertyValueTable.PrintFunctionDefaultParameters.FirstOrDefault(p =>
                    {
                        return p.Key == item.PrintFunctionName;
                    }).Value;
                    if (string.IsNullOrWhiteSpace(item.PrintKeyValue))
                    {
                        item.PrintKeyValue = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 删除路径下所有文件
        /// </summary>
        /// <param name="path"></param>
        private static void DeleteFilesByPath(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
