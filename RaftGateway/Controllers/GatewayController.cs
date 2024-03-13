using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaftElection;

namespace RaftGateway.Controllers;
[Route("apiGateway/[controller]")]
public class GatewayController : Controller
{
    private readonly Gateway _gateway;
    private readonly ILogger<GatewayController> _logger;
    private List<string> _nodeLocations;

    public GatewayController(Gateway gateway, ILogger<GatewayController> logger)
    {
        _gateway = gateway;
        _logger = logger;
        _nodeLocations = new List<string>();
            _nodeLocations.Add(Environment.GetEnvironmentVariable("NODE1_LOCATION"));
            _nodeLocations.Add(Environment.GetEnvironmentVariable("NODE2_LOCATION"));
            _nodeLocations.Add(Environment.GetEnvironmentVariable("NODE3_LOCATION"));

    }

    [HttpGet("EventaulGet")]
    public async Task<Node> GetLeaderEventually()
    {
        var leader = _gateway.EventualGet();
        if (leader != null) { return leader; }
        else 
        { 
            _logger.LogInformation($"Leader couldn't be found.");
            return null; 
        }
    }

    [HttpGet("StrongGet")]
    public async Task<bool> GetLeaderCheckedNow()
    {
        Node leader = _gateway.leaderNode;
        var isCurrentLeader = _gateway.StrongGet(leader);
        if (isCurrentLeader) 
        {
            _logger.LogInformation($"The Node {leader.Name} is still the leader");
            return true; 
        }
        else
        {
            _logger.LogInformation($"The Node {leader.Name} is not the current leader");
            return false;
        }
    }

    [HttpPost]
    public async Task<bool> PostCompareAndSwap(Node follower)
    {
        var leader = _gateway.leaderNode;
        try
        {
            bool swapped = _gateway.CompareAndSwap(follower, leader);
            if (swapped) 
            {
                _logger.LogInformation($"Node {follower.Name} updated its information");
                return true; 
            }
            else
            {
                _logger.LogInformation($"Node {follower.Name} has updated infromation");
                return false;
            }
        }
        catch
        {
            _logger.LogInformation($"A problem occurred when checking node {follower.Name}'s information");
            return false;
        }
    }

}
