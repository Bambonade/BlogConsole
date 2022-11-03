using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

                // Create and save a new Blog
                Console.WriteLine("1.) Display all blogs");
                Console.WriteLine("2.) Add blog");
                Console.WriteLine("3.) Create post");
                Console.WriteLine("4.) Display posts");
                string input = Console.ReadLine();

                if(input =="1"){
                    try{
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine("All blogs in the database");
                        foreach (var item in query){
                            Console.WriteLine(item.Name);
                        }
                    }
                    catch(Exception ex){
                        logger.Error(ex.Message);
                    }
                }
                if (input == "2"){
                    try{
                        Console.WriteLine("Enter a name for a new Blog: ");
                        String blogName = Console.ReadLine();
                        var blog = new Blog { Name = blogName };
                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {blogName}", blogName);
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);
                    }
                    catch (Exception ex){
                        logger.Error(ex.Message);
                        }
                }
            logger.Info("Program ended");
        }
    }
}