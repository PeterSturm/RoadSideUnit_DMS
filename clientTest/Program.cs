using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace clientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                while (true)
                {
                    Console.ReadKey();
                    try
                    {
                        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("HttpClient"));
                        var result = await client.GetStringAsync("http://localhost:51467/api/rsu/sturm/test");
                        //if(result.)

                        Console.WriteLine(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }                   
                }
            }
        }


    }
}
