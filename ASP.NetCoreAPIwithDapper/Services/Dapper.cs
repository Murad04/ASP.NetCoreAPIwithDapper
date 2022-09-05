using ASP.NetCoreAPIwithDapper.Services.Interface;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ASP.NetCoreAPIwithDapper.Services
{
    public class Dapper : IDapper
    {
        private IConfiguration _configuration;
        private string ConnectionString = "DefaultConnection";

        public Dapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Delete<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(ConnectionString));
            return db.Query<T>(sp, dynamicParameters, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(ConnectionString));
            return db.Query<T>(sp, dynamicParameters, commandType: commandType).ToList();
        }

        public DbConnection GetDbConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString(ConnectionString));
        }

        public T Insert<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed) db.Open();
                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, dynamicParameters, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open) db.Close();
            }
            return result;
        }

        public T Update<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString(ConnectionString));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, dynamicParameters, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }
}
