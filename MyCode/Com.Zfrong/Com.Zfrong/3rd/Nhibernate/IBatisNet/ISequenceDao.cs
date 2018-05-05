
using System;
namespace Com.Zfrong.Common.Data.IBatisNet
{
	/// <summary>
	/// Summary description for ISequenceDao.
	/// </summary>
	public interface ISequenceDao
	{
		int GetNextId(string name);
	}
}
