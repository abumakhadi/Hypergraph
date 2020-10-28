using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Hypergraph<T>
{
    public readonly List<Vertex<T>> Vertices;
    public readonly List<Edge<T>> Edges;

    //Is multi
    public bool IsMultiHyperGraph => GetEvenEdges().Length > 0;

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

    //Is two edges even
    public bool IsEven(Edge<T> e1, Edge<T> e2)
    {
        return e1.Vertices.Intersect(e2.Vertices).Count() == e1.Length;
    }

    //Is another graph subgraph
    public bool IsSubGraph(Hypergraph<T> hypergraph)
    {
        int edgesInCommon = hypergraph.Edges.Intersect(hypergraph.Edges).Count();

        if (edgesInCommon >= Edges.Count && edgesInCommon <= 0)
            return false;
        
        int verticesInCommon = hypergraph.Vertices.Intersect(hypergraph.Vertices).Count();

        if (verticesInCommon >= Vertices.Count && verticesInCommon <= 0)
            return false;

        return true;
    }

    //Is graph l-divided
    public bool IsLDivided(int l)
    {
        HashSet<Vertex<T>> verticesInEdges = new HashSet<Vertex<T>>();

        if (l != Edges.Count)
            return false;
        
        foreach (var edge in Edges)
        {
            foreach (var vertex in edge.Vertices)
            {
                if (verticesInEdges.Contains(vertex))
                    return false;
                verticesInEdges.Add(vertex);
            }
        }

        return true;
    }

    //Is graph L Homogeneous
    public bool IsLHomogeneous(int l)
    {
        if (Edges.Count != l)
            return false;

        foreach (var edge in Edges)
        {
            foreach (var edge_other in Edges)
            {
                if(edge == edge_other)
                    continue;
                
                if (IsEven(edge, edge_other))
                    return false;
            }
        }

        return true;
    }

    public bool IsCombination(IEnumerable<Edge<T>> edges)
    {
        var edgesArray = edges as Edge<T>[] ?? edges.ToArray();
        
        foreach (var edge_1 in edgesArray)
        {
            foreach (var edge_2 in edgesArray)
            {
                if(edge_1 == edge_2)
                    continue;

                if (edge_1.Vertices.Intersect(edge_2.Vertices).Any())
                    return false;
            }
        }

        return true;
    }

    public bool IsPerfectCombination(IEnumerable<Edge<T>> edges)
    {
        IEnumerable<Edge<T>> enumerableEdges = edges as Edge<T>[] ?? edges.ToArray();
        
        if (!IsCombination(enumerableEdges))
            return false;
     
        HashSet<Vertex<T>> vertices = new HashSet<Vertex<T>>();


        foreach (var edge in enumerableEdges)
        {
            foreach (var vertex in edge.Vertices)
            {
                if (vertices.Contains(vertex))
                    return false;

                vertices.Add(vertex);
            }
        }

        return true;
    }
    
    public bool IsLDividedHomogeneous(int l)
    {
        return IsLDivided(l) && IsLHomogeneous(l);
    }

    public bool IsStar(List<Edge<T>> edges, out int power)
    {
        power = -1;
        
        int count = 0;
        HashSet<Vertex<T>> vertices = new HashSet<Vertex<T>>();

        foreach (var edge in edges)
        {
            foreach (var vertex in edge.Vertices)
            {
                if (vertices.Contains(vertex))
                    count++;

                vertices.Add(vertex);
            }
        }

        if (count > 1)
            return false;

        power = count;

        return true;
    }


    public bool IsStarsCovered(List<List<Edge<T>>> edgesList)
    {
        List<Vertex<T>> vertices = new List<Vertex<T>>(Vertices); 
        
        foreach (var edges in edgesList)
        {
            if (!IsStar(edges, out _))
                return false;

            foreach (var edge in edges)
            {
                foreach (var vertex in edge.Vertices)
                {
                    if (vertices.Contains(vertex))
                        vertices.Remove(vertex);
                }
            }
        }

        return !vertices.Any();
    }

    //Get all event edges
    public Edge<T>[] GetEvenEdges(){

        List<Edge<T>> result = new List<Edge<T>>();
            
        for (int i = 0; i < Edges.Count; i++)
        {
            for (int j = 0; j < Edges.Count; j++)
            {
                if (i == j)
                    continue;

                if (IsEven(Edges[i], Edges[j]))
                {
                    result.Add(Edges[i]);
                    result.Add(Edges[j]);
                }
            }
        }

        return result.ToArray();
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
