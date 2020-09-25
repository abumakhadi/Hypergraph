using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Hypergraph<T>
{
    public readonly List<Vertex<T>> Vertices;
    public readonly List<Edge<T>> Edges;

    //Initialize graph from size and order
    public Hypergraph(int size, int order)
    {
        Vertices = new List<Vertex<T>>();
        Edges = new List<Edge<T>>();

        for (int i = 0; i < size; i++)
            Vertices.Add(new Vertex<T>());

        for (int i = 0; i < order; i++)
            Edges.Add(new Edge<T>());
    }

    //Initialize graph from edges
    public Hypergraph(IEnumerable<Edge<T>> edges)
    {
        Vertices = new List<Vertex<T>>();
        Edges = new List<Edge<T>>();

        foreach (Edge<T> edge in edges)
        {
            Edges.Add(edge);

            foreach (Vertex<T> vertex in edge.Vertices)
                Vertices.Add(vertex);
        }
    }


    //Initialize graph from vertices
    public Hypergraph(IEnumerable<Vertex<T>> vertices)
    {
        Vertices = new List<Vertex<T>>();
        Edges = new List<Edge<T>>();

        foreach (Vertex<T> vertex in vertices)
            Vertices.Add(vertex);

    }

    //Copy constrictor
    public Hypergraph(Hypergraph<T> hypergraph)
    {
        Vertices = new List<Vertex<T>>(hypergraph.Vertices);
        Edges = new List<Edge<T>>(hypergraph.Edges);
    }

    //Empty property
    public bool IsEmpty => Edges == null || Edges.Count == 0;

    //d-regular property
    public bool IsDRegular(int d)
    {
        foreach (Vertex<T> vertex in Vertices)
        {
            int edgesCount = 0;

            foreach (Edge<T> edge in Edges)
            {
                if (edge.Contains(vertex))
                    edgesCount++;
            }

            if (edgesCount < d)
                return false;
        }

        return true;
    }

    //k-uniform property
    public bool IsKUniform(int k)
    {
        foreach (Edge<T> edge in Edges)
        {
            if (edge.Length != k)
                return false;
        }

        return true;
    }

    //Check if vertex isolated
    public bool IsIsolatedVertex(Vertex<T> vertex)
    {
        return GetEdgesWithVertex(vertex).Length == 0;
    }

    //Is two vertices adjacent
    public bool IsAdjacent(Vertex<T> v1, Vertex<T> v2)
    {
        Edge<T>[] edges1 = GetEdgesWithVertex(v1);
        Edge<T>[] edges2 = GetEdgesWithVertex(v2);

        return edges1.Intersect(edges2).Any();
    }

    //Visulize graph
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("   ");
        
        for (int i = 0; i < Vertices.Count; i++)
            sb.Append($"v{i}  ");
        
        
        for (int i = 0; i < Edges.Count; i++)
        {
            Edge<T> edge = Edges[i];

            sb.Append($"{Environment.NewLine}e{i} ");

            for (int j = 0; j < Vertices.Count; j++)
            {
                if (j != 0)
                    sb.Append(" - ");
                
                sb.Append(edge.Vertices.Contains(Vertices[j]) ? "1" : "0");
            }
        }
        
        return sb.ToString();
    }

    
    //Terminate empty edges
    public void Trim()
    {
        List<Edge<T>> edgesToRemove = new List<Edge<T>>();
        
        foreach (Edge<T> edge in Edges)
            if(edge.Length == 0)
                edgesToRemove.Add(edge);

        foreach (Edge<T> edge in edgesToRemove)
            Edges.Remove(edge);
    }
    
    //Returns edges contains entire vertex
    public Edge<T>[] GetEdgesWithVertex(Vertex<T> vertex)
    {
        List<Edge<T>> result = new List<Edge<T>>();
        
        foreach (Edge<T> edge in Edges)
        {
            if(edge.Contains(vertex))
                result.Add(edge);
        }

        return result.ToArray();
    }
}

public class Vertex<T>
{
    public T Value;

    public Vertex() { }
    
    public Vertex(T value)
    {
        Value = value;
    }
}

public class Edge<T>
{
    readonly public List<Vertex<T>> Vertices;

    public Edge()
    {
        Vertices = new List<Vertex<T>>();
    }
    
    public Edge(IEnumerable<Vertex<T>> vertices)
    {
        Vertices = vertices.ToList();
    }

    public void Add(Vertex<T> vertex)
    {
        Vertices.Add(vertex);
    }
    
    public int Length => Vertices.Count;

    public bool Contains(Vertex<T> vertex)
    {
        return Vertices.Contains(vertex);
    }
}
