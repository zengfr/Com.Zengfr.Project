using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MS.CodePlex.PermutationTools.Exceptions;

namespace MS.CodePlex.PermutationTools
{
    /// <summary>
    /// A collection of all the combinations of an array or enum where order is irrelevant.
    /// </summary>
    /// <remarks>
    /// This collection allows the user to get N wise combinations of the input list. N can be 1 to the size of the list.
    /// Foreaching over the collection will return and object array with each combination in turn.
    /// 
    /// The OneToNPermutations will give all the combinations of 1, 2..N where N is the size of this list. This could be
    /// used for example to get all of the possible combinations of a bit flag enum.
    /// </remarks>
    /// <example>
    /// This is a small console application that will first show all the combinations possible with three picks from a list of 5 items.
    /// Then it will show all of the combinations of 1, 2, 3, 4, and 5 items that are possible by using the OneToNPermutations enumerator.
    /// <code>
    ///    class Program
    ///    {
    ///
    ///        enum stuff
    ///        {
    ///            A,
    ///            B,
    ///            C,
    ///            D,
    ///            E
    ///        }
    ///
    ///        static void Main(string[] args)
    ///        {
    ///            //get a list of everyone combinations of 3 items from the list.      
    ///           CombinationGenerator&lt;stuff> cg = new CombinationGenerator&lt;stuff>(typeof(stuff), 3);
    ///           Console.WriteLine(cg.NumOfCombinations);
    ///           foreach (stuff[] oa in cg)
    ///           {
    ///              foreach (stuff o in oa)
    ///               {
    ///                   Console.Write("{0} ", o);
    ///               }
    ///               Console.WriteLine();
    ///           }
    ///
    ///
    ///           Console.WriteLine("----------------------------------");
    ///           Console.WriteLine(cg.NumOfOneToNCombinations);
    ///           //Get a list of all the combinations of 1, 2..N items from the list where N is the number of items in the list.
    ///           foreach (stuff[] oa in cg.OneToNPermutations)
    ///           {
    ///               foreach (stuff o in oa)
    ///               {
    ///                   Console.Write("{0} ", o);
    ///               }
    ///               Console.WriteLine();
    ///           }
    ///        }
    ///    }
    /// </code>
    /// </example>
    /// <typeparam name="T">The element type for the underlying collection.</typeparam>
    public class CombinationGenerator<T> : IEnumerable
    {
        T[] array;
        int pick;
        List<int> listOfIndexes = new List<int>();
        T[] current;
        ulong numCombinations = 0;
        ulong numOneToNcombinations = 0;

        #region Constructors
        /// <summary>
        /// Instantiate a new collection based on any array.
        /// </summary>
        /// <param name="array">any array</param>
        /// <param name="pick">The number of combination slots to create. Must be less than or equal too array.Length</param>       
        public CombinationGenerator(T[] array, int pick)
        {
            this.array = array;
            init(pick);
        }

        /// <summary>
        /// Instantite a collection based off an enum type.
        /// </summary>
        /// <remarks>
        /// Uses the Enum.GetValues(type) method to convert an Enum into an array.
        /// </remarks>
        /// <param name="type">typeof(SomeEnum)</param>
        /// <param name="pick">Number of combination slots to create. Must be less than or equal the number of elements in the Enum.</param>
        public CombinationGenerator(Type type, int pick)
        {
            if (type.IsEnum == false)
            {
                throw new TypeIsNotEnumBasedException("You may only add Enum types with this Add(Type, int) overload.");
            }

            List<T> list = new List<T>();
            foreach (T e in Enum.GetValues(type))
            {
                list.Add(e);
            }

            this.array = list.ToArray();
            init(pick);
        }



        //helper init for all constructors
        private void init(int pick)
        {
            if (array.Length < pick)
            {
                throw new ArgumentException("pick must not be larger than the number of items.");
            }


            this.pick = pick;
            int x = 0;
            for (int i = 0; i < pick; i++)
            {
                listOfIndexes.Add(x++);
            }

            current = new T[pick];

        }

        #endregion

        #region public properties

        /// <summary>
        /// Returns the current array of items for use with foreach
        /// </summary>
        public T[] Current
        {
            get { return current; }
        }

        /// <summary>
        /// Returns the number of order indepenant combinations for the giving list with the giving number of picks.
        /// </summary>
        public ulong NumOfCombinations
        {
            //only calculate on demand and then cache the result. Factorial is expensive.            
            get
            {
                if (numCombinations == 0)
                {
                    numCombinations = choose((ulong)array.Length, (ulong) pick);
                    return numCombinations;
                }
                return numCombinations;
            }
        }


        /// <summary>
        /// Returns the number of combinations of the list 1 wise + 2 wise ... N Wise where N is the number of items in the list.
        /// </summary>
        public ulong NumOfOneToNCombinations
        {
            get
            {
                if (numOneToNcombinations == 0)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        numOneToNcombinations += choose((ulong)array.Length, (ulong)i);
                    }
                }
                return numOneToNcombinations;
            }

        }
        #endregion


        #region public members
        /// <summary>
        /// Exposes a collection of all combinations (order doesn't matter) sized one to the number of items.
        /// </summary>
        /// <remarks>
        /// Exposes a collection that contains all the the combinations of the underlying list from 1 to N where N is the size of the list.
        /// For example if your list was a 3 item enum {A, B, C,} the output would show all the single, all the pairwise combinations and all the 3 wise combinations.
        /// 
        /// A
        /// B
        /// C
        /// AB
        /// AC
        /// BC
        /// ABC
        /// 
        /// Useful for creating lists of all possible combinations of things like enum permissions.
        /// </remarks>
        /// <example>
        /// <code>
        /// foreach(object[] list in combinationaGenerator.OnToNPermutations)
        /// {
        ///  foreach(object item in list) { Console.Write("{0} ", item)}
        ///  Console.WriteLine();
        /// }
        /// </code>
        ///
        /// </example>
        public IEnumerable OneToNPermutations
        {
            get { return new CombinationOneToNenumerable(this); }
        }

        #endregion

        #region publicMethods
        #endregion

        #region private methods

        ulong choose(ulong n, ulong k)
        {
            ulong result = 1;

            for (ulong i = Math.Max(k, n - k) + 1; i <= n; ++i)
                result *= i;

            for (ulong i = 2; i <= Math.Min(k, n - k); ++i)
                result /= i;

            return result;
        }



        #endregion

        #region internal methods
        internal bool isCountValid()
        {
            bool isValid = true;
            for (int i = 0; i < pick; i++)
            {
                if (listOfIndexes[i] == array.Length)
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        internal void updateCurrent()
        {
            for (int i = 0; i < pick; i++)
            {
                Current.SetValue(array.GetValue(listOfIndexes[i]), i);                
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns the Enumerator class for use with foreach
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new CombinationEnumerator(this);
        }

        /// <summary>
        /// the default enumerator.
        /// </summary>
        internal class CombinationEnumerator : IEnumerator
        {

            CombinationGenerator<T> combinationGenerator;
            List<int> listOfMaxes = new List<int>();

            /// <summary>
            /// Enemerator construtor. Takes a parent CombinationGenerator that has been initialized to type T.
            /// Since this is only called from CombinationGenerator T is garuanteed to match.
            /// </summary>
            /// <param name="obj"></param>
            internal CombinationEnumerator(CombinationGenerator<T> obj)
            {
                combinationGenerator = obj;


                listOfMaxes.AddRange(new int[combinationGenerator.pick]);

                int y = combinationGenerator.pick - 1;
                for (int i = 0; i < combinationGenerator.pick; i++)
                {
                    listOfMaxes[y--] = combinationGenerator.array.Length - i;
                }
            }

            #region IEnumerator Members

            /// <summary>
            /// Returns the current array of items.
            /// </summary>
            public object Current
            {
                get { return combinationGenerator.current; }
            }

            /// <summary>
            /// Moves .Current to the next pattern.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {

                
                bool isValid = combinationGenerator.isCountValid();
                if (isValid)
                {
                    //Set current to the current value.
                    combinationGenerator.updateCurrent();
                    //update counters for the next go round.
                    combinationGenerator.listOfIndexes[combinationGenerator.listOfIndexes.Count - 1]++; //add one to the last list.
                    
                    //start at the end of the list and check if anything is overflowed.
                    //Don't go down to the 0th list so that i-1 still works.
                    for (int i = combinationGenerator.listOfIndexes.Count - 1; i > 0; i--)
                    {
                        //check if current list is at it's max.
                        if (combinationGenerator.listOfIndexes[i] == listOfMaxes[i])
                        {                            
                            //add one to the previous list.
                            combinationGenerator.listOfIndexes[i - 1]++;

                            //Once we visited an item in a previous list we don't need to see it in a later list.
                            //Sine the current item is in overflow we reset all the remaining items to be in a pattern
                            // like N, N+1, N+2, ... from the current item to the end of the list.
                            for (int j = i; j < listOfMaxes.Count; j++ )
                            {
                                combinationGenerator.listOfIndexes[j] = combinationGenerator.listOfIndexes[j - 1] + 1;
                            }
                        }
                    }
                }
                return isValid;
            }

            /// <summary>
            /// Resets the object to start over from the begining.
            /// </summary>
            public void Reset()
            {
                for (int i = 0; i < combinationGenerator.listOfIndexes.Count; i++)
                {
                    combinationGenerator.listOfIndexes[i] = 0;
                }
            }

            #endregion
        }

        
        /// <summary>
        /// Wrapper enumerable class to expose a second enumerator type for the class.
        /// </summary>
        internal class CombinationOneToNenumerable : IEnumerable
        {

            CombinationGenerator<T> combinationGenerator;
            internal CombinationOneToNenumerable(CombinationGenerator<T> obj)
            {
                combinationGenerator = obj;
            }

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new CombinationOneToNenumerator(combinationGenerator);
            }

            #endregion
        }

        internal class CombinationOneToNenumerator : IEnumerator
        {
            CombinationGenerator<T> combinationGenerator;
            CombinationGenerator<T> subGenerator;
            CombinationEnumerator subEnumerator;
            int N = 1;
            internal CombinationOneToNenumerator(CombinationGenerator<T> obj)
            {
                combinationGenerator = obj;
                subGenerator = new CombinationGenerator<T>(combinationGenerator.array, 1);
                subEnumerator = new CombinationEnumerator(subGenerator);                
            }

            #region IEnumerator Members

            /// <summary>
            /// Returns the current array of items.
            /// </summary>
            public object Current
            {
                get { return combinationGenerator.current; }
            }


            /// <summary>
            /// Moves .Current to the next pattern in the list.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                bool returnVal = true;
                
                //This pattern uses multiple combinationGenerators in turn call subGenerator.
                //Set the top level generator to be the same as the current subGenerator.
                combinationGenerator.current = subGenerator.current;


                //Call movenext on the current subgenerator.
                if (!subEnumerator.MoveNext())
                {
                    //If the current generator returned false, we are done with it and create a new one for the 
                    //next sized combination. (Got from single to pairwise to threewise to N);
                    if (N < combinationGenerator.array.Length)
                    {
                        N++;
                        subGenerator = new CombinationGenerator<T>(combinationGenerator.array, N);
                        subEnumerator = new CombinationEnumerator(subGenerator);                                                
                    }
                    else
                    {
                        //all out of patterns. We used up all the needed subgenerators.
                        returnVal = false;
                    }
                }

                return returnVal;


            }

            /// <summary>
            /// Resest the object to start over from the first pattern.
            /// </summary>
            public void Reset()
            {
                for (int i = 0; i < combinationGenerator.listOfIndexes.Count; i++)
                {
                    combinationGenerator.listOfIndexes[i] = 0;
                    N = 0;
                }
            }
            #endregion
        }




        #endregion
    }
}
