using System;
using System.Linq;
using System.Data.Entity;
using System.Reflection.Emit;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Common;

namespace DynamicTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = @"Data Source=PARIS\PARIS16;Initial Catalog=Test;Integrated Security=True";


            //string query3 = "IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL ";
            //query3 += "BEGIN ";
            //query3 += "DROP TABLE dbo.Test ";
            //query3 += "END";

            //ExecuteQuery(connectionString, query3);

            //using (DbContext context = new DbContext(connectionString))
            //{
            //    context.Database.CreateIfNotExists();
            //}

            //    string query = "USE Test IF OBJECT_ID('dbo.Users', 'U') IS NULL ";
            //query += "BEGIN ";
            //query += "CREATE TABLE [dbo].[Users](";
            //query += "[Id] INT IDENTITY(1,1) NOT NULL,";
            //query += "[Name] VARCHAR(100) NOT NULL,";
            //query += "[Country] VARCHAR(50) NOT NULL";
            //query += ")";
            //query += " END";

            //ExecuteQuery(connectionString, query);

            //            string query2 = @"USE Test
            //INSERT INTO Users VALUES
            //('Pesho', 'BG'),
            //('Gosho', 'BG'),
            //('Trendafil', 'BG'),
            //('Ivan', 'Bulgaria'),
            //('Peshi', 'Bul')";

            //ExecuteQuery(connectionString, query2);


            //// Read User input
            //ICollection<string> input = new List<string>();
            //string name = Console.ReadLine();
            //while (true)
            //{
            //    string line = Console.ReadLine();
            //    if (line.Trim().ToLower() == "end")
            //    {
            //        break;
            //    }

            //    input.Add(line);
            //}

            //// Build Query
            //string queryCreate = $"USE Test IF OBJECT_ID('dbo.{name}', 'U') IS NULL ";
            //queryCreate += "BEGIN ";
            //queryCreate += $"CREATE TABLE [dbo].[{name}](";
            //queryCreate += "[Id] INT IDENTITY(1,1) NOT NULL,";

            //foreach (var item in input)
            //{
            //    var prop = item.Split(new char[] { ':', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //    string variableType = string.Empty;
            //    switch (prop[1])
            //    {
            //        case "string":
            //            variableType = "NVARCHAR(100)";
            //            break;

            //        case "int":
            //            variableType = "INT";
            //            break;

            //        default:
            //            break;
            //    }
            //    queryCreate += $"[{prop[0]}] {variableType},";
            //}

            //queryCreate += ")";
            //queryCreate += " END";

            //ExecuteQuery(connectionString, queryCreate);


            // Dynamic assembly
            // Create assembly and module
            var assemblyName = new AssemblyName("DynamicAssemblyExample");
            var assemblyBuilder = AppDomain.CurrentDomain.
                DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.
                DefineDynamicModule(assemblyName.Name, assemblyName.Name + ".dll");

            // Open a new Type for definition
            var typeBuilder = moduleBuilder.DefineType("MyDynamicType",
                TypeAttributes.Public |
                TypeAttributes.Sealed |
                TypeAttributes.ExplicitLayout |
                TypeAttributes.Serializable |
                TypeAttributes.AnsiClass,
                typeof(ValueType));

            // Find MarshalAsAttribute's constructor by signature, then invoke
            var ctorParameters = new Type[] { typeof(UnmanagedType) };
            var ctorInfo = typeof(MarshalAsAttribute).GetConstructor(ctorParameters);

            var fields = typeof(MarshalAsAttribute).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var sizeConst = (from f in fields
                             where f.Name == "SizeConst"
                             select f).ToArray();
            var marshalAsAttr = new CustomAttributeBuilder(ctorInfo,
                new object[] { UnmanagedType.ByValTStr }, sizeConst, new object[] { 5 });

            var fbString = typeBuilder.DefineField(
                "m_string",
                typeof(string),
                FieldAttributes.Public);
            fbString.SetOffset(0);
            //fbString.SetCustomAttribute(marshalAsAttr);

            var fbNumber = typeBuilder.DefineField(
                "m_number",
                typeof(int),
                FieldAttributes.Public);
            fbNumber.SetOffset(8);

            var type = typeBuilder.CreateType();
            bool isVal = type.IsValueType; // true

            assemblyBuilder.Save(assemblyName.Name + ".dll");
            Assembly assembly = Assembly.LoadFrom("./" + assemblyName.Name + ".dll");
            AppDomain.CurrentDomain.Load(assembly.GetName());


            var typeBuiler = CreateTypeBuilder(assemblyName.FullName, moduleBuilder.FullyQualifiedName, "MyDynamicType");
            Program.CreateAutoImplementedProperty(typeBuiler, "m_string", typeof(string));
            Program.CreateAutoImplementedProperty(typeBuiler, "m_number", typeof(int));

            Type resultType = typeBuiler.CreateType();
            dynamic obj = Activator.CreateInstance(resultType);

            PropertyInfo property = resultType.GetProperty("m_string");

            object value = property.GetValue(obj, null);
            if (value != null)
            {
                Console.WriteLine(value.ToString());
            }

            property.SetValue(obj, "Hello world!", null);


            object value2 = property.GetValue(obj, null);
            Console.WriteLine(value2.ToString());


            //PrintUsers(connectionString);

            //var list = GetColumnNames(connectionString, "Users");
            var table = getReadingTableFromSchema(connectionString, "peshos");
            foreach (var item in table.Columns)
            {
                Console.WriteLine($"{((DataColumn)item).ColumnName}: {((DataColumn)item).DataType.Name}");
            }
        }

        public static void PrintUsers(string connectionString)
        {
            using (DbContext context = new DbContext(connectionString))
            {
                TypeBuilder builder = Program.CreateTypeBuilder(
                    "MyDynamicAssembly", "MyModule", "MyType");
                Program.CreateAutoImplementedProperty(builder, "Name", typeof(string));
                Program.CreateAutoImplementedProperty(builder, "Country", typeof(string));
                Program.CreateAutoImplementedProperty(builder, "Id", typeof(int));

                Type resultType = builder.CreateType();

                dynamic queryResult = context.Database.SqlQuery(
                    resultType, "SELECT * FROM dbo.Users");
                int count = 0;


                Console.WriteLine("{0,20} {1,20} {2,10}", "Name", "Country", "Id");
                foreach (dynamic item in queryResult)
                {
                    count++;
                    Console.WriteLine("{0,10} {1,4} {2,10}", item.Name, item.Country, item.Id);
                }
                Console.WriteLine($"Users count: {count}");
            }
        }

        public static void ExecuteQuery(string connectionString, string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static IEnumerable<string> GetColumnNames(string connectionString,string tableName)
        {
            IEnumerable<string> columnList;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                    con.Open();

                    string[] restrictions = new string[4] { null, null, tableName, null };
                    columnList = con.GetSchema("Columns", restrictions).AsEnumerable().Select(s => s.Field<String>("Column_Name")).ToList();
                    con.Close();
            }

            return columnList;
        }

        private static DataTable getReadingTableFromSchema(string connectionString, string tableName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM [{tableName}]";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                DbDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtbl = new DataTable();
                da.FillSchema(dtbl, SchemaType.Source);
                return dtbl;
            }
        }

        public static TypeBuilder CreateTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        public static void CreateAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName), propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
    }
}