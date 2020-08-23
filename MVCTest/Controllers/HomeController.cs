using MVCTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers
{
    public class HomeController : Controller
    {
        string connString = @"server=CHASE\SQLEXPRESS;uid=sa;pwd=123456;database=NORTHWND;Min Pool Size=50;Pooling=false;Enlist=false";
        SqlConnection conn = new SqlConnection();

        //SearchData
        public ActionResult Index()
        {
            conn.ConnectionString = connString;
            string sql = @" 
Select	OrderID, CustomerID, OrderDate
From	Orders";
            SqlCommand cmd = new SqlCommand(sql, conn);
            List<Order> list = new List<Order>();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    Order order = new Order();
                    int.TryParse(dr["OrderID"].ToString(), out int orderID);
                    order.OrderID = orderID;
                    order.CustomerID = dr["CustomerID"].ToString();
                    list.Add(order);
                }
            }

            if (conn.State != ConnectionState.Closed)
                conn.Close();

            ViewBag.List = list;
            return View();
        }

        /// <summary>
        /// InsertData
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertData()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// InsertData
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertData(Order order)
        {
            DBmanager dBmanager = new DBmanager();
            dBmanager.InsertOrder(order);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// UpdateData
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateData(int OrderID)
        {
            Console.WriteLine(OrderID.ToString());
            DBmanager dBmanager = new DBmanager();
            Order order = dBmanager.GetOrder(OrderID);
            return View(order);
        }

        [HttpPost]
        /// <summary>
        /// UpdateData
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateData(Order order)
        {
            DBmanager dBmanager = new DBmanager();
            dBmanager.UpdateOrder(order);
            return RedirectToAction("Index");
        }
    }
}