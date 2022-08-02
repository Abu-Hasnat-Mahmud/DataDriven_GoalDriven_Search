using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDriven_GoalDriven_Search
{
    /* input data
     * 
        p^q->goal
        r^s->p
        w^r->q
        t^u->q
        v->s
        start->v^r^q
     *
     */
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.Write("Which algorithm will be execute? 1: Data Driven, 2: GoalDrive - ");
                var algorithm = Console.ReadLine();

                Dictionary<string, string> ProductinSets = new Dictionary<string, string>();
                List<string> workingMemory = new();

                Console.Write("How many productions are set? ");

                int setNumber = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < setNumber; i++)
                {
                    Console.Write($"Set {i + 1}: ");
                    var set = Console.ReadLine().Split("->");
                    ProductinSets.Add(set[0], set[1]);
                }

                if (algorithm == "1")
                {
                    DataDriven(ProductinSets, workingMemory);
                }
                else if (algorithm == "2")
                {
                    GoalDriven(ProductinSets, workingMemory);
                }
                
                Console.WriteLine("\nAgain....\n");               
            }
        }

        static void DataDriven(Dictionary<string, string> ProductinSets, List<string> workingMemory)
        {
            var workingState = ProductinSets.Where(a => a.Key.ToLower() == "start").FirstOrDefault();
            workingMemory.Add(workingState.Key);

            while (workingState.Value.ToLower() != "goal")
            {
                var currentKey = workingState.Key;

                var foundItems = workingState.Value.Split("^");

                workingMemory.AddRange(foundItems);
                workingMemory = workingMemory.Distinct().ToList();

                ProductinSets.Remove(currentKey);

                foreach (var item in ProductinSets)
                {
                    var elements = item.Key.Split("^").ToList();
                    if (elements.Intersect(workingMemory).Count() == elements.Count)
                    {
                        workingState = item;
                        break;
                    }
                }
            }

            workingMemory.Add(workingState.Value);
            Console.WriteLine("Data driven search result: " + String.Join(", ", workingMemory));
        }

        static void GoalDriven(Dictionary<string, string> ProductinSets, List<string> workingMemory)
        {
            var workingState = ProductinSets.Where(a => a.Value.ToLower() == "goal").FirstOrDefault();
            workingMemory.Add(workingState.Value);

            while (workingState.Key.ToLower() != "start")
            {
                var currentKey = workingState.Key;
                var currentValue = workingState.Value;

                var foundItems = workingState.Key.Split("^");

                workingMemory.AddRange(foundItems);
                workingMemory = workingMemory.Distinct().ToList();

                ProductinSets.Remove(currentKey);

                foreach (var item in ProductinSets)
                {
                    var elements = item.Value.Split("^").ToList();
                    if (elements.Intersect(workingMemory).Count() == elements.Count)
                    {
                        workingState = item;
                        break;
                    }
                }
            }

            workingMemory.Add(workingState.Key);
            Console.WriteLine("Goal driven search result: " + String.Join(", ", workingMemory));
        }
    }
}
