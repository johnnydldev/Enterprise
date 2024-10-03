using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace DAOControllers
{
    public class DAOEmployee
    {
        SqlConnection objConnection = new SqlConnection(Connection.enterpriseConnection);

        public List<Employee> employeesListing()
        {
            List<Employee> allEmployees = new List<Employee>();

            string jsonStringResponse;
            
            using (objConnection)
            {
                StringBuilder sb = new StringBuilder();
                SqlDataReader reader;
                try
                {
                    sb.AppendLine("SELECT e.idEmployee, e.name, e.age, e.sex, e.workDescription, b.idBranch, b.description[enterpriseName], e.createdDate FROM Employee e");
                    sb.AppendLine("INNER JOIN Branch b ON e.idBranch = b.idBranch;");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), objConnection);
                    cmd.CommandType = CommandType.Text;

                    objConnection.Open();

                    using (reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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

            jsonStringResponse = createJsonResponse(allEmployees);

            return allEmployees;
        }//End listing employees

        public List<Employee> allEmployeeByBranch(int idBranch)
        {
            List<Employee> allEmployees = new List<Employee>();

            string jsonStringResponse;
            
            try
            {
                SqlDataReader reader;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT e.idEmployee, e.name, e.age, e.sex, e.workDescription, e.idBranch, b.description[enterprise]");
                sb.AppendLine("FROM Employee e INNER JOIN Branch b ON e.idBranch = b.idBranch");
                sb.AppendLine("WHERE b.idBranch = " + idBranch);

                SqlCommand cmd = new SqlCommand(sb.ToString(), objConnection);
                cmd.CommandType = CommandType.Text;

                objConnection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
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
                                description = reader["enterprise"].ToString()
                            }
                        });

                    }

                }

            }//End query reading
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                allEmployees = new List<Employee>();
            }

            jsonStringResponse = createJsonResponse(allEmployees);

            return allEmployees;
        }//End listing employees by branch

        public int getMaxId()
        {
            int id = 0;

            using (objConnection)
            {
                try
                {
                    string consult = "SELECT MAX(idEmployee) as idResult FROM Employee";

                    SqlCommand cmd = new SqlCommand(consult, objConnection);
                    cmd.CommandType = CommandType.Text;

                    objConnection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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

        public int createEmployee(Employee objEmployee, out string message)
        {
            int employeeGenerated = 0;
            message = string.Empty;

            string jsonStringResponse;
            
            try
            {
                using (objConnection)
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
                
                    objConnection.Open();
                    cmd.ExecuteNonQuery();
                
                    employeeGenerated = Convert.ToInt32(cmd.Parameters["idResult"].Value);
                    message = cmd.Parameters["message"].Value.ToString();

                }
            }
            catch (Exception ex) 
            {
                employeeGenerated = 0;
                message = ex.ToString();
            }

            jsonStringResponse = createJsonResponse(objEmployee);

            return employeeGenerated;
        }//End create employee

        public bool editEmployee(Employee objEmployee, out string message)
        {
            bool response = false;
            message = string.Empty;

            string jsonStringResponse;

            try
            {
                using (objConnection)
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
                
                    objConnection.Open();
                    cmd.ExecuteNonQuery();

                    response = Convert.ToBoolean(cmd.Parameters["response"].Value);
                    message = cmd.Parameters["message"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                response = false;
                message = ex.Message;
            }

            jsonStringResponse = createJsonResponse(objEmployee);

            return response;

        }//End edit employee

        public bool deleteEmployee(Employee objEmployee, out string message)
        {
            bool response = false;
            message = string .Empty;

            string jsonStringResponse;

            try
            {
                using (objConnection)
                {

                    SqlCommand cmd = new SqlCommand("sp_delete_employee",objConnection);
                    cmd.Parameters.AddWithValue("idEmployee", objEmployee.idEmployee);
                    cmd.Parameters.Add("response", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("message", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    objConnection.Open();
                    cmd.ExecuteNonQuery();

                    response = Convert.ToBoolean(cmd.Parameters["response"].Value);
                    message = cmd.Parameters["message"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                response=false; 
                message = ex.Message;
            }

            jsonStringResponse = createJsonResponse(objEmployee);

            return response;

        }//End delete employee

        public string createJsonResponse(Employee objEmployee)
        {
            string jsonStringResponse;
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };

                jsonStringResponse = JsonSerializer.Serialize(objEmployee, options);
                Console.Write(jsonStringResponse);
            }
            catch (NotSupportedException ex)
            {
                string message = ex.Message.ToString();
                jsonStringResponse = "{}";
            }

            return jsonStringResponse;
        }//End create json string object response

        public string createJsonResponse(List<Employee> allEmployees)
        {
            string jsonStringResponse;
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };

                jsonStringResponse = JsonSerializer.Serialize(allEmployees, options);
                Console.Write(jsonStringResponse);

            }
            catch (NotSupportedException ex)
            {
                string message = ex.Message.ToString();
                jsonStringResponse = "[]";
            }

            return jsonStringResponse;
        }//End create json string list objects response

    }//End DAO employee class
}//End DAOControllers namespace
