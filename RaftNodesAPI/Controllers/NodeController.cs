using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaftElection;

namespace RaftNodesAPI.Controllers;
[Route("apiNode/[controller]")]
public class NodeController : Controller
{
    private readonly Node _node;
    public NodeController(Node node)
    {
        _node = node;
    }

    /* Get state
     * post vote
     * get leader
     * get log?
     */
    [HttpGet("GetResponse")]
    public async Task<bool> GetResponse()
    {

        return true;
    }

    [HttpPost("PostResponse")]
    public async Task <bool> PostResponse()
    {
        return true;
    }

}
