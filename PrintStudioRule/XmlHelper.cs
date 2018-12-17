using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PrintStudioModel;

namespace PrintStudioRule
{
    public class XmlHelper
    {
        /// <summary>
        /// 保存到配置文件 如果不希望出现写死的节点,如xmlDoc.CreateElement("Print").
        /// 可创建一个基类,仅保存需要保存到配置文件的属性.可继承该基类,增加不需要保存到配置文件的属性.
        /// 保存时,仅保存基类的属性即可.当然遇到复杂类型属性,可能还是需要特殊处理哦.
        /// 可根据pi.PropertyType判断是否为基本类型,如果是基本类型就可以进行保存,如果是复杂类型就需要根据不同的Type
        /// 进行特殊处理
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string SavePrintTemplet(PrintFactoryModel p, string path)
        {
            string reValue = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlAttribute attribute = null;
            XmlNode configurationNode = xmlDoc.CreateElement("Print");
            attribute = xmlDoc.CreateAttribute("Width");
            attribute.Value = p.Width.ToString();
            configurationNode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("Height");
            attribute.Value = p.Height.ToString();
            configurationNode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("PicturResource");
            attribute.Value = p.PicturResource.ToString();
            configurationNode.Attributes.Append(attribute);
            foreach (PrintItemModel item in p.PrintItems)
            {
                XmlNode printItem = xmlDoc.CreateElement("PrintItem");
                attribute = xmlDoc.CreateAttribute("PrintCaption");
                attribute.Value = item.PrintCaption;
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("PrintFunctionName");
                attribute.Value = item.PrintFunctionName;
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("PrintKeyValue");
                attribute.Value = item.PrintKeyValue;
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("DataSourceType");
                attribute.Value = item.DataSourceType.ToString();
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("Index");
                attribute.Value = item.Index.ToString();
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("IsValid");
                attribute.Value = item.IsValid.ToString();
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("Width");
                attribute.Value = item.Width.ToString();
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("Height");
                attribute.Value = item.Height.ToString();
                printItem.Attributes.Append(attribute);
                XmlNode conttrolDataSource = xmlDoc.CreateElement("ConttrolDataSource");
                if (item.ConttrolData == null)
                {
                    item.ConttrolData = new ConttrolDataItemModel();
                }
                attribute = xmlDoc.CreateAttribute("ValueContainer");
                attribute.Value = item.ConttrolData.ValueContainer;
                conttrolDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ControlName");
                attribute.Value = item.ConttrolData.ControlName;
                conttrolDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ControlCaption");
                attribute.Value = item.ConttrolData.ControlCaption;
                conttrolDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("PropertyName");
                attribute.Value = item.ConttrolData.PropertyName;
                conttrolDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ChildPropertyName");
                attribute.Value = item.ConttrolData.ChildPropertyName;
                conttrolDataSource.Attributes.Append(attribute);
                XmlNode functionDataSource = xmlDoc.CreateElement("FunctionDataSource");
                if (item.FunctionData == null)
                {
                    item.FunctionData = new FunctionDataItemModel();
                }
                attribute = xmlDoc.CreateAttribute("ValueContainer");
                attribute.Value = item.FunctionData.ValueContainer;
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("FunctionName");
                attribute.Value = item.FunctionData.FunctionName;
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("FunctionIndexs");
                attribute.Value = item.FunctionData.FunctionIndexsCaption;
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("RandomLength");
                attribute.Value = item.FunctionData.RandomLength.ToString();
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("RandomType");
                attribute.Value = item.FunctionData.RandomType.ToString();
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("IsHeadMontage");
                attribute.Value = item.FunctionData.IsHeadMontage.ToString();
                functionDataSource.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("MontageLength");
                attribute.Value = item.FunctionData.MontageLength.ToString();
                functionDataSource.Attributes.Append(attribute);
                XmlNode parameters = xmlDoc.CreateElement("Parameters");
                if (item.Parameters != null)
                {
                    foreach (KeyValuePair<string, string> ky in item.Parameters)
                    {
                        XmlNode Parameter = xmlDoc.CreateElement("Parameter");
                        attribute = xmlDoc.CreateAttribute("Key");
                        attribute.Value = ky.Key;
                        Parameter.Attributes.Append(attribute);
                        attribute = xmlDoc.CreateAttribute("Value");
                        attribute.Value = ky.Value;
                        Parameter.Attributes.Append(attribute);
                        parameters.AppendChild(Parameter);
                    }
                }
                printItem.AppendChild(conttrolDataSource);
                printItem.AppendChild(functionDataSource);
                printItem.AppendChild(parameters);
                configurationNode.AppendChild(printItem);
            }
            xmlDoc.AppendChild(configurationNode);
            xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);
            xmlDoc.Save(path);
            return reValue;
        }

        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PrintFactoryModel GetPrintTemplet(string path, out string errorInfo)
        {
            errorInfo = string.Empty;
            PrintItemModel printItem = null;
            ConttrolDataItemModel controlItem = null;
            FunctionDataItemModel functionItem = null;
            PrintFactoryModel reValue = new PrintFactoryModel();
            List<PrintItemModel> printItems = new List<PrintItemModel>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                //基本信息获取
                XmlNodeList baseData = xmlDoc.SelectNodes("/Print");
                if (baseData.Count < 1)
                {
                    throw new Exception("无效的打印配置文件.");
                }
                foreach (XmlAttribute item in baseData[0].Attributes)
                {
                    CommonRule.SetPropertyValue(reValue, item.Name, item.Value);
                }
                //数据提取
                foreach (XmlNode item in xmlDoc.SelectNodes("/Print/PrintItem"))
                {
                    printItem = new PrintItemModel() { };
                    //打印属性
                    foreach (XmlAttribute attribute in item.Attributes)
                    {
                        CommonRule.SetPropertyValue(printItem, attribute.Name, attribute.Value);
                    }
                    //Control
                    XmlNodeList temp = item.SelectNodes("ConttrolDataSource");
                    if (temp != null && temp.Count > 0)
                    {
                        controlItem = new ConttrolDataItemModel();
                        foreach (XmlAttribute controlNode in temp[0].Attributes)
                        {
                            CommonRule.SetPropertyValue(controlItem, controlNode.Name, controlNode.Value);
                        }
                        printItem.ConttrolData = controlItem;
                    }
                    //Function
                    temp = item.SelectNodes("FunctionDataSource");
                    if (temp != null && temp.Count > 0)
                    {
                        functionItem = new FunctionDataItemModel();
                        foreach (XmlAttribute functionNode in temp[0].Attributes)
                        {
                            if (functionNode.Name.Equals("FunctionIndexs"))
                            {
                                if (!string.IsNullOrWhiteSpace(functionNode.Value))
                                {
                                    functionItem.FunctionIndexs = CommonRule.ParseIndexs(functionNode.Value);
                                    functionItem.FunctionIndexsCaption = functionNode.Value;
                                }
                            }
                            else
                            {
                                CommonRule.SetPropertyValue(functionItem, functionNode.Name, functionNode.Value);
                            }
                        }
                        printItem.FunctionData = functionItem;
                    }
                    //Parameters
                    temp = item.SelectNodes("Parameters");
                    if (temp != null && temp.Count > 0)
                    {
                        //参数列表提取
                        if (temp[0].HasChildNodes)
                        {
                            ParameterModel p = null;
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            foreach (XmlNode child in temp[0].ChildNodes)
                            {
                                p = new ParameterModel();
                                foreach (XmlAttribute childattribute in child.Attributes)
                                {
                                    CommonRule.SetPropertyValue(p, childattribute.Name, childattribute.Value);
                                }
                                parameters.Add(p.Key, p.Value);
                            }
                            printItem.Parameters = parameters;
                        }
                    }
                    printItems.Add(printItem);
                }
                reValue.PrintItems = printItems;
            }
            catch (Exception ex)
            {
                errorInfo = ex.Message;
                reValue = null;
            }
            return reValue;
        }

        public static PrintClientConfig GetClientConfig(string path)
        {
            PrintClientConfig printClientConfig = new PrintClientConfig();
            PrintItemControlModel temp = new PrintItemControlModel();
            List<PrintItemControlModel> reValue = new List<PrintItemControlModel>();
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                //数据提取
                xmlDoc.Load(path);
                //基本信息获取
                foreach (XmlAttribute item in xmlDoc.SelectNodes("/PrintItemControls")[0].Attributes)
                {
                    CommonRule.SetPropertyValue(printClientConfig, item.Name, item.Value);
                }
                foreach (XmlNode item in xmlDoc.SelectNodes("/PrintItemControls/PrintItemControl"))
                {
                    temp = new PrintItemControlModel();
                    //打印属性
                    foreach (XmlAttribute attribute in item.Attributes)
                    {
                        CommonRule.SetPropertyValue(temp, attribute.Name, attribute.Value);
                    }
                    reValue.Add(temp);
                }
                printClientConfig.PrintItemControls = reValue;
            }
            catch
            {
                printClientConfig = null;
            }
            return printClientConfig;
        }

        public static List<ParameterModel> GetHelpDocument(string fileName)
        {
            List<ParameterModel> reValue = new List<ParameterModel>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            foreach (XmlNode item in xmlDoc.SelectNodes("/Items/Item"))
            {
                ParameterModel temp = new ParameterModel();
                foreach (XmlAttribute attribute in item.Attributes)
                {
                    CommonRule.SetPropertyValue(temp, attribute.Name, attribute.Value);
                }
                reValue.Add(temp);
            }
            return reValue;
        }

        public static string SaveClientConfig(PrintClientConfig printClientConfig, string path)
        {
            string reValue = string.Empty;
            List<PrintItemControlModel> printItemControls = printClientConfig.PrintItemControls;
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlAttribute attribute = null;
            XmlNode configurationNode = xmlDoc.CreateElement("PrintItemControls");
            attribute = xmlDoc.CreateAttribute("X");
            attribute.Value = printClientConfig.X.ToString();
            configurationNode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("Y");
            attribute.Value = printClientConfig.Y.ToString();
            configurationNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("PrintTypeIndex");
            attribute.Value = printClientConfig.PrintTypeIndex.ToString();
            configurationNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("PrintNameIndex");
            attribute.Value = printClientConfig.PrintNameIndex.ToString();
            configurationNode.Attributes.Append(attribute);
            foreach (PrintItemControlModel item in printItemControls)
            {
                XmlNode printItem = xmlDoc.CreateElement("PrintItemControl");
                attribute = xmlDoc.CreateAttribute("Caption");
                attribute.Value = item.Caption;
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("Name");
                attribute.Value = item.Name;
                printItem.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("Value");
                attribute.Value = item.Value;
                printItem.Attributes.Append(attribute);
                configurationNode.AppendChild(printItem);
            }
            xmlDoc.AppendChild(configurationNode);
            xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);
            xmlDoc.Save(path);
            return reValue;
        }
    }
}
