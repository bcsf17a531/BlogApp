using BlogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {

        // Return The  Dummy About Page
        public ViewResult About()
        {
            return View("About");
        }

       
        // Return the Very First Page
        [HttpGet]
        public ViewResult Login()
        {
            return View("Index");
        }


        // Gets Data From Login Form Validets it From DB and return accordingly
        [HttpPost]
        public ViewResult Login(Users u)
        {
            

            try
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"select * from users where Email='{u.RegisterEmail}' and Password='{u.RegisterPassword}'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();

               // int r= (int)dr.GetValue(0);
                //String r1 = r.ToString();
                //HttpContext.Session.SetString("refrence",r1);

                if (dr.Read())
                {


                    // Admin Values are HardCodded it should be following
                    if (u.RegisterEmail == "admin@admin.com" && u.RegisterPassword == "admin")
                    {
                        //Get All users from Db
                        UsersRepos.Users();
                        
                        return View("Admin",UsersRepos.users);
                        
                    }
                    else
                    {
                        // Clean the previous List and Get Latest Data From DB into it
                        BlogRepos.Clean();
                        BlogRepos.GetPosts();
                        return View("Front",BlogRepos.blog);
                        //, BlogRepos.blog);
                    }
                }
                else
                {
                    return View("Index");
                }
                // users.Add(u);

            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                return View("Error");
            }


        }


        // Return the Register Page
        [HttpGet]
        public ViewResult Register()
        {
            return View("Register");
        }

        // Get Data from Register Page And Stores it into DB
        [HttpPost]
        public ViewResult Register(Users u)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    string query = $"insert into users (Uname,Email,Password,Image)values('{u.RegisterUsername}','{u.RegisterEmail}','{u.RegisterPassword}','{u.ProfileImage}' )";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int insertedRow = cmd.ExecuteNonQuery();
                    if (insertedRow >= 1)
                    {
                        return View("Index");
                    }
                    else
                    {
                        return View();
                    }
                    // users.Add(u);

                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View("Error");
                }
            }
            else
            {

                return View();
            }

        }

        //Return The Front Page When user clicks on "Home" in NavBar
        public ViewResult Front()
        {
            // Clean the previous List and Get Latest Data From DB into it
            BlogRepos.Clean();
            BlogRepos.GetPosts();

            return View("Front", BlogRepos.blog);
        }

        //Find The Current Logged In user in List By using its ID and return Profile Page
        public ViewResult Profile(int id)
        {
            UsersRepos.users.Clear();
            UsersRepos.Users();
            Users u = UsersRepos.users.Find(u => u.Id == id);

            
            return View("Profile",u);
        }





        //       All The Below Functions Are Related To Admin Panel 




        // To update any User 
        [HttpGet]
        public ViewResult Update(int id)
        {


            UsersRepos.users.Clear();
            UsersRepos.Users();
            Users u = UsersRepos.users.Find(b => b.Id == id);

            return View("UpdateUser", u);
        }

        // Store The Updated User Attributes into DB
        [HttpPost]
        public ViewResult Update(Users bl)
        {
            UsersRepos.users.Clear();
            UsersRepos.Users();
            if (ModelState.IsValid)
            {
                foreach (Users u in UsersRepos.users)
                {
                    if (u.Id == bl.Id)
                    {
                        u.RegisterUsername = bl.RegisterUsername;
                        u.RegisterEmail = bl.RegisterEmail;
                        u.RegisterPassword = bl.RegisterPassword;
                        try
                        {
                            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                            SqlConnection con = new SqlConnection(connectionString);
                            con.Open();
                            string query = $"Update users set Uname='{u.RegisterUsername}', Email='{u.RegisterEmail}',Password='{u.RegisterPassword}' where id={u.Id} ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            int effectedRow = cmd.ExecuteNonQuery();
                            if (effectedRow >= 1)
                            {
                                // Clean the previous List and Get Latest Data From DB into it
                                UsersRepos.users.Clear();
                                UsersRepos.Users();
                                return View("Admin", UsersRepos.users);

                            }
                            else
                            {
                                return View();
                            }
                            // users.Add(u);

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = ex.Message;
                            return View("Error");
                        }

                    }
                }

            }

            return View();
        }


        // To Remove any User From DB By Finding Its ID in User List
        public ViewResult Remove(int id)
        {

            // Clean the previous List and Get Latest Data From DB into it
            UsersRepos.users.Clear();
            UsersRepos.Users();
            Users u = UsersRepos.users.Find(b => b.Id == id);

            try
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"delete from users where id={u.Id} ";
                SqlCommand cmd = new SqlCommand(query, con);
                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow >= 1)
                {
                    // Clean the previous List and Get Latest Data From DB into it
                    UsersRepos.users.Clear();
                    UsersRepos.Users();
                    return View("Admin", UsersRepos.users);

                }
                else
                {
                    return View();
                }
                // users.Add(u);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }


        }





        //It returns the Front Page of Posts Only for Admin 
        public ViewResult UpdatPosts()
        {
            BlogRepos.Clean();
            BlogRepos.GetPosts();
            return View("UpdatePosts", BlogRepos.blog);
        }


        //TO View Any Post Details
        public ViewResult Detail(int id)
        {


            BlogRepos.Clean();
            BlogRepos.GetPosts();
            Blog b = BlogRepos.blog.Find(b => b.Id == id);

            return View("PostDetail", b);
        }


        //IT return the Update Page For Post
        [HttpGet]
        public ViewResult PostUpdate(int id)
        {


            BlogRepos.Clean();
            BlogRepos.GetPosts();
            Blog b = BlogRepos.blog.Find(b => b.Id == id);

            return View("PostUpdate", b);
        }


        //Stores the Updated Attributes into DB
        [HttpPost]
        public ViewResult PostUpdate(Blog bl)
        {
            BlogRepos.Clean();
            BlogRepos.GetPosts();
            if (ModelState.IsValid)
            {
                foreach (Blog b in BlogRepos.blog)
                {
                    if (b.Id == bl.Id)
                    {
                        b.Title = bl.Title;
                        b.Context = bl.Context;
                        try
                        {
                            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Blog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                            SqlConnection con = new SqlConnection(connectionString);
                            con.Open();
                            string query = $"Update post set Title='{b.Title}', Context='{b.Context}' where id={b.Id} ";
                            SqlCommand cmd = new SqlCommand(query, con);
                            int effectedRow = cmd.ExecuteNonQuery();
                            if (effectedRow >= 1)
                            {
                                // Clean the previous List and Get Latest Data From DB into it
                                BlogRepos.Clean();
                                BlogRepos.GetPosts();
                                return View("UpdatePosts", BlogRepos.blog);

                            }
                            else
                            {
                                return View();
                            }
                            // users.Add(u);

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Error = ex.Message;
                            return View("Error");
                        }

                    }
                }

            }

            return View();
        }

        //TO delete any post from DB using its ID in List
        public ViewResult PostRemove(int id)
        {

            // Clean the previous List and Get Latest Data From DB into it
            BlogRepos.Clean();
            BlogRepos.GetPosts();
            Blog b = BlogRepos.blog.Find(b => b.Id == id);

            try
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Blog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = $"delete from post where id={b.Id} ";
                SqlCommand cmd = new SqlCommand(query, con);
                int effectedRow = cmd.ExecuteNonQuery();
                if (effectedRow >= 1)
                {
                    // Clean the previous List and Get Latest Data From DB into it
                    BlogRepos.Clean();
                    BlogRepos.GetPosts();
                    return View("UpdatePosts", BlogRepos.blog);

                }
                else
                {
                    return View();
                }
                // users.Add(u);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }


        }

        // To Post any Blog Through Admin Side
        [HttpGet]
        public ViewResult CreatePost()
        {
            return View("CreatePost");
        }


        //Stroes The Post Created By Admin into DB
        [HttpPost]
        public ViewResult CreatePost(Blog b)
        {
            // String refrence=HttpContext.Session.GetString("refrence");
            //int refr = Int32.Parse(refrence);

            if (ModelState.IsValid)
            {
                try
                {
                    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Blog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    string query = $"insert into post(Title, Context)values('{b.Title}', '{b.Context}')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int insertedRow = cmd.ExecuteNonQuery();
                    if (insertedRow >= 1)
                    {
                        // Clean the previous List and Get Latest Data From DB into it
                        BlogRepos.Clean();
                        BlogRepos.GetPosts();
                        return View("UpdatePosts", BlogRepos.blog);

                    }
                    else
                    {
                        return View("Error");
                    }
                    // users.Add(u);

                }
                catch (Exception ex)
                {
                    return View("Error");
                }
            }
            else
            {
                return View();
            }

        }

    }
}
