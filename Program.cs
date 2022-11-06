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

            string input = "";

            do{
            Console.WriteLine("1.) Display all blogs");
            Console.WriteLine("2.) Add blog");
            Console.WriteLine("3.) Create post");
            Console.WriteLine("4.) Display posts");
            Console.WriteLine("Press enter to quit");
            input = Console.ReadLine();

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
                        string blogName = Console.ReadLine();
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
                if(input == "3"){
                    try{
                        Console.Write("Enter the blog you want to post to: ");
                        string blogName = Console.ReadLine();

                        var db = new BloggingContext();
                        int IDEntry = 0;

                        try{
                        var blogChoice = db.Blogs.First(b => b.Name == blogName);
                        Console.WriteLine($"One blog was found with the name of \"{blogChoice.Name}\"");
                        Console.Write("Would you like to post to this blog (Y/N): ");
                        string wouldContinue = Console.ReadLine();
                        if(wouldContinue.ToUpper() == "Y"){
                            IDEntry = blogChoice.BlogId;
                        }
                        }
                        catch{
                            Console.WriteLine($"No blog found with the name of \"{blogName}\"");
                        }
                        try{
                            var finalBlog = db.Blogs.First(b => b.BlogId == IDEntry);
                            int blogID = finalBlog.BlogId;

                            Console.Write("Enter the title of the post: ");
                            string postTitle = Console.ReadLine();

                            Console.Write("Enter the content of the post: ");
                            string postContent = Console.ReadLine();

                            var post = new Post{Title = postTitle, Content = postContent, BlogId = blogID, Blog = finalBlog};

                            db.AddPost(post);
                            logger.Info("Post added - {postTitle} to {blogName}",postTitle,blogName);
                    }
                    catch(Exception ex){
                        logger.Error(ex.Message);
                    }
                    }
                    catch(Exception ex){
                        logger.Error(ex.Message);
                    }
                }
                if(input == "4"){
                    try{
                        var db = new BloggingContext();
                        var post = db.Posts;
                        Console.WriteLine("All posts in the database:");
                        foreach (var item in post){
                            Console.WriteLine($"\t{item.Title} -- {item.Content}");
                        }
                    }
                    catch (Exception ex){
                        logger.Error(ex.Message);
                    }
                }
            }while (input == "1" || input == "2" || input == "3" || input == "4");
            logger.Info("Program ended");
        }
    }
}