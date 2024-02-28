using RaftElection;

namespace RaftElectionTest
{
    public class Tests
    {
       
        [Test]
        public void LeaderGetsElectedIf2of3NodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA",true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeB",true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeC",false);
            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for(int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected= true;
                            break;
                        }

                    }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);
            Assert.AreNotEqual("leader", nodes[1].state);
            Assert.AreNotEqual("leader", nodes[2].state);

            foreach(var thread in allThreads)
            {
                thread.Join();
            }            
        }

        //a leader gets elected if three of five nodes are healthy.
        [Test]
        public void LeaderDoesntGetElectedIf3of5NodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA",true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeD",false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeE",false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
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
        }

        //a leader does not get elected if three of five nodes are unhealthy.
        [Test]
        public void LeaderDoesntGetElectedIf3of5NodesAreNotHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "candidate", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeC",false);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeD", false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeE",false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
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
        }
        //a node continues as leader if al nodes remain healhy.
        [Test]
        public void LeaderDoesntChangeIfNodesAreHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "leader", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeD", true);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeE", true);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
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
        }

        //a node continues as leader even if two of five nodes become unhealthy.
        [Test]
        public void LeaderDoesntChangeIf2or5NodesAreNotHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "leader", true, 0, "nodeA", true);
            Node nodeB = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeB", true);
            Node nodeC = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeC", true);
            Node nodeD = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeD", false);
            Node nodeE = new Node(Guid.NewGuid(), "follower", true, 2000, "nodeE", false);

            nodes.Add(nodeA);
            nodes.Add(nodeB);
            nodes.Add(nodeC);
            nodes.Add(nodeD);
            nodes.Add(nodeE);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
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
        }

        //some other importent scenario that isn' covered by the above(all nodes are dead?
        [Test]
        public void IfOnlyOneNodeItBecomesTheLeaderIfHealthyTest()
        {
            List<Node> nodes = new List<Node>();
            Node nodeA = new Node(Guid.NewGuid(), "follower", true, 0, "nodeA", true);

            nodes.Add(nodeA);
            var allThreads = Program.StartupThreads(nodes);
            bool leaderSelected = false;
            string checkedTh = Program.CheckThreads(allThreads, nodes);
            if (!string.IsNullOrEmpty(checkedTh))
            {
                while (leaderSelected)
                {
                    for (int i = 0; i < allThreads.Count; i++)
                    {
                        nodes = Program.StartVoteing(nodes[i], nodes, Guid.Empty, null, checkedTh);
                        if (nodes[i].state == "leader")
                        {
                            leaderSelected = true;
                            break;
                        }

                    }
                }
            }
            Assert.AreEqual("leader", nodes[0].state);

            foreach (var thread in allThreads)
            {
                thread.Join();
            }
        }

        //a node will call for an election if messages from the leader to that node take too long.
        //avoiding two double voting:
            //node a starts an electrion and gets voes from b,c,aand d and then b,c,and d get reboot.
            //meanwhile node e starts an election in the same term and asks b, c, and d for their vote.
            //node e should not get botes from b,c and d since they already gave their votes before reboot.
    }
}