using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace CertificatesOfDeposit.Infrastructure
{
    public enum DatabaseConnectionName
    {
        Connection1,
        Connection2
    }
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(DatabaseConnectionName connectionName);
    }
    public class DapperDbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDictionary<DatabaseConnectionName, string> _connectionDict;

        public DapperDbConnectionFactory(IDictionary<DatabaseConnectionName, string> connectionDict)
        {
            _connectionDict = connectionDict;
        }

        public IDbConnection CreateDbConnection(DatabaseConnectionName connectionName)
        {
            string connectionString = null;
            if (_connectionDict.TryGetValue(connectionName, out connectionString))
            {
                return new OracleConnection(connectionString);
            }

            throw new ArgumentNullException();
        }
    }
}
