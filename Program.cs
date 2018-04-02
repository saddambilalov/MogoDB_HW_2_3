using System.Linq;
using MongoDB.Driver;
using MongoDB.Models;

namespace MongoDB
{
    internal class Program
    {
        private static void Main()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("students");
            var collection = db.GetCollection<Grades>("grades");

            var elementsToRemove = collection.Find(x => x.Type.Equals("homework"))
                .ToList()
                .GroupBy(x =>  x.StudentId)
                .Select(grp => new
                {
                    grp.Key,
                    Result = grp.OrderBy(x => x.Score).FirstOrDefault()
                }).ToList();

            foreach (var element in elementsToRemove)
            {
                collection.DeleteOne(x => x.Id == element.Result.Id);
            }
        }
    }
}
