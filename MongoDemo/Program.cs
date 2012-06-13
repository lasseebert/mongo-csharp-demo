using System;
using System.Collections.Generic;
using System.Linq;
using MongoDemo.Models;

namespace MongoDemo
{
    class Program
    {
        static void Main()
        {
            // Ensure index
            BlogPost.EnsureIndex("Title");

            // Delete all existing
            foreach (var blogPostToDelete in BlogPost.AsQueryable.ToList())
            {
                blogPostToDelete.Delete();
                Console.WriteLine("Deleted BlogPost with id: {0}", blogPostToDelete.Id);
            }
            Console.WriteLine();
            Console.ReadKey();

            // Insert BlogPost
            var blogPost = new BlogPost
                {
                    Title = "MongoDB is awesome!",
                    Content = "Lorem ipsum",
                    Comments = new List<Comment>
                        {
                            new Comment
                                {
                                    Content = "Wow!",
                                    CreatedAt = DateTime.UtcNow.AddHours(-1)
                                },
                            new Comment
                                {
                                    Content = "Omg!!",
                                    CreatedAt = DateTime.UtcNow.AddHours(-2)
                                },

                        }
                };
            Console.WriteLine("Ready to save BlogPost...");
            Console.ReadKey();
            blogPost.Save();
            Console.WriteLine("BlogPost saved");
            Console.WriteLine("BlogPost.Id: {0}", blogPost.Id);
            Console.WriteLine();
            Console.ReadKey();

            // Query BlogPost
            Console.WriteLine("Ready to query for BlogPost...");
            Console.ReadKey();
            var loadedBlogPost = BlogPost.AsQueryable.FirstOrDefault(x => x.Title == "MongoDB is awesome!");
            Console.WriteLine("BlogPost loaded");
            Console.WriteLine("BlogPost.Id: {0}", loadedBlogPost.Id);
            Console.WriteLine("BlogPost.Content: {0}", loadedBlogPost.Content);
            Console.WriteLine("BlogPost.CreatedAt (UTC): {0}", loadedBlogPost.CreatedAt);
            Console.WriteLine();
            Console.ReadKey();

            // Insert a bunch of BlogPosts
            const int COUNT = 10000;
            var randomBlogPosts = Enumerable.Range(1, COUNT).Select(x => new BlogPost
                {
                    Title = string.Format("Title{0}", x),
                    Content = string.Format("Content{0}", x),
                    Comments = Enumerable.Range(1, 5).Select(y => new Comment
                        {
                            Content = string.Format("Comment{0}", y),
                            CreatedAt = DateTime.UtcNow
                        }).ToList()

                }).ToList();
            Console.WriteLine("Ready to insert {0} BlogPosts with 5 comments each...", COUNT);
            Console.ReadKey();
            var startTime = DateTime.Now;
            foreach (var randomBlogPost in randomBlogPosts)
                randomBlogPost.Save();
            var endTime = DateTime.Now;
            var ms = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Took {0} ms", ms);
            Console.WriteLine("That's {0} ms pr. BlogPost!", ms / COUNT);
            Console.WriteLine();
            Console.ReadKey();

            // Find all by id
            var ids = randomBlogPosts.Select(x => x.Id).ToList();
            Console.WriteLine("Ready to fetch {0} BlogPosts by id...", COUNT);
            Console.ReadKey();
            startTime = DateTime.Now;
            foreach (var objectId in ids)
                BlogPost.Find(objectId);
            endTime = DateTime.Now;
            ms = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Took {0} ms", ms);
            Console.WriteLine("That's {0} ms pr. BlogPost!", ms / COUNT);
            Console.WriteLine();
            Console.ReadKey();

            // Find all by title
            var titles = randomBlogPosts.Select(x => x.Title).ToList();
            Console.WriteLine("Ready to fetch {0} BlogPosts by title...", COUNT);
            Console.ReadKey();
            startTime = DateTime.Now;
            foreach (var title in titles)
                BlogPost.AsQueryable.FirstOrDefault(x => x.Title == title);
            endTime = DateTime.Now;
            ms = (endTime - startTime).TotalMilliseconds;
            Console.WriteLine("Took {0} ms", ms);
            Console.WriteLine("That's {0} ms pr. BlogPost!", ms / COUNT);
            Console.WriteLine();
            Console.ReadKey();

        }
    }
}
