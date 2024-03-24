using Microsoft.AspNetCore.Mvc;
using RaftElection;

namespace GatewayAPI.Controllers;
[Route("Gatewayapi/[controller]")]
public class GatewayAPIController : Controller
{
    private readonly Gateway _gateway;
    private readonly ILogger<GatewayAPIController> _logger;
    private List<string> _nodeLocations;

    public GatewayAPIController(Gateway gateway, ILogger<GatewayAPIController> logger)
    {
        _gateway = gateway;
        _logger = logger;
        _nodeLocations = Environment.GetEnvironmentVariable("NODES_LOCATION")?.Split(", ")?.ToList();

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
    [HttpGet("RunGateway")]
    public async Task<bool> GetRunGateway()
    {
        return true;
    }
    [HttpPost("SendLog")]
    public async Task<bool> PostSendLog(string logMessage)
    {
        try
        {
            var httpClient = new HttpClient();
            foreach (var nodeLocation in _nodeLocations)
            {
                var requestUrl = $"{nodeLocation}/apiNode/Node/PostAddLog";
                var content = new StringContent(logMessage);
                var response = new httpClient.PostAsync(requestUrl, content);

                if(!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to send log to node");
                }
            }
            return true;
        }catch(Exception ex) { _logger.LogError($"An Error occured");
            return false;
        }
    }
}
