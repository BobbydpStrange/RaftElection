using System;
namespace RaftElection;

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

        Node node1 = new Node(Guid.NewGuid(),"follower");
        Node node2 = new Node(Guid.NewGuid(),"follower");
        Node node3 = new Node(Guid.NewGuid(), "follower");

        allNodes.Add(node1);
        allNodes.Add(node2);
        allNodes.Add(node3);

        Thread thread1 = new Thread(node1.Run);
        Thread thread2 = new Thread(node2.Run);
        Thread thread3 = new Thread(node3.Run);


        thread1.Start();
        thread2.Start();
        thread3.Start();
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

                        foreach (var follower in allNodes)
                        {
                            if (follower.nodeid != candidateId)
                            {
                                bool voteResult = follower.Vote(term, candidateId);
                                voteCount++;
                            }
                        }
                        if (voteCount >= 2)
                        {
                            node.state = "leader";
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
    }

    static string CheckThreads(params Thread[] threads)
    {
        foreach( var thread in threads)
        {
            if (thread.Join(0))
            {
                return thread.Name;
            }
        }
        return null;
    }
}