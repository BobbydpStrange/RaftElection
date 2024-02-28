﻿using System;
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
    public bool isTestinng;
    public int setTimer;
    public string Name;
    public bool isHealthy;
    public Node(Guid nodeid, string state, bool testing, int setTimer,string Name, bool isHealthy)
    {
        fileName = $"{nodeid}.log";
        this.Name = Name ;
        this.state = state ;
        this.isTestinng = testing;
        this.isHealthy = isHealthy ;
        log = new List<Tuple<int, string>>();
        votedFor = new Dictionary<int, Guid>();
        currentTerm = 0 ;
        if (!testing) { timeInterval = new Random().Next(500, 1000); }
        else { timeInterval = setTimer; }
        
    }
    public void HeartBeatReceived(string message)
    {
        //Console.WriteLine("updating timespan");
        int randomTime = new Random().Next(100, 1000);
        timeInterval += randomTime;
        LogInfo($"{message}");
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

    private void Follower()
    {
        LogInfo("Im a Follower");
    }
    private void Candidate() 
    {
        LogInfo("Im now a Candidate lets vote!");
        LogInfo($"Voted for node: {nodeid}");

    }
    public void Leader(bool yourLeader) 
    { 
        LogInfo("Im now the leader");
        while (yourLeader)
        {

        }
        //Console.WriteLine("old leader stopped running");
        state = "follower";

    }
    public Boolean Vote(int term, Guid Candidateid)
    {
        if(isHealthy)
        {
            HeartBeatReceived("New Election Vote");

            if (currentTerm < term)
            {
                currentTerm = term;
                LogInfo($"Im voting for Candidate {Candidateid}.");
                votedFor.Add(term, Candidateid);
                return true;
            }
        }
        return false;
    }
   
}
