﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using DAOControllers.ManagerControllers;

namespace DAOControllers
{
    public class DAOEmployee : IGenericRepository<Employee>
    {
        private readonly string _connection = string.Empty;

        public DAOEmployee(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("enterpriseConnection");
        }
        public async Task<List<Employee>> getAll()
        {
            List<Employee> allEmployees = new List<Employee>();

            using (var objConnection = new SqlConnection(_connection))
            {
                SqlDataReader reader;
                try
                {

                    SqlCommand cmd = new SqlCommand("sp_list_all_employees", objConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            allEmployees.Add(new Employee() 
                            {
                                idEmployee = Convert.ToInt32(reader["idEmployee"]),
                                name = reader["name"].ToString(),
                                age = Convert.ToInt32(reader["age"]),
                                sex = reader["sex"].ToString(),
                                workDescription = reader["workDescription"].ToString(),
                                objBranch = new Branch() 
                                {
                                    idBranch = Convert.ToInt32(reader["idBranch"]),
                                    description = reader["enterpriseName"].ToString(),
                                },
                                createdDate = Convert.ToDateTime(reader["createdDate"].ToString())

                            });//End employees listing

                        }
                    }//End information reading

                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    allEmployees = new List<Employee>();
                }


            }//End using of stringConnection


            return allEmployees;
        }//End listing employees

        public async Task<Employee> getById(int idEmployee)
        {
            Employee employee = new Employee();

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlDataReader reader;
                    SqlCommand cmd;

                    cmd = new SqlCommand("sp_select_employee_by_id", objConnection);
                    cmd.Parameters.AddWithValue("idEmployee", idEmployee);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employee = new Employee
                            {
                                idEmployee = Convert.ToInt32(reader["idEmployee"].ToString()),
                                name = reader["name"].ToString(),
                                age = Convert.ToInt32(reader["age"].ToString()),
                                sex = reader["sex"].ToString(),
                                workDescription = reader["workDescription"].ToString(),
                                objBranch = new Branch
                                {
                                    idBranch = Convert.ToInt32(reader["idBranch"].ToString()),
                                    description = reader["enterpriseName"].ToString(),
                                },
                                createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                employee = new Employee(); ;
            }

            return employee;
        }//End get employee id

        public async Task<List<Employee>> allMatches(int idBranch, string name)
        {
            List<Employee> allEmployees = new List<Employee>();

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlDataReader reader;

                    SqlCommand cmd = new SqlCommand("sp_all_employees_matches", objConnection);
                    cmd.Parameters.AddWithValue("idBranch", idBranch);
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            allEmployees.Add(new Employee
                            {
                                idEmployee = Convert.ToInt32(reader["idEmployee"].ToString()),
                                name = reader["name"].ToString(),
                                age = Convert.ToInt32(reader["age"].ToString()),
                                sex = reader["sex"].ToString(),
                                workDescription = reader["workDescription"].ToString(),
                                objBranch = new Branch
                                {
                                    idBranch = Convert.ToInt32(reader["idBranch"].ToString()),
                                    description = reader["enterpriseName"].ToString(),
                                    createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                                }
                            });

                        }
                    }
                }

            }//End query reading
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                allEmployees = new List<Employee>();
            }

            return allEmployees;
        }//End listing employees by branch
        public async Task<List<Employee>> allMatchedBy(int idBranch)
        {
            List<Employee> allEmployees = new List<Employee>();

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlDataReader reader;
                   
                    SqlCommand cmd = new SqlCommand("sp_list_all_employees_by_branch", objConnection);
                    cmd.Parameters.AddWithValue("idBranch", idBranch);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            allEmployees.Add(new Employee
                            {
                                idEmployee = Convert.ToInt32(reader["idEmployee"].ToString()),
                                name = reader["name"].ToString(),
                                age = Convert.ToInt32(reader["age"].ToString()),
                                sex = reader["sex"].ToString(),
                                workDescription = reader["workDescription"].ToString(),
                                objBranch = new Branch
                                {
                                    idBranch = Convert.ToInt32(reader["idBranch"].ToString()),
                                    description = reader["enterpriseName"].ToString(),
                                    createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                                }
                            });

                        }
                    }
                }

            }//End query reading
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                allEmployees = new List<Employee>();
            }

            return allEmployees;
        }//End listing employees by branch

        public async Task<List<Employee>> allMatchedWith(string name)
        {
            List<Employee> allEmployees = new List<Employee>();

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlDataReader reader;

                    SqlCommand cmd = new SqlCommand("sp_all_employees_match_with", objConnection);
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.CommandType = CommandType.StoredProcedure;

                    await objConnection.OpenAsync();

                    using (reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            allEmployees.Add(new Employee
                            {
                                idEmployee = Convert.ToInt32(reader["idEmployee"].ToString()),
                                name = reader["name"].ToString(),
                                age = Convert.ToInt32(reader["age"].ToString()),
                                sex = reader["sex"].ToString(),
                                workDescription = reader["workDescription"].ToString(),
                                objBranch = new Branch
                                {
                                    idBranch = Convert.ToInt32(reader["idBranch"].ToString()),
                                    description = reader["enterpriseName"].ToString(),
                                    createdDate = Convert.ToDateTime(reader["createdDate"].ToString())
                                }
                            });

                        }
                    }
                }

            }//End query reading
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                allEmployees = new List<Employee>();
            }

            return allEmployees;
        }//End listing employees by branch

        public async  Task<int> getMaxId()
        {
            int id = 0;

            using (var objConnection = new SqlConnection(_connection))
            {
                try
                {
                    string consult = "SELECT MAX(idEmployee) as idResult FROM Employee";

                    SqlCommand cmd = new SqlCommand(consult, objConnection)
                    {
                        CommandType = CommandType.Text
                    };

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
        }//End getting max id employee

        public async Task<int> create(Employee objEmployee)
        {
            int employeeGenerated = 0;
            string message = string.Empty;

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {
                    SqlCommand cmd = new SqlCommand("sp_create_employee", objConnection);
                    cmd.Parameters.AddWithValue("name", objEmployee.name);
                    cmd.Parameters.AddWithValue("age", objEmployee.age);
                    cmd.Parameters.AddWithValue("sex", objEmployee.sex);
                    cmd.Parameters.AddWithValue("workDescription", objEmployee.workDescription);
                    cmd.Parameters.AddWithValue("idBranch", objEmployee.objBranch.idBranch);
                    cmd.Parameters.Add("idResult", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                
                    await objConnection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                
                    employeeGenerated = Convert.ToInt32(cmd.Parameters["idResult"].Value);
                    message = cmd.Parameters["message"].Value.ToString();

                }
            }
            catch (Exception ex) 
            {
                employeeGenerated = 0;
                message = ex.ToString();
            }

            Console.WriteLine(message);

            return employeeGenerated;
        }//End create employee

        public async Task<bool> edit(Employee objEmployee)
        {
            bool response = false;
            string message = string.Empty;

                using (var objConnection = new SqlConnection(_connection))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_edit_employee", objConnection);
                        cmd.Parameters.AddWithValue("idEmployee", objEmployee.idEmployee);
                        cmd.Parameters.AddWithValue("name", objEmployee.name);
                        cmd.Parameters.AddWithValue("age", objEmployee.age);
                        cmd.Parameters.AddWithValue("sex", objEmployee.sex);
                        cmd.Parameters.AddWithValue("workDescription", objEmployee.workDescription);
                        cmd.Parameters.AddWithValue("idBranch", objEmployee.objBranch.idBranch);
                        cmd.Parameters.Add("response", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;


                        await objConnection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        response = Convert.ToBoolean(cmd.Parameters["response"].Value);
                        message = cmd.Parameters["message"].Value.ToString();

                    }
                    catch (Exception ex)
                    {
                        response = false;
                        message = ex.Message.ToString();
                    }
                }

            Console.WriteLine(message);

            return response;

        }//End edit employee

        public async Task<bool> delete(int idEmployee)
        {
            bool response = false;
            string message;

            try
            {
                using (var objConnection = new SqlConnection(_connection))
                {

                    SqlCommand cmd = new SqlCommand("sp_delete_employee",objConnection);
                    cmd.Parameters.AddWithValue("idEmployee", idEmployee);
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
                response = false; 
                message = ex.Message;
            }

            Console.WriteLine(message);

            return response;

        }//End delete employee

    }//End DAO employee class
}//End DAOControllers namespace
