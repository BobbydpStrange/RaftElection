using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaftElection;

namespace RaftNodesAPI.Controllers;
[Route("apiNode/[controller]")]
public class NodeController : Controller
{
    private readonly Node _node;
    private List<string> _nodeLocations;
    public NodeController(Node node)
    {
        _node = node;
        _nodeLocations = Environment.GetEnvironmentVariable("NODES_LOCATION")?.Split(", ")?.ToList();
    }

    [HttpGet("GetLatestLog")]
    public async Task<Tuple<string, int>> GetLatestLog()
    { 
        _node.history.TryGetValue("LogVersion", out Tuple<string, int> result);
        return result;
    }
    [HttpGet("GetNode")]
    public async Task<Node> GetNodeInfo()
    {
        return _node;
    }
    [HttpGet("GetVote")]
    public async Task<bool> GetVote(int term, Guid Candidateid)
    {
        var response = _node.Vote(term, Candidateid);
        return response;
    }
    [HttpPost("PostAddLog")]
    public async Task<bool> PostAddLog(string message)
    {
        _node.LogInfo(message);
        return true;
    }
    [HttpPost("PostElection")]
    public async Task <bool> PostElection()
    {
        try
        {
            if (_node.state =="candidate")
            {
                var httpClient = new HttpClient();
                var voteCount = 0;
                var majority = ((_nodeLocations.Count())/2)+1 ;
                for(var i = 0; i < _nodeLocations.Count(); i++)
                {
                    var response = await httpClient.GetAsync($"{_nodeLocations[i]}/GetVote?term={_node.currentTerm}&CandidateId={_node.nodeid}");
                    if (response.Content.ReadAsStringAsync().Result == "true")
                    {
                        voteCount++;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }catch (Exception ex) { Console.WriteLine(ex.Message);  return false; }
    }

}
