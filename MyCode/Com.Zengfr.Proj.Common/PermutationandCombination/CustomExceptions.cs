using System;



namespace MS.CodePlex.PermutationTools.Exceptions
{

    /// <summary>
    /// Exception indicating that the need for unique parameters has been violated.
    /// </summary>
    [global::System.Serializable]
    public class UniqueParameterNameViolationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UniqueParameterNameViolationException() { }
        public UniqueParameterNameViolationException(string message) : base(message) { }
        public UniqueParameterNameViolationException(string message, Exception inner) : base(message, inner) { }
        protected UniqueParameterNameViolationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception indicating the add cannot be called once you are traversing the collection.
    /// </summary>
    [global::System.Serializable]
    public class CalledAddAfterNextException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public CalledAddAfterNextException() { }
        public CalledAddAfterNextException(string message) : base(message) { }
        public CalledAddAfterNextException(string message, Exception inner) : base(message, inner) { }
        protected CalledAddAfterNextException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// <summary>
    /// Exception indicating that the type passed to the object isn't an enum.
    /// </summary>
    [global::System.Serializable]
    public class TypeIsNotEnumBasedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TypeIsNotEnumBasedException() { }
        public TypeIsNotEnumBasedException(string message) : base(message) { }
        public TypeIsNotEnumBasedException(string message, Exception inner) : base(message, inner) { }
        protected TypeIsNotEnumBasedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


}
