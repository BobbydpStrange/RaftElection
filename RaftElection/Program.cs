namespace RaftElection;
using System;
using System.Timers;

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

class Program
{
    static void Main(string[] args)
    {
        List<Node> allNodes = new List<Node>();
        Guid leaderid= Guid.Empty;
        Node leaderNode = null;

        Node node1 = new Node(Guid.NewGuid(),"follower");
        Node node2 = new Node(Guid.NewGuid(),"follower");
        Node node3 = new Node(Guid.NewGuid(), "follower");

        allNodes.Add(node1);
        allNodes.Add(node2);
        allNodes.Add(node3);

        Thread thread1 = new Thread(node1.Run);
        Thread thread2 = new Thread(node2.Run);
        Thread thread3 = new Thread(node3.Run);
        thread1.Name = "node1";
        thread2.Name = "node2";
        thread3.Name = "node3";

        thread1.Start();
        thread2.Start();
        thread3.Start();

        Timer heartbeatTimer = new Timer(500);
        heartbeatTimer.Elapsed += (sender, e) => HeartBeat(leaderid,allNodes);
        heartbeatTimer.AutoReset = true;
        heartbeatTimer.Enabled = true;

        while (true)
        {

            string checkedTh = CheckThreads(thread1, thread2, thread3);

            if (!string.IsNullOrEmpty(checkedTh))
            {
                foreach (var node in allNodes)
                {
                    if (node.state == "candidate")
                    {
                        int voteCount = 0;
                        Guid candidateId = node.nodeid;
                        int term = node.currentTerm + 1;

                        foreach (var voter in allNodes)
                        {
                           bool voteResult = voter.Vote(term, candidateId);
                           voteCount++;
                                  
                        }
                        if (voteCount >= 2)
                        {
                            Console.WriteLine("new leader");
                            if(leaderNode != null)
                            {
                                leaderNode.Leader(false);
                            }
                            node.state = "leader";
                            leaderid = node.nodeid;
                            leaderNode = node;
                            foreach(var voter in allNodes)
                            {
                                if (voter != node && voter.state != "follower" && voter.state != "leader")
                                    voter.state = "follower";
                            }
                        }
                        switch(checkedTh)
                        {
                            case "node1":
                                thread1 = new Thread(node.Run);
                                thread1.Start();
                                break;
                            case "node2":
                                thread2 = new Thread(node.Run);
                                thread2.Start();
                                break;
                            case "node3":
                                thread3 = new Thread(node.Run);
                                thread3.Start();
                                break;
                        }

                    }
                }
            }
        }
    }
   
    static string CheckThreads(params Thread[] threads)
    {
        foreach( var thread in threads)
        {
            if (!thread.IsAlive)
            {
                Console.WriteLine("thread finished");
                return thread.Name;
            }
        }
        return null;
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