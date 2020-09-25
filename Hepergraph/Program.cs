using System;

internal class Program
{
    public static void Main(string[] args)
    {
        Hypergraph<string> hypergraph = new Hypergraph<string>(7, 6);
        
        hypergraph.Edges[0].Add(hypergraph.Vertices[0]);
        hypergraph.Edges[0].Add(hypergraph.Vertices[1]);
        hypergraph.Edges[0].Add(hypergraph.Vertices[2]);
        
        hypergraph.Edges[1].Add(hypergraph.Vertices[1]);
        hypergraph.Edges[1].Add(hypergraph.Vertices[2]);

        hypergraph.Edges[2].Add(hypergraph.Vertices[2]);
        hypergraph.Edges[2].Add(hypergraph.Vertices[3]);
        hypergraph.Edges[2].Add(hypergraph.Vertices[4]);
        
        hypergraph.Edges[3].Add(hypergraph.Vertices[5]);
        
        hypergraph.Edges[5].Add(hypergraph.Vertices[6]);
        
        Console.WriteLine(hypergraph.ToString());
        Console.WriteLine();
        
        Console.WriteLine("If we'd like to know edges contains vertex we can use GetEdgesWithVertex operation");
        Console.WriteLine("For example, the third vertex contains in edges:");

        foreach (Edge<string> edge in hypergraph.GetEdgesWithVertex(hypergraph.Vertices[2]))
            Console.Write(hypergraph.Edges.IndexOf(edge) + " ");
        
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Now, edge 4 is empty and after Trim operation in should be terminated");
        
        hypergraph.Trim();
        
        Console.WriteLine(hypergraph.ToString());
    }
}
