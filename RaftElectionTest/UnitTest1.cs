using RaftElection;

namespace RaftElectionTest
{
    public class Tests
    {

        [Test]
        public void LeaderGetsElectedIf2of3NodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", false);
            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh, leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);

            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //a leader gets elected if three of five nodes are healthy.
        [Test]
        public void LeaderGetsElectedIf3of5NodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 100, "nodeE", false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh,leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //a leader does not get elected if three of five nodes are unhealthy.
        [Test]
        public void LeaderDoesntGetElectedIf3of5NodesAreNotHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", false);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 100, "nodeE", false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh, leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                    if (!leaderSelected)
                    { leaderSelected = true; /*will keep on checking until true*/ }
                }
            }
            Assert.AreNotEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //a node continues as leader if all nodes remain healhy.
        [Test]
        public void LeaderDoesntChangeIfNodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "leader", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", true);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 100, "nodeE", true);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh, leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                    if (!leaderSelected)
                    { leaderSelected = true; /*will keep on checking until true*/ }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //a node continues as leader even if two of five nodes become unhealthy.
        [Test]
        public void LeaderDoesntChangeIf2of5NodesAreNotHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "leader", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 100, "nodeE", false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh, leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                    if (!leaderSelected)
                    { leaderSelected = true;/*will keep on checking until true*/ }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //some other importent scenario that isn' covered by the above(all nodes are dead?
        [Test]
        public void IfOnlyOneNodeItBecomesTheLeaderIfHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);

            nodes.Add(nodeA);
            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(50);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh,leaderTerm);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                    if (!leaderSelected)
                    { leaderSelected = true; /*will keep on checking until true*/ }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);

            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //a node will call for an election if messages from the leader to that node take too long.
        [Test]
        public void IfLeaderIsntTalkingToNodesAnElectionWillGetHeldTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "leader", true, 0, "nodeA", false);
            Node nodeB = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", true);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 100, "nodeE", true);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);

            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(100);
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (!leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, nodes[0].nodeid, nodes[0], checkedTh, leaderTerm);
                        if (nodes[i].state == "leader" && nodes[i].isHealthy)
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                    if (!leaderSelected)
                    { leaderSelected = true;/*will keep on checking until true*/ }
                }
            }
            Assert.AreNotEqual("leader", nodes[0].state);
            Assert.AreEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }

        //avoiding two double voting:
        //node a starts an electrion and gets votes from b,c,aand d and then b,c,and d get reboot.
        //meanwhile node e starts an election in the same term and asks b, c, and d for their vote.
        //node e should not get votes from b,c and d since they already gave their votes before reboot.
        [Test]
        public void CantHaveTwoLeadersOrDuplicateVotesTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", true);
            Node nodeE = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeE", true);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);

            var allThreads = Program.StartupThreads(nodes);
            int leaderTerm = 0;
            bool leaderSelected = false;
            Thread.Sleep(100);
            for (int t = 0; t < nodes.Count(); t++)
            {
                string checkedTh = Program.CheckThreads(allThreads, nodes);
                if (!string.IsNullOrEmpty(checkedTh))
                {
                    while (!leaderSelected)
                    {
                        for (int i = 0; i < allThreads.Count; i++)
                        {
                            nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh, leaderTerm);
                            if (nodes[i].state == "leader" && nodes[i].isHealthy)
                            {
                                leaderTerm = nodes[i].currentTerm;
                                leaderSelected = true;
                                break;
                            }

                        }
                        if (!leaderSelected)
                        { leaderSelected = true; /*will keep on checking until true*/ }
                    }
                }
                t++;
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);
            Assert.AreNotEqual("leader", nodes[3].state);
            Assert.AreNotEqual("leader", nodes[4].state);
            Assert.AreEqual(nodes[0].currentTerm, nodes[1].currentTerm);
            Assert.AreEqual(nodes[0].currentTerm, nodes[2].currentTerm);
            Assert.AreEqual(nodes[0].currentTerm, nodes[3].currentTerm);
            Assert.AreEqual(nodes[0].currentTerm, nodes[4].currentTerm);


            foreach (var thread in allThreads)
            {
                thread.Join();
            }
            nodes.Clear();
            allThreads.Clear();
        }
    [Test]
    public void CanFindTheLeaderTest()
    {
        List<Node> nodes = new List<Node>();
        Node nodeA = new Node(Guid.NewGuid(), "follower", true, 0, "nodeA", true);
        Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
        Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
        Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", true);
        Node nodeE = new Node(Guid.NewGuid(), "leader", true, 0, "nodeE", true);

        nodes.Add(nodeA);
        nodes.Add(nodeB);
        nodes.Add(nodeC);
        nodes.Add(nodeD);
        nodes.Add(nodeE);
        Gateway gateway = new Gateway(nodes);
        var leaderNode = gateway.EventualGet();
        Assert.AreEqual(leaderNode.state, nodes[4].state);
        Assert.AreNotEqual(leaderNode.state, nodes[0].state);
        Assert.AreNotEqual(leaderNode.state, nodes[1].state);
        Assert.AreNotEqual(leaderNode.state, nodes[2].state);
        Assert.AreNotEqual(leaderNode.state, nodes[3].state);
        nodes.Clear();
    }
    [Test]
    public void CheckIfStillTheLeaderTest()
    {
        List<Node> nodes = new List<Node>();
        Node nodeA = new Node(Guid.NewGuid(), "follower", true, 0, "nodeA", true);
        Node nodeB = new Node(Guid.NewGuid(), "follower", true, 100, "nodeB", true);
        Node nodeC = new Node(Guid.NewGuid(), "follower", true, 100, "nodeC", true);
        Node nodeD = new Node(Guid.NewGuid(), "follower", true, 100, "nodeD", true);
        Node nodeE = new Node(Guid.NewGuid(), "leader", true, 0, "nodeE", true);
            nodeA.leaderName = "nodeE";
            nodeB.leaderName = "nodeC";//not current and uptodate
            nodeC.leaderName = "nodeC";
            nodeD.leaderName = "nodeE";
            nodeE.leaderName = "nodeE";

            nodes.Add(nodeA);
        nodes.Add(nodeB);
        nodes.Add(nodeC);
        nodes.Add(nodeD);
        nodes.Add(nodeE);
        Gateway gateway = new Gateway(nodes);
        var leaderNode = gateway.StrongGet(nodeE);
            Assert.IsTrue(leaderNode);//returns true if the leader node is nodeE
        nodes.Clear();
    }
    }

}
