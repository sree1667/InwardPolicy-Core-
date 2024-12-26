using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class DBConnection
    {
        private static string ConnString = GetConnectionString("ConnectionString", string.Empty);
        public static string GetConnectionString(string key, string defaultValue)
        {
            string value = string.Empty;
            if (string.IsNullOrEmpty(defaultValue))
            {
                value = defaultValue;
            }
            try
            {
                value = "Data Source=ABSDK108/ORCL19C;User ID=E0188;Password=e0188;";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error reading webconfig : " + ex.ToString());
            }
            return value;
        }
        public static OracleConnection OpenConnection()
        {
            try
            {
                OracleConnection con = new OracleConnection();
                con.ConnectionString = ConnString;
                con.Open();
                return con;
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {

            }
        }

        public static void CloseConnection(OracleConnection connection)
        {
            try
            {
                if (connection != null)
                    connection.Close();
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
            }
        }

        public static int ExecuteQuery(string sql)
        {
            OracleConnection connection = null;
            try
            {
                OracleCommand cmd = new OracleCommand();
                connection = OpenConnection();
                cmd.CommandText = sql;
                cmd.Connection = connection;
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;

            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public static DataTable ExecuteDataset(string query)
        {
            OracleCommand cmd;
            DataSet Dset = new DataSet();
            OracleConnection connection = null;
            try
            {
                connection = OpenConnection();
                cmd = new OracleCommand(query, connection);
                OracleDataAdapter Da = new OracleDataAdapter(cmd);
                Da.Fill(Dset);
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    CloseConnection(connection);
            }
            if (Dset != null && Dset.Tables[0] != null)
            {
                return Dset.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static object ExecuteScalar(string query)
        {
            DataSet Dset = new DataSet();
            OracleConnection connection = null;
            try
            {
                connection = OpenConnection();
                OracleCommand objcmd = new OracleCommand(query, connection);
                object outcount = objcmd.ExecuteScalar();
                return outcount;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    CloseConnection(connection);
            }
            if (Dset != null && Dset.Tables[0] != null)
            {
                return Dset.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static int ExecuteQuery(Dictionary<string, object> paramValues, string query)
        {

            OracleCommand cmd = new OracleCommand();
            OracleConnection connection = null;
            int rval;
            try
            {

                connection = OpenConnection();
                cmd.CommandType = CommandType.Text;

                foreach (KeyValuePair<string, object> pValue in paramValues)
                {

                    if (query.Contains(":" + pValue.Key.ToString()))
                    {
                        if (pValue.Value != null && !string.IsNullOrEmpty(pValue.Value.ToString()))
                        {
                            cmd.Parameters.Add(pValue.Key.ToString(), pValue.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add(pValue.Key.ToString(), DBNull.Value);
                        }
                    }
                }
                cmd.CommandText = query;
                cmd.Connection = connection;
                rval = cmd.ExecuteNonQuery();
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    CloseConnection(connection);
            }
            return rval;

        }

        public static (long, string) ExecuteProc(int polUid, string polCrBy)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection connection = null;
            try
            {
                string msg = string.Empty;
                connection = OpenConnection();
                cmd.CommandText = "DPRC_COPY";
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameters
                cmd.Parameters.Add("P_POL_UID", OracleType.Long).Value = polUid;
                cmd.Parameters.Add("P_CRBY", OracleDbType.Varchar2).Value = polCrBy;

                // Output Parameters
                cmd.Parameters.Add("M_NEW_POL_UID", OracleDbType.Long).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("M_ERROR_MESSAGE", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;

                cmd.Connection = connection;

                // Execute the Procedure
                cmd.ExecuteNonQuery();

                // Retrieve Output Parameters
                long newPolUid = Convert.ToInt64(cmd.Parameters["M_NEW_POL_UID"].Value);
                msg = cmd.Parameters["M_ERROR_MESSAGE"].Value?.ToString();

                return (newPolUid, msg);
            }
            catch (OracleException sqlerr)
            {
                throw new Exception($"Database error: {sqlerr.Message}", sqlerr);
            }
            catch (Exception err)
            {
                throw new Exception($"Unhandled error: {err.Message}", err);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    CloseConnection(connection);
            }
        }

        //public static (long, string) ExecuteProc(int polUid, string polCrBy)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    OracleConnection connection = null;
        //    try
        //    {
        //        string msg = string.Empty;
        //        connection = OpenConnection();
        //        cmd.CommandText = "DPRC_COPY";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("P_POL_UID", OracleDbType.Long).Value = polUid;
        //        cmd.Parameters.Add("P_CRBY", OracleDbType.Varchar2).Value = polCrBy;
        //        cmd.Parameters.Add("M_NEW_POL_UID", OracleDbType.Long).Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add("M_ERROR_MESSAGE", OracleDbType.Varchar2, 400000).Direction = ParameterDirection.Output;
        //        cmd.Connection = connection;
        //        cmd.ExecuteNonQuery();
        //        object poluid = cmd.Parameters["M_NEW_POL_UID"].Value;
        //        //var msg= cmd.Parameters["M_ERROR_MESSAGE"].Value);
        //        int pPolUid = Convert.ToInt32(poluid);
        //        long a = Convert.ToInt64(pPolUid);
        //        return (a, msg);
        //    }
        //    catch (OracleException sqlerr)
        //    {
        //        throw sqlerr;
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    finally
        //    {
        //        if (connection != null && connection.State == ConnectionState.Open) CloseConnection(connection);
        //    }
        //}

        public static DataSet ExecuteQuerySelect(Dictionary<string, object> paramValues, string query)
        {

            OracleCommand cmd = new OracleCommand();
            OracleConnection connection = null;
            DataSet Dset = new DataSet();
            try
            {

                connection = OpenConnection();
                cmd.CommandType = CommandType.Text;

                foreach (KeyValuePair<string, object> pValue in paramValues)
                {

                    if (query.Contains(":" + pValue.Key.ToString()))
                    {
                        if (pValue.Value != null && !string.IsNullOrEmpty(pValue.Value.ToString()))
                        {
                            cmd.Parameters.Add(pValue.Key.ToString(), pValue.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add(pValue.Key.ToString(), DBNull.Value);
                        }
                    }
                }
                cmd.CommandText = query;
                cmd.Connection = connection;
                OracleDataAdapter Da = new OracleDataAdapter(cmd);
                Da.Fill(Dset);
            }
            catch (OracleException sqlerr)
            {
                throw sqlerr;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    CloseConnection(connection);
            }
            return Dset;

        }


    }
}

