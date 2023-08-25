using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class CommunityDetection
    {

        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given an UNDIRECTED Graph with relations between different subjects, count the number of different communities 
        /// </summary>
        /// <param name="edges">array of relation-edges in the graph</param>
        /// <returns>number of different communities in the Graph</returns>
        
        private static HashSet<string> verticesOfGraph;
        private static Dictionary<string, string> visitedVertices;
        private static Dictionary<string, List<string>> verticesAndItsAdjacentLists;

        public static int CountCommunities(KeyValuePair<string, string>[] edges)
        {
            verticesOfGraph = new HashSet<string>();
    
            // create set to store a unique vertices of Graph
            GetVerticesFromEdges(edges, ref verticesOfGraph);

            // Initialize (visitedVertices && verticesAndItsAdjacentLists)
            InitializeDictionarys(ref verticesOfGraph);

            // Bulding Graph
            CreateGraphFromEdges(edges);
            
           
            
            // Calculate No. of Communities
            return CalculateNumberOfDifferentCommunities(ref verticesOfGraph);
        }

        private static void DFSVisit(string vertex)
        {
            /////////////  Lecture Psoudo Code  //////////////// 
            /*
                    color[u] = GRAY                     //discovered
                     	for each v in Adj[u]
                           if color[v] = WHITE then 	//new
       	                       DFS-Visit(v)
                      color[u] = BLACK     //explored
 
            */
            visitedVertices[vertex] = "gray";    //discovered
            foreach (string adjacentOfVertex in verticesAndItsAdjacentLists[vertex])
            {
                if (visitedVertices[adjacentOfVertex] == "white")   //new
                {
                    DFSVisit(adjacentOfVertex);
                }
            }
            //visitedVertices[vertex] = "black";   //explored ==> Not important in this problem
        }

        private static void GetVerticesFromEdges(KeyValuePair<string, string>[] edges, ref HashSet<string> verticesOfGraph)
        {
            foreach (KeyValuePair<string, string> edge in edges)
            {
                verticesOfGraph.Add(edge.Key);
                //graph[edge.Key] = new List<string>();
                verticesOfGraph.Add(edge.Value);
                //graph[edge.Value] = new List<string>();
            }
        }

        private static void InitializeDictionarys(ref HashSet<string> verticesOfGraph)
        {
            verticesAndItsAdjacentLists = new Dictionary<string, List<string>>(verticesOfGraph.Count);
            visitedVertices = new Dictionary<string, string>(verticesOfGraph.Count);
            
            /////////////  Lecture Psoudo Code  //////////////// 
            /*
                    for each vertex u in V[G]
                     	color[u] = WHITE
 
            */
            foreach (string vertex in verticesOfGraph)
            {
                // initial adjacencyList
                verticesAndItsAdjacentLists[vertex] = new List<string>();

                // initially all nodes are white
                visitedVertices[vertex] = "white";
            }
        }

        private static void CreateGraphFromEdges(KeyValuePair<string, string>[] edges)
        {
            //  1 ==> 2
            //  1 <== 2
            foreach (KeyValuePair<string, string> edge in edges)
            {
                //Build un directed graph in both directions ==> add adjacent vertix to adjacencyList of vertix And vice versa
                verticesAndItsAdjacentLists[edge.Key].Add(edge.Value);
                //verticesAndItsAdjacentLists[edge.Key] = edge.Value;
                verticesAndItsAdjacentLists[edge.Value].Add(edge.Key);
                //verticesAndItsAdjacentLists[edge.Key] = edge.Key;
            }
        }


        private static int CalculateNumberOfDifferentCommunities(ref HashSet<string> verticesOfGraph)
        {
            int numberOfDifferentCommunities = 0;
            /////////////  Lecture Psoudo Code  //////////////// 
            /*
                    for each vertex u in V[G]
                     	if color[u] = WHITE then 
                     		DFS-Visit(u) 
            */
            foreach (string vertex in verticesOfGraph)
            {
                if (visitedVertices[vertex] == "white") // Not Visited
                {
                    numberOfDifferentCommunities++; // Number of conected componaned = numberOfDifferentCommunities
                    DFSVisit(vertex);
                }
            }
            return numberOfDifferentCommunities;
        }
        #endregion
    }
}
