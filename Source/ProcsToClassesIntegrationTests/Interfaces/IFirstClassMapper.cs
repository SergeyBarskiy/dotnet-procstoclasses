
using System;
using System.Data.Common;
namespace Abstractions
{
	public interface IFirstClassMapper
	{
		FirstClass Map(DbDataReader reader);
	}
}
