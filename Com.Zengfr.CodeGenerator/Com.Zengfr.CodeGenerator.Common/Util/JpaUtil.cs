using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.CodeGenerator.Common.Algorithm;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
    public class JpaUtil
    {
        private static IList<string> keywords1 =  new List<string> { "And", "Or" };
        private static IList<string> keywords2 =  new List<string> { "", "Not", "Between", "In", "GreaterThanEqual",
                "GreaterThan", "LessThan" };
        private static JpaUtil instance;
        public static JpaUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JpaUtil();
                }
                return instance;
            }
        }
        public Boolean isOriginalType(Type cls)
        {
            String fieldTypetName = cls.FullName;
            return fieldTypetName.StartsWith("java.") || fieldTypetName.StartsWith("java.lang")
                    || fieldTypetName.StartsWith("java.util") || fieldTypetName.StartsWith("java.sql")
                    || fieldTypetName.StartsWith("System.");
        }

        public ICollection<FieldInfo> getAllFields(Type cls, Boolean filter)
        {
            return cls.GetFields(BindingFlags.Default| BindingFlags.NonPublic| BindingFlags.Public).Where(t => (!t.IsStatic) && (!filter || !fieldIsIgnore(t))).ToList() ;      
        }

        public ICollection<FieldInfo> getAllEntityFields(Type cls, Boolean filter)
        {
            ICollection<FieldInfo> fields = getAllFields(cls, filter);
            return fields.Where(t => (t.FieldType.Name.IndexOf("System") == -1)).ToList();
        }

        public IDictionary<String, String> getFieldsAllByAll(Type cls, Boolean filter)
        {
            return getFieldsAllByAll(cls, filter, "");
        }

        private IDictionary<String, String> getFieldsAllByAll(Type cls, Boolean filter, String parentPath)
        {
            IDictionary<String, String> fMap = new Dictionary<String, String>();
            ICollection<FieldInfo> fSet = getAllFields(cls, filter);
            String fPath;
            foreach (FieldInfo f in fSet)
            {
                fPath = String.IsNullOrEmpty(parentPath) ? f.Name : parentPath + "." + f.Name;
                if (isOriginalType(f.FieldType))
                {
                    fMap.Add(fPath, f.FieldType.Name);
                }
                else
                {
                    IDictionary<String, String> fsMap = getFieldsAllByAll(f.FieldType, filter, fPath);
                    foreach (KeyValuePair<String, String> fs in fsMap)
                    {
                        fMap.Add(fs.Key, fs.Value);
                    }
                }
            }
            return fMap;
        }

        private Boolean fieldIsIgnore(FieldInfo f)
        {
            Boolean isIgnore = false;
            Type t = f.FieldType;
            String tName = t.Name.ToLower();
            String fName = StringUtil.Instance.ToPascal(f.Name);
            if (tName.LastIndexOf(".") >= 0)
                tName = tName.Substring(tName.LastIndexOf(".") + 1);
            switch (tName)
            {
                case "date":
                    break;
                case "datetime":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "timestamp":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "time":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "boolean":
                    break;
                case "byte":
                    break;
                case "short":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "bigdecimal":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "int":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "biginteger":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "integer":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "long":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "string":
                    isIgnore = fieldNameIsIgnore(fName);
                    break;
                case "double":
                    isIgnore = true;
                    break;
                case "float":
                    isIgnore = true;
                    break;
                default:
                    break;
            }
            return isIgnore;
        }

        private static Boolean keywordsAndFieldTypeIsIgnore(String k1, String fieldTypeName)
        {
            Boolean isIgnore = false;
            String tName = fieldTypeName.ToLower();
            if (tName.LastIndexOf(".") >= 0)
                tName = tName.Substring(tName.LastIndexOf(".") + 1);
            switch (tName)
            {
                case "date":
                    isIgnore = isIgnore || "Not,".Contains(k1);
                    break;
                case "datetime":
                    isIgnore = isIgnore || "Not,In".Contains(k1);
                    break;
                case "timestamp":
                    isIgnore = isIgnore || "Not,In".Contains(k1);
                    break;
                case "time":
                    isIgnore = isIgnore || "Not,In".Contains(k1);
                    break;

                case "byte":
                    break;
                case "short":

                    break;
                case "bigdecimal":
                    isIgnore = isIgnore || "Not,In".Contains(k1);
                    break;
                case "int":
                    break;
                case "biginteger":
                    isIgnore = isIgnore || "Not,".Contains(k1);
                    break;
                case "integer":
                    break;
                case "long":
                    isIgnore = isIgnore || "Not,".Contains(k1);
                    break;
                case "string":
                    isIgnore = isIgnore || "Between,GreaterThanEqual,GreaterThan,LessThan".Contains(k1);
                    break;
                case "double":
                    isIgnore = true;
                    break;
                case "float":
                    isIgnore = true;
                    break;
                case "boolean":
                    isIgnore = k1 == null || k1.Length > 0;
                    break;
                default:
                    isIgnore = true;
                    break;
            }
            return isIgnore;
        }

        private Boolean fieldNameIsIgnore(String name)
        {
            return !(name.EndsWith("Id") || name.EndsWith("Name") || name.EndsWith("Status") || name.EndsWith("Stats")
                    || name.EndsWith("Key") || name.EndsWith("Count") || name.EndsWith("Size") || name.EndsWith("Len")
                    || name.EndsWith("LastTime"));
        }
        public String buildArgsWithoutType(String args)
        {
            var rtn = new StringBuilder();
            var argsArray = args.Split(',');
            foreach (var argsArrayItem in argsArray) {
                var typeAndArg = argsArrayItem.Split(' ');
                rtn.Append(","+typeAndArg[1]);
            }
            return rtn.ToString().Trim(',');
        }
        public IDictionary<String, String> buildOne(ICollection<FieldInfo> fieldInfos)
        {
            IDictionary<String, String> fieldNameAndTypeNamesMap = fieldInfos.Where(t => !t.IsStatic).ToDictionary(t => t.Name, t => t.FieldType.FullName);
            return buildOne(fieldNameAndTypeNamesMap);
        }
        public IDictionary<String, String> buildOne(IDictionary<String, String> fieldNameAndTypeNamesMap)
        {
            IDictionary<String, String> results = new Dictionary<String, String>();
            
            List<String> fieldNamesList = fieldNameAndTypeNamesMap.Keys.ToList();
            Combinations<String> fieldNamesCombinations = new Combinations<String>(fieldNamesList, 1);
            Combinations<String> k2Combinations = new Combinations<String>(keywords2, 1);
            foreach (IList<String> fieldNamesCombinationItem in fieldNamesCombinations)
            {
                foreach (IList<String> k2CombinationItem in k2Combinations)
                {
                    String key = fieldNamesCombinationItem[0];
                    if (!keywordsAndFieldTypeIsIgnore(k2CombinationItem[0], fieldNameAndTypeNamesMap[key]))
                    {
                        results.Add(String.Format("{0},{1}", fieldNamesCombinationItem[0], k2CombinationItem[0]),
                                buildArgs(k2CombinationItem[0], key, fieldNameAndTypeNamesMap[key]));
                    }
                }
            }
            return results;
        }
        public IDictionary<String, String> buildTwo(IDictionary<String, String> fieldNameAndTypeNamesMap)
        {
            IDictionary<String, String> resultsOne = buildOne(fieldNameAndTypeNamesMap);
            return buildTwoByResultsOne(resultsOne);
        }
        public IDictionary<String, String> buildTwo(ICollection<FieldInfo> fields)
        {
            IDictionary<String, String> resultsOne = buildOne(fields);
            return buildTwoByResultsOne(resultsOne);
        }
        private IDictionary<String, String> buildTwoByResultsOne(IDictionary<String, String> resultsOne)
        {
            IDictionary<String, String> resultsTwo = new Dictionary<String, String>();
           
            Permutations<String> fPermutations = new Algorithm.Permutations<String>(resultsOne.Keys, 2);
            Combinations<String> k1Combinations = new Algorithm.Combinations<String>(keywords1, 1);
            foreach (IList<String> fPermutationItem in fPermutations)
            {
                if (!keywordsIsIgnore(fPermutationItem[0], fPermutationItem[1]))
                {
                    foreach (IList<String> k1CombinationItem in k1Combinations)
                    {
                        String arg1 = resultsOne[fPermutationItem[0]];
                        String arg2 = resultsOne[fPermutationItem[1]];
                        String arg;
                        if (!arg1.Equals(arg2))
                        {
                            arg = String.Format("{0},{1}", arg1, arg2);
                        }
                        else
                        {
                            arg = String.Format("{0},{1}", arg1 + "1", arg2 + "2");
                        }
                        resultsTwo.Add(String.Format("{0},{1},{2}", fPermutationItem[0], k1CombinationItem[0],
                                fPermutationItem[1]), arg);
                    }
                }
            }
            return resultsTwo;
        }

        private static String buildArgs(String operatorStr, String name, String typeName)
        {
            String args = "";
            String clsName =StringUtil.Instance.ToPascal(typeName);
            name = StringUtil.Instance.ToCamel(name);
            /*if (clsName.Equals("Int", StringComparison.CurrentCultureIgnoreCase))
            {
                clsName = "Integer";
            }*/
            switch (operatorStr) {
		      case "In":
			args = String.Format("Collection<{0}> {1}", clsName, name + "s");
                break;
		 case "Between":
			args = String.Format("{0} {1},{0} {2}", clsName, name + "1", name + "2");
                break;
            default:
			args = String.Format("{0} {1}", clsName, name);
                break;
            }
            return args;
        }

        private static Boolean keywordsIsIgnore(String k1, String k2)
        {
            if (k1.Equals(k2,StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            if (k1.IndexOf(k2) > -1 || k2.IndexOf(k1) > -1)
            {
                return true;
            }
            else if (k1.Split(',')[0].Equals(k2.Split(',')[0], StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
