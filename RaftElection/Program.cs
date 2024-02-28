namespace RaftElection;
using System;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Timers;
using System.Xml.Linq;

/*___________List_________
 *      multi threaded simulation of election. 
 * 3 nodes(classes and files)
 *      guid
 *      log info to file that has the guid name(key value version [](index, term #, action)dictionary)
 *          term who they voted for
 * Leader
 *      Sends heart beats to everyone else
 *      If new election happends it is no longer the leader and can vote
 * Canidate
 *      If the leaders election is smaller than new election number then voting can happen
 *      Votes for themselves first 
 *      wins election if majority vote.
 *      consider canidate(nodeid and termnumber)
 * follower
 *      works and once timer or clock is done then becomes a canidate
 *      
 * Replicated logs
 *      if missing entries the leader sends the missing ones with their heartbeat
 */

public class Program
{
    static void Main(string[] args)
    {
        List<Node> allNodes = new List<Node>();
        List<Thread> allThreads = new List<Thread>();
        Guid leaderid= Guid.Empty;
        Node leaderNode = null;

        Node node1 = new Node(Guid.NewGuid(),"follower", false,0, "node1",true);
        Node node2 = new Node(Guid.NewGuid(),"follower",false, 0,"node2", true);
        Node node3 = new Node(Guid.NewGuid(), "follower", false,0, "node3", true);

        allNodes.Add(node1);
        allNodes.Add(node2);
        allNodes.Add(node3);
        
       allThreads = StartupThreads(allNodes);

        System.Timers.Timer heartbeatTimer = new System.Timers.Timer(500);
        heartbeatTimer.Elapsed += (sender, e) => HeartBeat(leaderid,allNodes);
        heartbeatTimer.AutoReset = true;
        heartbeatTimer.Enabled = true;

        while (true)
        {

            string checkedTh = CheckThreads(allThreads, allNodes);

            if (!string.IsNullOrEmpty(checkedTh))
            {
                foreach (var node in allNodes)
                {
                    allNodes = StartVoteing(node, allNodes, leaderid, leaderNode, checkedTh);
                }
            }
        }
    }
    public static List<Node> StartVoteing (Node node, List<Node> allNodes, Guid leaderid, Node leaderNode, string checkedTh)
    {
        if (node.state == "candidate")
        {
            int voteCount = 0;
            Guid candidateId = node.nodeid;
            int term = node.currentTerm + 1;
            var allThreads = new List<Thread>();

            foreach (var voter in allNodes)
            {
                bool voteResult = voter.Vote(term, candidateId);
                voteCount++;

            }
            if (voteCount >= 2)
            {
                //Console.WriteLine("new leader");
                if (leaderNode != null)
                {
                    leaderNode.Leader(false);
                }
                node.state = "leader";
                leaderid = node.nodeid;
                leaderNode = node;
                foreach (var voter in allNodes)
                {
                    if (voter.state != "follower" && voter.Name != node.Name|| voter.nodeid != leaderid)
                        voter.state = "follower";

                    Console.WriteLine($"node: {voter} is now {voter.state}");
                }

                allThreads = StartupThreads(allNodes);
                Console.WriteLine($"Term: {node.currentTerm}");
            }
            if ( checkedTh == $"{node}")
            {
                for(int i = 0; i < allThreads.Count; i++)
                {
                    if (allThreads[i].Name == $"{node}")
                    {
                        allThreads[i] = new Thread(node.Run);
                        allThreads[i].Start();
                    }
                }
            }
            

        }
        return allNodes;
    }
    public static string CheckThreads(List<Thread> threads,List<Node> nodes)
    {
        for (int i = 0; i < threads.Count; i++)
        {
            if (!threads[i].IsAlive && nodes[i].state == "candidate")
            {
                return threads[i].Name;
            }
        }

        return null;
    }
    public static List<Thread> StartupThreads (List<Node> allNodes)
    {
        List<Thread> threads = new List<Thread>();
        foreach(var node in allNodes)
        {
            Thread t = new Thread(node.Run);
            t.Name = $"{node.Name}";
            threads.Add(t);
            t.Start();
        }
        return (threads);
    }
    static void HeartBeat(Guid leaderid,List<Node> allNodes)
    {
        if (leaderid != Guid.Empty)
        {
            foreach (var node in allNodes)
            {
                node.HeartBeatReceived($"heartbeat from leader :{leaderid}");
            }
        }
    }
}