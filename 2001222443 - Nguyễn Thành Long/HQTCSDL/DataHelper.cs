using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HQTCSDL
{
    public class DataHelper
    {
        private readonly string connectionString;
        public string ConnectionString => connectionString;
        public DataHelper()
        {
            // Lấy chuỗi kết nối từ App.config
            connectionString = ConfigurationManager.ConnectionStrings["GymDBConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Trả về DataTable kết quả truy vấn SELECT
        /// </summary>
        public DataTable ExecuteQuery(string sql, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Thực thi câu lệnh INSERT/UPDATE/DELETE hoặc Stored Procedure không trả kết quả
        /// </summary>
        public int ExecuteNonQuery(string sql, SqlParameter[] parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Lấy 1 giá trị đơn (ví dụ: COUNT(*), MAX, MIN, ...)
        /// </summary>
        public object ExecuteScalar(string sql, SqlParameter[] parameters = null)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        /// <summary>
        /// Thực thi Stored Procedure với tham số (INSERT, UPDATE, DELETE, ...).
        /// </summary>
        public object ExecuteStoredProcedureScalar(string procName, SqlParameter[] parameters = null)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        public int ExecuteStoredProcedureScalarInt(string procName, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(procName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int count))
                    return count;
                return 0;
            }
        }

    }
}
