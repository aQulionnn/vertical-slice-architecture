using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApi.Interceptors;

public class SqlLoggingInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        Debug.WriteLine($"SQL Executed: {command.CommandText}");
        return base.ReaderExecuting(command, eventData, result);
    }
}