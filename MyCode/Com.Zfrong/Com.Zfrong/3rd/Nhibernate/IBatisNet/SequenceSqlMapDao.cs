
using System;
using IBatisNet.DataMapper.Exceptions;
using Com.Zfrong.Common.Data.IBatisNet.Domain;
namespace Com.Zfrong.Common.Data.IBatisNet.Persistence.MapperDao
{
	/// <summary>
	/// Summary description for SequenceSqlMapDao.
	/// </summary>
    public class SequenceSqlMapDao : BaseSqlMapDao, ISequenceDao
	{
		#region ISequenceDao Members

		/// <summary>
		/// This is a generic sequence ID generator that is based on a database
		/// table called 'SEQUENCE', which contains two columns (NAME, NEXTID).
		/// </summary>
		/// <param name="name">name The name of the sequence.</param>
		/// <returns>The Next ID</returns>
		public int GetNextId(string name)
		{
			Sequence sequence = new Sequence(name, -1);

			sequence = ExecuteQueryForObject<Sequence>("GetSequence", sequence);
			if (sequence == null) 
			{
				throw new DataMapperException("Error: A null sequence was returned from the database (could not get next " + name + " sequence).");
			}
			object parameterObject = new Sequence(name, sequence.NextId + 1);
			ExecuteUpdate("UpdateSequence", parameterObject);

			return sequence.NextId;
		}

		#endregion
	}
}
