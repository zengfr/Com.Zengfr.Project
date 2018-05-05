/*

$Header$
$Author$
$Date$ 
$Revision$
$History$

*/

using System;

namespace Com.Zfrong.Common.Data.NH.ActiveRecords
{
    /// <summary>
    /// 统计(聚合)方式
    /// </summary>
    public enum AggregateEnum
    {
        Avg,
        Count,
        CountDistinct,
        Max,
        Min,
        Sum,
    }
}