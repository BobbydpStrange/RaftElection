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
    private Dictionary<int, Guid> votedFor;//term, id for who they votedfor
    public int currentTerm;
    public Node(Guid nodeid, string state)
    {
        fileName = $"{nodeid}.log";
        this.state = state ;
        log = new List<Tuple<int, string>>();
        votedFor = new Dictionary<int, Guid>();
        currentTerm = 0 ;
    }
    public void HeartBeatReceived(string message)
    {
        int randomTime = new Random().Next(100, 1000);
        timeInterval += randomTime;
        LogInfo($"{message}");
    }
    private void LogInfo(string message)
    {
        //make and log to a file with the guid as the name.
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine($"{DateTime.Now}:Term {currentTerm}  ,{message}");
        }
    }
    public void Run()
    {
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
                    Leader();
                    break;
            }
    }

    private void Follower()
    {
        LogInfo("Im a Follower");
    }
    private void Candidate() 
    {
        LogInfo("Im now a Candidate lets vote!");
        LogInfo($"Voted for node: {nodeid}");

    }
    private void Leader() 
    { 
        LogInfo("Im now the leader");
        while (true)
        {

        }

    }
    public Boolean Vote(int term, Guid Candidateid)
    {
        HeartBeatReceived("New Election Vote");
        
        if (currentTerm < term)
        {
            currentTerm = term ;
            LogInfo($"Im voting for Candidate {Candidateid}.");
            votedFor.Add(term, Candidateid);
            return true;
        }
        return false;
    }
   
}
