using ProtoFiber.Core.Collections;

namespace ProtoFiber.Core.Graph;

public class Graph
{

    private NodeSlotMap _nodes = new();

    private Dictionary<NodeConnection, NodeConnection> Edges = [], BackEdges = [];

}
