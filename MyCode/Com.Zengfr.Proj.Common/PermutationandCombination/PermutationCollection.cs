using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MS.CodePlex.PermutationTools.Exceptions;



namespace MS.CodePlex.PermutationTools
{
    /// <summary>
    /// a class that allows users to easily create permuation lists using arbitrary array types and enums.
    /// </summary>
    /// <example>
    /// <code>
    /// public static void example()
    /// {
    ///     PermutationCollection pg = new PermutationCollection();
    ///     string[] stringParameters = { "default", "abcdef", "1234567890", "" };
    ///     int[] intParameters = { 1, 3, 5 };
    ///
    ///     pg.Add("PrimeNumber", intParameters);
    ///     pg.Add("UserName", stringParameters);
    ///
    ///     //automatically add all the members of an enum.
    ///     pg.Add("GenericUriParserOption", typeof(System.GenericUriParserOptions));
    ///     //Remove one of the enum members from an added enum.
    ///     pg.RemoveListMember("GenericUriParserOption", System.GenericUriParserOptions.NoUserInfo);
    ///
    ///     Console.WriteLine(pg.NumOfPermutations);
    ///
    ///     //display every combination of paramters
    ///     foreach (Dictionary&lt;string, object> parameters in pg)
    ///     {
    ///         int prime = (int)parameters["PrimeNumber"];
    ///         string username = (string)parameters["UserName"];
    ///         System.GenericUriParserOptions option = (GenericUriParserOptions)parameters["GenericUriParserOption"];
    ///         Console.WriteLine("Prime='{0}' UserName='{1}' Option='{2}'", prime, username, option);
    ///     }
    /// }
    ///       
    /// 
    /// /*
    /// OUTPUT:
    /// 99
    /// Prime='1' UserName='' Option='Default'
    /// Prime='3' UserName='' Option='Default'
    /// Prime='5' UserName='' Option='Default'
    /// Prime='1' UserName='abcdef' Option='Default'
    /// Prime='3' UserName='abcdef' Option='Default'
    /// Prime='5' UserName='abcdef' Option='Default'
    /// Prime='1' UserName='1234567890' Option='Default'
    /// Prime='3' UserName='1234567890' Option='Default'
    /// Prime='5' UserName='1234567890' Option='Default'
    /// Prime='1' UserName='' Option='GenericAuthority'
    /// Prime='3' UserName='' Option='GenericAuthority'
    /// Prime='5' UserName='' Option='GenericAuthority'
    /// ...
    /// */
    /// </code>
    /// </example>
    public class PermutationCollection : IEnumerable
    {
        private List<IList> listOfLists = new List<IList>();
        private List<int> indexOfLists = new List<int>();
        private List<string> namesOfLists = new List<string>();
        private Dictionary<string,object> permutation;
        private string pattern = string.Empty;
        private int index;

        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>        
        public PermutationCollection()
        {           
        }

        #endregion

        #region public Accessors
        /// <summary>
        /// Return the index of the last accessed permutation.
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Return a string that lists one digit for each parameter indicating the last index of each parameter.
        /// </summary>
        public String Pattern
        {
            get { return pattern.Trim(); }
        }


        /// <summary>
        /// The number of permutations the current lists would generate.
        /// </summary>
        public int NumOfPermutations
        {
            get
            {
                int p = 1;
                foreach (IList l in listOfLists)
                {
                    p *= l.Count;
                }
                return p;
            }
        }

        /// <summary>
        /// Returns how many results are needed to visit each value only once.
        /// </summary>
        public int NumOfMinimalSet
        {
            get
            {
                int total = 1;
                foreach (IList l in listOfLists)
                {
                    total += l.Count - 1;
                }
                return total;
            }
        }

        /// <summary>
        /// Returns a result set of Dictionary&gt;string,object&lt; that will visit each value once.
        /// </summary>
        public IEnumerable MinimalSet
        {
            get
            {
                return new MinimalSetCollection(this);
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a list and a parameter type to the object.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type">typeof(MyType)</param>
        /// <param name="array">An array of type</param>
        public void Add(string name, Array array)
        {
            //convert any array type to an object[] to feed to the main overload.
            object[] oArray = new object[array.Length];
            array.CopyTo(oArray, 0);            
            Add(name, oArray);
        }



        /// <summary>
        /// Adds a named list to the object.
        /// </summary>
        /// <remarks>
        /// This is the master overload. Other overloads call into this one.
        /// </remarks>
        public void Add(String name, object[] parameterList)
        {
            if (parameterList.Length == 0)
            {
                throw new ArgumentException("Lists may not be empty.");
            }

            Type parameterType = parameterList[0].GetType();

            if (namesOfLists.Contains(name))
            {
                throw new UniqueParameterNameViolationException("Must give each parameter a unique name.");
            }

            if (((PermutationEnumerator)this.GetEnumerator()).Index != -1)
            {
                throw (new CalledAddAfterNextException("Can't after Next() has been called."));
            }

            //Create a generic list of the underlying object on the fly using reflection.
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(parameterType));
            
            //add the items into the list.
            foreach (object o in parameterList)
            {
                list.Add(o);
            }
            //add the list to the master list.
            listOfLists.Add(list);

            //set the initial index to this list.
            indexOfLists.Add(0);

            //record the name.
            namesOfLists.Add(name);
        }

        /// <summary>
        /// Add an enum as list of parameters.
        /// </summary>
        /// <remarks>
        /// Uses the Enum.GetValues to convert an enum into an array.
        /// </remarks>
        /// <param name="type"></param>
        public void Add(string name, Type type)
        {
            if (type.IsEnum == false)
            {
                throw new TypeIsNotEnumBasedException("You may only add Enum types with the Add(String, Type) overload.");
            }


            List<Object> list = new List<Object>();
            foreach (Enum e in Enum.GetValues(type))
            {
                list.Add(e);
            }

            //take the converted list and pass it to the other overloads.            
            Add(name, list.ToArray());
        }

        /// <summary>
        /// Removes one  members from a previously added list.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="functionalArea"></param>
        public void RemoveListMember(string p, Enum enumMember)
        {
            int i = namesOfLists.IndexOf(p);
            listOfLists[i].Remove(enumMember);            
        }


        /// <summary>
        /// Gets the enumerator to allow foreach operations that returns Dictionary&lt;string,object&gt; array that will visit every combination.
        /// </summary>
        /// <returns>ParameterGenerator</returns>
        public IEnumerator GetEnumerator()
        {
            return new PermutationEnumerator(this);
        }

        #endregion

        #region enumerators and nested enumerator classes

        abstract internal class PermutationEnumeratorBase : IEnumerator
        {
            internal PermutationCollection permutationCollection;
            internal int index = -1;

            internal PermutationEnumeratorBase(PermutationCollection obj)
            {
                permutationCollection = obj;
            }

            #region internal class public Accessors
            /// <summary>
            /// The current pattern index.
            /// </summary>
            public int Index
            {
                get { return index; }
            }

            #endregion

            /// <summary>
            /// Returns a space delimited string of digits indicating which elements of each list are being visited in the current value.
            /// </summary>
            public string CurrentPattern
            {
                get
                {
                    StringBuilder result = new StringBuilder();
                    foreach (int i in permutationCollection.indexOfLists)
                    {
                        result.Append(i);
                        result.Append(" ");
                    }
                    return result.ToString();
                }
            }

            internal Dictionary<string, object> Current
            {
                get { return permutationCollection.permutation; }
            }

            /// <summary>
            /// checks the current state if the counters and "carries the one" if any items are past the limit.
            /// </summary>
            internal void rollOdometer()
            {
                for (int i = 0; i < permutationCollection.indexOfLists.Count; i++)
                {
                    if (permutationCollection.indexOfLists[i] == permutationCollection.listOfLists[i].Count)
                    {
                        if (i < permutationCollection.indexOfLists.Count - 1) //carry the one to the next parameter
                        {

                            permutationCollection.indexOfLists[i] = 0;
                            permutationCollection.indexOfLists[i + 1]++;

                        }
                    }
                }
            }

            /// <summary>
            /// Check to see if the count has gone off the end.
            /// </summary>
            /// <returns></returns>
            internal bool isCountValid()
            {
                bool isValid = true;
                for (int i = 0; i < permutationCollection.listOfLists.Count; i++)
                {
                    if (permutationCollection.indexOfLists[i] == permutationCollection.listOfLists[i].Count)
                    {
                        isValid = false;
                    }
                }
                return isValid;
            }

            /// <summary>
            /// Updates the .Current object. Don't call this unless isCountValid == true!
            /// </summary>
            /// <returns></returns>
            internal Dictionary<string, object> createNewCurrentDictionary()
            {
                
                Dictionary<string, object> returnDictionary = new Dictionary<string, object>();
                for (int i = 0; i < permutationCollection.listOfLists.Count; i++)
                {
                        returnDictionary.Add(permutationCollection.namesOfLists[i], permutationCollection.listOfLists[i][permutationCollection.indexOfLists[i]]);
                }
                return returnDictionary;
            }

            /// <summary>
            /// Call during MoveNext() to update the collection.
            /// </summary>
            internal void updateCollectionItems()
            {
                index++;
                permutationCollection.permutation = createNewCurrentDictionary();
                permutationCollection.index = this.index;
                permutationCollection.pattern = this.CurrentPattern;
            }

            #region IEnumerator Members


            abstract public bool MoveNext();

            public void Reset()
            {
                index = -1;
                for (int i = 0; i < permutationCollection.indexOfLists.Count; i++)
                {
                    permutationCollection.indexOfLists[i] = 0;
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            #endregion
        }


        /// <summary>
        /// An enumeration class for all permuations in a collection
        /// </summary>
        internal class PermutationEnumerator : PermutationEnumeratorBase, IEnumerator
        {

            internal PermutationEnumerator(PermutationCollection obj) : base(obj)
            {
                
            }

            #region Public Ovveride Methods


            override public bool MoveNext()
            {             
   
                bool validCount = isCountValid();//check to see that the current count hasn't overflowed the list.
                if (validCount)
                {
                    updateCollectionItems();
                    permutationCollection.indexOfLists[0]++; //add one to the 0th list.
                    rollOdometer(); //check each element of the list in turn and "carry the one" if they length is exceeded.                  
                }

                return validCount;                  
            }           

           

            #endregion

            #region IEnumerator Members

            

            #endregion
        }


        /// <summary>
        /// Returns a collection based on the permutation collection. Every parameter will be returned at least once, but not every permutation.
        /// </summary>
        /// <remarks>
        /// The 0th item of each array will be considered the default and will show up most often.
        /// </remarks>
        /// <returns></returns>        
        internal class PermutationEnumeratorMinimal : PermutationEnumeratorBase, IEnumerator
        {            
            int currentList = 0;

            internal PermutationEnumeratorMinimal(PermutationCollection obj) : base(obj)
            {}             

            #region IEnumerator Members            
            /// <summary>
            /// Move to the next item in the minimal set.
            /// </summary>
            /// <returns></returns>
            override public bool MoveNext()
            {
                bool validCount = isCountValid();
                if (validCount)
                {
                    updateCollectionItems();
                    permutationCollection.indexOfLists[currentList]++; //move the current list to the next item.

                    //check if the current list is at it's limit and if so move on to updating the next list on the next iteration.
                    if (permutationCollection.indexOfLists[currentList] == permutationCollection.listOfLists[currentList].Count)
                    {
                        currentList++;
                    }
                    rollOdometer();//check for overflows and "carry the one" to the next list if so.
                }
                return validCount;
            }
            #endregion
        }


        /// <summary>
        /// This is a wrapper class to allow the PermutationCollection to expose a second enumerator.
        /// </summary>
        private class MinimalSetCollection : IEnumerable
        {
            private PermutationCollection permutationCollection;

            /// <summary>
            /// Returns a collection that will visit every element of every list once.
            /// </summary>
            /// <param name="obj"></param>
            public MinimalSetCollection(PermutationCollection obj)
            {
                permutationCollection = obj;
            }

            /// <summary>
            /// returns and enumerator for use with Foreach that returns a Dictionary&lt;string,object&lt; of the current parameters.
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator()
            {
                return new PermutationEnumeratorMinimal(permutationCollection);
            }

        }

        #endregion

        
    }


}
