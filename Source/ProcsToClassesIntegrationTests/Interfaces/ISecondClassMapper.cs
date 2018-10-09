
using System;
using System.Data.Common;
namespace Abstractions
{
	public interface ISecondClassMapper
	{
		SecondClass Map(DbDataReader reader);
	}
}
