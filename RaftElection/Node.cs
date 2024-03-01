using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaftElection;
public class Node
{
    public Guid nodeid;
    private string fileName;
    private int timeInterval;
    public string state;
    private List<Tuple<int, string>> log;//term, action
    public Dictionary<int, Guid> votedFor;//term, id for who they votedfor
    private Dictionary<string, Tuple<int, int>> history;// string -> term# ,log index
    public int currentTerm;
    public bool isTestinng;
    public int setTimer;
    public string Name;
    public bool isHealthy;
    public string leaderName;
    public Node(Guid nodeid, string state, bool testing, int setTimer, string Name, bool isHealthy)
    {
        fileName = $"{nodeid}.log";
        this.Name = Name;
        this.state = state;
        this.isTestinng = testing;
        this.isHealthy = isHealthy;
        log = new List<Tuple<int, string>>();
        votedFor = new Dictionary<int, Guid>();
        history = new Dictionary<string, Tuple<int, int>>() {
            { "CurrentTerm", new Tuple<int,int>(0, 0) }};
        currentTerm = 0 ;
        if (!testing) { timeInterval = new Random().Next(500, 1000); }
        else { timeInterval = setTimer; }
        leaderName = null;
        
    }
    public void HeartBeatReceived(string message)
    {
        if(isHealthy)
        {
            //Console.WriteLine("updating timespan");
            int randomTime = new Random().Next(100, 1000);
            timeInterval += randomTime;
            LogInfo($"{message}");
        }
    }
    private void LogInfo(string message)
    {
        //Console.WriteLine($"logging {message}");
        //make and log to a file with the guid as the name.
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine($"{DateTime.Now}:Term {currentTerm}  ,{message}");
        }
    }
    public void Run()
    {
        if (isHealthy && !isTestinng) { 
            switch(state)
            {
                case "follower":
                    Follower();
                    Thread.Sleep(timeInterval);
                    state = "candidate";
                    break;
                case "candidate":
                    Thread.Sleep(timeInterval);
                    Candidate();
                    break;
                case "leader":
                    //Console.WriteLine("there is a new leader said the node");
                    Leader(true);
                    break;
            }
        }
    }

    private void Follower()
    {
        LogInfo("Im a Follower");
    }
    public void Candidate()
    {
        LogInfo("Im now a Candidate lets vote!");
        LogInfo($"Voted for node: {nodeid}");
        currentTerm = currentTerm +1;
        if (history.TryGetValue("CurrentTerm", out Tuple<int, int> termIndexTuple))
        {
            votedFor.Add(currentTerm, nodeid);

            int historyCurrentTerm = termIndexTuple.Item1;
            int historyLogIndex = termIndexTuple.Item2;
            int index = votedFor.Count();
            history["CurrentTerm"] = new Tuple<int, int>(currentTerm, index);
        }
    }
    public void Leader(bool yourLeader) 
    { 
        LogInfo("Im now the leader");
        if (!isTestinng)
        {
            while (yourLeader)
            {

            }
            //Console.WriteLine("old leader stopped running");
            state = "follower";
        }

    }
    public Boolean Vote(int term, Guid Candidateid)
    {
        if(isHealthy && state == "follower")
        {
            HeartBeatReceived("New Election Vote");
            if (history.TryGetValue("CurrentTerm", out Tuple<int,int> termIndexTuple))
            {
                int historyCurrentTerm = termIndexTuple.Item1;
                int historyLogIndex = termIndexTuple.Item2;

                if (historyCurrentTerm < term && historyCurrentTerm != term)
                {
                    currentTerm = term;
                    LogInfo($"Im voting for Candidate {Candidateid}.");
                    votedFor.Add(term, Candidateid);

                    int index =votedFor.Count();
                    history["CurrentTerm"] = new Tuple<int, int>(term, index);
                    return true;
                }
                else { return false; }
            }
        }
        return false;
    }
   
}
