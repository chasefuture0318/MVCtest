using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCTest.Models
{
    public class Order
    {
        public Int32 OrderID { get; set; }
        public string CustomerID { get; set; }
    }

    public class DBmanager
    {
        private readonly string ConnStr = @"Data Source=CHASE\SQLEXPRESS;Initial Catalog=NORTHWND;Integrated Security = False;uid = sa; password = 123456;";
        public void InsertOrder(Order order)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"   SET IDENTITY_INSERT Orders On;
                                                        Insert into Orders(OrderID, CustomerID)
                                                        Values(@OrderID, @CustomerID )");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
            sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public Order GetOrder(int OrderID)
        {
            Order order = new Order();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"   Select  *  From Orders Where OrderID = @OrderID");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@OrderID", OrderID));
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    order = new Order
                    {
                        OrderID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        CustomerID = reader.GetString(reader.GetOrdinal("CustomerID")),
                    };
                }
            }
            sqlConnection.Close();
            return order;
        }
        public void UpdateOrder(Order order)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(@"   Update Orders
                                                        Set CustomerID = @CustomerID
                                                        Where OrderID = @OrderID ");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Parameters.Add(new SqlParameter("@OrderID", order.OrderID));
            sqlCommand.Parameters.Add(new SqlParameter("@CustomerID", order.CustomerID));
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}