using System;
using System.Data.SqlClient;
using System.Threading;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateQuery();
        }

        static void CreateQuery()
        {
            Console.WriteLine("\n1. CreateDepartment\n");
            Console.WriteLine("2. GetDepartment\n");
            Console.WriteLine("3. GetAllDepartments\n");
            Console.WriteLine("4. UpdateDepartmentName\n");
            Console.WriteLine("5. UpdateDepartmentManager\n");
            Console.WriteLine("6. DeleteDepartment\n");
            Console.WriteLine("=========================================\n");
            Console.WriteLine("Select option: ");

            int option = int.TryParse(Console.ReadLine(), out option) ? option : 0;
            string query;

            switch (option)
            {
                case 1:
                    Console.WriteLine("Type Department Name:");
                    string DName = Console.ReadLine();
                    Console.WriteLine("Type Manager SSN:");
                    string mgrSSN = Console.ReadLine();
                    query = "DECLARE @count INT;" +
                            "EXEC usp_CreateDepartment @DName = '" + DName + "' ,@MgrSSN = " + mgrSSN +
                            ", @output_id = @count OUTPUT;" +
                            "SELECT @count AS 'Number of products found';";
                    ExecuteQuery(query, 1);
                    break;
                case 2:
                {
                    Console.WriteLine("Type Department ID:");
                    string DNumber = Console.ReadLine();
                    query = "SELECT * FROM usp_GetDepartment (" + DNumber + ");";
                    ExecuteQuery(query, 2);
                    break;
                }
                case 3:
                    query = "SELECT * FROM usp_GetAllDepartments ();";
                    ExecuteQuery(query, 3);
                    break;
                case 4:
                    Console.WriteLine("Type Department ID:");
                    string DNumber1 = Console.ReadLine();
                    Console.WriteLine("Type Department Name:");
                    string newDName = Console.ReadLine();
                    query = "EXEC usp_UpdateDepartmentName @DName = '" + newDName + "' ,@DNumber = " + DNumber1;
                    ExecuteQuery(query, 4);
                    break;
                case 5:
                    Console.WriteLine("Type Department ID:");
                    string DNumber2 = Console.ReadLine();
                    Console.WriteLine("Type New Manager SSN:");
                    string newMgrSSN = Console.ReadLine();
                    query = "EXEC usp_UpdateDepartmentManager @MgrSSN = " + newMgrSSN + " ,@DNumber = " + DNumber2;
                    ExecuteQuery(query, 5);
                    break;
                case 6:
                    Console.WriteLine("Type Department ID:");
                    string DNumber3 = Console.ReadLine();
                    query = "EXEC [usp_DeleteDepartment] @DNumber = " + DNumber3;
                    ExecuteQuery(query, 6);
                    break;
                default:
                {
                    Console.WriteLine("Not a option!");
                    Thread.Sleep(2000);
                    CreateQuery();
                    break;
                }
            }
        }

        static void ExecuteQuery(string query, int caseNumber)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = "localhost";
                builder.UserID = "sa";
                builder.Password = "Qwerty135";
                builder.InitialCatalog = "Company";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            switch (caseNumber)
                            {
                                case 1:
                                    Console.WriteLine("{0}", "ID of newly created department");
                                    Console.WriteLine("=========================================");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("{0}", reader.GetInt32(0));
                                    }

                                    break;
                                case 2: case 3:
                                    Console.WriteLine("{0} {1}", "DName", "TotalEmp");
                                    Console.WriteLine("=========================================");
                                    while (reader.Read())
                                    {
                                        Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetInt32(1));
                                    }

                                    break;
                                case 4: case 5:
                                    Console.WriteLine("Department updated successfully!");
                                    break;
                                case 6:
                                    Console.WriteLine("Department removed successfully!");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            Thread.Sleep(2000);
            CreateQuery();
        }
    }
}