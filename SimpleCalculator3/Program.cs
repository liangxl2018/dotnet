using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace SimpleCalculator3
{
    /// <summary>
    /// [InheritedExport] 可以直接在接口上加InheritedExport，就不用在每个实类加特性了。
    /// </summary>
    public interface ICalculator
    {
        String Calculate(String input);
    }

    public interface IOperation
    {
        int Operate(int left, int right);

        void Dispose();
    }

    public interface IOperationData
    {
        Char Symbol { get; }
    }

    /// <summary>
    /// 指定带有 System.ComponentModel.Composition.ExportAttribute 标记的类型、属性、字段或方法的元数据。
    /// 这里Symbol必须与IOperationData中属性名称一致。且IOperationData中有且仅能有一个Symbol属性。
    /// </summary>
    [Export(typeof(IOperation)), PartCreationPolicy(CreationPolicy.Shared)]
    [ExportMetadata("Symbol", '+')]
    class Add : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
    [PartNotDiscoverable]//不被发现
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    class Subtract : IOperation, IPartImportsSatisfiedNotification
    {

        public int Operate(int left, int right)
        {
            return left - right;
        }


        public void OnImportsSatisfied()
        {
            //构造函数
        }

        public void Dispose()
        {
            //资源释放
        }
    }

    /// <summary>
    /// 同时存在契约名和类型时，使用 [Import("MySimpleCalculator", typeof(ICalculator))]或[Import("MySimpleCalculator")]
    /// 因为默认契约名称为null或String.Empty,[Import(typeof(ICalculator))]等价于[Import("", typeof(ICalculator))]当然无法匹配[Export("MySimpleCalculator",typeof(ICalculator))]
    /// </summary>
    [Export("MySimpleCalculator", typeof(ICalculator))]
    class MySimpleCalculator : ICalculator
    {
        /// <summary>
        /// 当导入MySimpleCalculator时,系统会自动发现  catalog.Catalogs()中的IEnumerable<Lazy<IOperation, IOperationData>>。
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        IEnumerable<Lazy<IOperation, IOperationData>> operations;

        public String Calculate(String input)
        {
            int left;
            int right;
            Char operation;
            int fn = FindFirstNonDigit(input);
            if (fn < 0) return "Could not parse command.";

            try
            {
                left = int.Parse(input.Substring(0, fn));
                right = int.Parse(input.Substring(fn + 1));
            }
            catch
            {
                return "Could not parse command.";
            }

            operation = input[fn];

            foreach (Lazy<IOperation, IOperationData> i in operations)
            {
                if (i.Metadata.Symbol.Equals(operation))
                {
                    return i.Value.Operate(left, right).ToString();
                }
            }
            return "Operation Not Found!";
        }

        private int FindFirstNonDigit(String s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (!(Char.IsDigit(s[i]))) return i;
            }
            return -1;
        }


        //导出私有的属性
        [Export(typeof(string))]
        private string _privateBookName = "私有的属性BookName";
        //导出公用的属性
        [Export(typeof(string))]
        public string _publicBookName = "公有的属性BookName";

        //导出公用方法(无参数)
        [Export(typeof(Func<string>))]
        public string PublicGetBookName()
        {
            return "PublicMathBook";
        }
        //导出私有方法(有参数)
        [Export(typeof(Func<int, string>))]
        public string PrivateGetBookName(int count)
        {
            return string.Format("我看到一个好书,我买了{0}本", count);
        }
    }


    class Program
    {
        private CompositionContainer _container;

        /// <summary>
        /// 如果[Export(typeof(ICalculator))]，那么可直接[Import]
        /// 如果[Export],那么MySimpleCalculator calculator=null;或Object calculator=null;
        /// </summary>
        [Import("MySimpleCalculator", typeof(ICalculator))]
        public ICalculator calculator = null;

        //导入属性(这里不区分public还是private)
        [ImportMany]
        public List<string> InputString { get; set; }

        //导入无参数方法
        [Import]
        public Func<string> methodWithoutPara { get; set; }
        //导入有参数的方法
        [Import]
        public Func<int, string> methodWithPara { get; set; }

        private Program()
        {
            var catalog = new AggregateCatalog();
            //导入MySimpleCalculator计算器
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            //导入扩展Mod类
            catalog.Catalogs.Add(new DirectoryCatalog("Extensions"));
            _container = new CompositionContainer(catalog);
            this._container.ComposeParts(this);

            //DirectoryCatalog.Fresh()
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            String s;
            Console.WriteLine("Enter Command:");
            while (true)
            {
                s = Console.ReadLine();
                Console.WriteLine(p.calculator.Calculate(s));
            }

            //void App_Startup(object sender, StartupEventArgs e)
            //{
            //var catalog = new AggregateCatalog();
            //catalog.Catalogs.Add(newDirectoryCatalog((@"\.")));
            //var container = new CompositionContainer(catalog);
            //container.Composeparts(this);
            //base.MainWindow = MainWindow;
            //this.DownloadAssemblies(catalog);
            //}

            //private void DownloadAssemblies(AggregateCatalog catalog)
            //{
            ////asynchronously downloads assemblies and calls AddAssemblies
            //}

            //private void AddAssemblies(Assembly[] assemblies, AggregateCatalog catalog)
            //{
            //var assemblyCatalogs = new AggregateCatalog();
            //foreach(Assembly assembly in assemblies)
            //assemblyCatalogs.Catalogs.Add(new AssemblyCatalog(assembly));
            //catalog.Catalogs.Add(assemblyCatalogs);
            //}
        }
    }
}
