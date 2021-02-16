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
    public class BlogController : Controller
    {

        //It return the Page where you wrote your Post
        [HttpGet]
        public ViewResult Post()
        {
            return View("Post");
        }



        //It validated your post and store it into database 
        [HttpPost]
        public ViewResult Post(Blog b)
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
                        //Return Front Page 
                        return View("Front", BlogRepos.blog);

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


        // Show the post Details By finding its ID it the Blog Type List 
        public ViewResult Detail(int id)
        {

            // Clean the previous List and Get Latest Data From DB into it
            BlogRepos.Clean();
            BlogRepos.GetPosts();
            Blog b = BlogRepos.blog.Find(b => b.Id == id);

            return View("Detail", b);
        }


           // Return the Update Page where attributes are filled according to its ID
        [HttpGet]
        public ViewResult Update(int id)
        {

            // Clean the previous List and Get Latest Data From DB into it
            BlogRepos.Clean();
            BlogRepos.GetPosts();
            Blog b = BlogRepos.blog.Find(b => b.Id == id);

            return View("Update", b);
        }


            // Get Updated Post Data from Page and Stores it into DB
        [HttpPost]
        public ViewResult Update(Blog bl)
        {
            // Clean the previous List and Get Latest Data From DB into it
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
                                return View("Front", BlogRepos.blog);

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

            // Delete the Post from DB By finding it through its ID in List
        public ViewResult Remove(int id)
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
                    return View("Front", BlogRepos.blog);

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
         
    
