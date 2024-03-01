using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftElection;

public class Gateway
{
    private List<Node> allNodes;
    private Node leaderNode;
    public Gateway(List<Node> allNodes)
    {
        this.allNodes = allNodes;
        leaderNode = null;
    }

    public Node EventualGet( )
    {
        if (leaderNode == null)
        {
            FindLeader();
            if (leaderNode == null) { return null; };
        }
        return leaderNode;
    }
    public void FindLeader()
    {
        foreach(Node node in allNodes)
        {
            if (node.state == "leader")
            {
                leaderNode = node;
                break;
            }
        }
    }

    public bool StrongGet(Node leaderNode)
    {
        if (leaderNode != null) 
        { 
            int majority = (allNodes.Count() + 1) / 2;
            int responseCount = 1;

            foreach (var node in allNodes)
            {
                if (node != leaderNode && node.leaderName == leaderNode.Name)
                {
                    responseCount++;
                    if (responseCount == majority)
                    {
                        return true;
                    }
                }
            } 
        }
        return false;
    }
    public bool CompareAndSwap(Node follower, Node leaderNode)
    {
        if(follower.currentTerm != leaderNode.currentTerm)
        {
            follower.currentTerm = leaderNode.currentTerm;
            follower.leaderName = leaderNode.leaderName;
            follower.votedFor.Add(leaderNode.votedFor.Last().Key, leaderNode.votedFor.Last().Value);
            return true;
        }
        return false;
    }
}
