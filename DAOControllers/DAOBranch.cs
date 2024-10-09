using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using DAOControllers.ManagerControllers;
using System.Security.Cryptography;

namespace DAOControllers
{
    public class DAOBranch : IGenericRepository<Branch>
    {
        private readonly string _connection = string.Empty;
        public DAOBranch(IConfiguration configuration) 
        {
            _connection = configuration.GetConnectionString("enterpriseConnection");
        }

        public async Task<List<Branch>> getAll()
        {
            List<Branch> allBranches = new List<Branch>();

            try
            {
                SqlDataReader reader;

                using (var objConnection = new SqlConnection(_connection))
                {
                   SqlCommand cmd = new SqlCommand("sp_list_all_branches", objConnection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    await objConnection.OpenAsync();
                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            allBranches.Add(new Branch
                            {
                                idBranch = Convert.ToInt32(reader["idBranch"]),
                                description = reader["description"].ToString(),
                                createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                            });
                        }
                    }
                }
            }catch (Exception ex)
            {
                Console.Write(ex.ToString());
                allBranches = new List<Branch>();
            }

            return allBranches;
        }//End listing all branches

        public async Task<Branch> getById(int idBranch)
        {
            Branch branch = new Branch();
            string message = string.Empty;

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlDataReader reader;
                    SqlCommand cmd;

                    cmd = new SqlCommand("sp_get_by_id_branch", objConnection);
                    cmd.Parameters.AddWithValue("idBranch", idBranch);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            branch = new Branch{
                            idBranch = Convert.ToInt32(reader["idBranch"].ToString()),
                            description = reader["description"].ToString(),
                            createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                            };

                        }
                        
                    }
                }

                message = branch.description;
                   
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                branch = new Branch(); ;
            }

            Console.WriteLine(message);

            return branch;
        }//End get branch id

        public async Task<int> getMaxId()
        {
            int id = 0;

            using (var objConnection = new SqlConnection(_connection))
            {
                try
                {
                    string consult = "SELECT MAX(idBranch) as idResult FROM Branch";

                    SqlCommand cmd = new SqlCommand(consult, objConnection);
                    cmd.CommandType = CommandType.Text;

                    await objConnection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            id = Convert.ToInt32(reader["idResult"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    id = 0;

                }
            }

            id++;

            return id;
        }//End getting max id branch

        public async Task<int> create(Branch objBranch)
        {
            int generatedBranch = 0;

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlCommand cmd = new SqlCommand("sp_create_branch", objConnection);
                    cmd.Parameters.AddWithValue("description",objBranch.description);
                    cmd.Parameters.Add("idResult", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await objConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    generatedBranch = Convert.ToInt32(cmd.Parameters["idResult"].Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                generatedBranch = 0;
            }

            Console.WriteLine(generatedBranch);

            return generatedBranch;
        }//End create branch


        public async Task<bool> edit(Branch objBranch)
        {
            bool response = false;
            string message = string.Empty;

            try
            {

                using(var objConnection = new SqlConnection(_connection))
                {
                    SqlCommand cmd = new SqlCommand("sp_edit_branch", objConnection);
                    cmd.Parameters.AddWithValue("idBranch", objBranch.idBranch);
                    cmd.Parameters.AddWithValue("description", objBranch.description);
                    cmd.Parameters.Add("response", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType= CommandType.StoredProcedure;

                    await objConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    response = Convert.ToBoolean(cmd.Parameters["response"].Value);
                    message = cmd.Parameters["message"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                message= ex.Message.ToString();
                response = false;
            }

            Console.WriteLine(message);

            return response;
        }//End edit branch

        public async Task<bool> delete(int idBranch)
        {
            bool response = false;
            string message = string.Empty;
            
            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlCommand cmd = new SqlCommand("sp_delete_branch", objConnection);
                    cmd.Parameters.AddWithValue("idBranch", idBranch);
                    cmd.Parameters.Add("response", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await objConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    response = Convert.ToBoolean(cmd.Parameters["response"].Value);
                    message = cmd.Parameters["message"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                message= ex.Message.ToString();
                response = false;
            }

            Console.WriteLine(message);

            return response;
        }//End delete branch


    }//End DAO branch class
}//End namespace
