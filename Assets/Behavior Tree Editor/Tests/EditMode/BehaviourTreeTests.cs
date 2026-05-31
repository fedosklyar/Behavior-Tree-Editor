using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BehaviourTreeTests
{
    private BehaviourTree tree;
    private Blackboard blackboard;

    [SetUp]
    public void SetUp()
    {
        tree = ScriptableObject.CreateInstance<BehaviourTree>();
        blackboard = ScriptableObject.CreateInstance<Blackboard>();
        tree.blackboard = blackboard;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(tree);
        Object.DestroyImmediate(blackboard);
    }

    // --- Traverse ---

    [Test]
    public void Traverse_VisitsAllNodes()
    {
        var root = CreateNode(Node.State.Running);
        var child1 = CreateNode(Node.State.Running);
        var child2 = CreateNode(Node.State.Running);

        var composite = ScriptableObject.CreateInstance<SequencerNode>();
        composite.children = new List<Node> { child1, child2 };

        var rootNode = ScriptableObject.CreateInstance<RootNode>();
        rootNode.child = composite;

        var visited = new List<Node>();
        tree.Traverse(rootNode, n => visited.Add(n));

        Assert.AreEqual(4, visited.Count); // root + composite + 2 children
    }

    [Test]
    public void Traverse_NullNode_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => tree.Traverse(null, n => { }));
    }

    // --- GetChildren ---

    [Test]
    public void GetChildren_RootNode_ReturnsSingleChild()
    {
        var child = CreateNode(Node.State.Running);
        var root = ScriptableObject.CreateInstance<RootNode>();
        root.child = child;

        var children = tree.GetChildren(root);

        Assert.AreEqual(1, children.Count);
        Assert.AreEqual(child, children[0]);
    }

    [Test]
    public void GetChildren_RootNode_NoChild_ReturnsEmpty()
    {
        var root = ScriptableObject.CreateInstance<RootNode>();

        var children = tree.GetChildren(root);

        Assert.AreEqual(0, children.Count);
    }

    [Test]
    public void GetChildren_CompositeNode_ReturnsAllChildren()
    {
        var child1 = CreateNode(Node.State.Running);
        var child2 = CreateNode(Node.State.Running);
        var composite = ScriptableObject.CreateInstance<SequencerNode>();
        composite.children = new List<Node> { child1, child2 };

        var children = tree.GetChildren(composite);

        Assert.AreEqual(2, children.Count);
    }

    [Test]
    public void GetChildren_DecoratorNode_ReturnsSingleChild()
    {
        var child = CreateNode(Node.State.Running);
        var decorator = ScriptableObject.CreateInstance<InverterNode>();
        decorator.child = child;

        var children = tree.GetChildren(decorator);

        Assert.AreEqual(1, children.Count);
        Assert.AreEqual(child, children[0]);
    }

     // --- Update ---

    [Test]
    public void Update_WhenRootRunning_ReturnsRunning()
    {
        var root = ScriptableObject.CreateInstance<RootNode>();
        var child = CreateNode(Node.State.Running);
        root.child = child;
        tree.rootNode = root;

        var result = tree.Update();

        Assert.AreEqual(Node.State.Running, result);
    }

    [Test]
    public void Update_WhenRootSucceeds_ReturnsSuccess()
    {
        var root = ScriptableObject.CreateInstance<RootNode>();
        var child = CreateNode(Node.State.Success);
        root.child = child;
        tree.rootNode = root;

        var result = tree.Update();

        Assert.AreEqual(Node.State.Success, result);
    }

    [Test]
    public void Update_WhenTreeAlreadySucceeded_DoesNotUpdateAgain()
    {
        var root = ScriptableObject.CreateInstance<RootNode>();
        var child = CreateNode(Node.State.Success);
        root.child = child;
        tree.rootNode = root;

        tree.Update(); // succeeds, root.state = Success
        tree.Update(); // should not re-enter

        // child's startCount should still be 1 — OnStart not called twice
        Assert.AreEqual(1, (child as TestNode).startCount);
    }

    // --- Clone ---

    [Test]
    public void Clone_ProducesIndependentBlackboard()
    {
        // give the tree a root node so Clone() doesn't throw
        var root = ScriptableObject.CreateInstance<RootNode>();
        var child = CreateNode(Node.State.Running);
        root.child = child;
        tree.rootNode = root;
        tree.nodes = new List<Node> { root, child };

        blackboard.PopulateFromEntries();
        blackboard["key"] = 1;

        var clone = tree.Clone();
        clone.blackboard["key"] = 99;

        Assert.AreEqual(1, tree.blackboard["key"]);
        Object.DestroyImmediate(clone);
    }

    [Test]
    public void Clone_AllNodesHaveClonedBlackboard()
    {
        var root = ScriptableObject.CreateInstance<RootNode>();
        var child = CreateNode(Node.State.Running);
        root.child = child;
        tree.rootNode = root;
        tree.nodes = new List<Node> { root, child };

        var clone = tree.Clone();

        clone.nodes.ForEach(n =>
        {
            Assert.AreEqual(clone.blackboard, n.blackboard);
        });

        Object.DestroyImmediate(clone);
    }

    // --- Helpers ---

    private TestNode CreateNode(Node.State returnState)
    {
        var node = ScriptableObject.CreateInstance<TestNode>();
        node.returnState = returnState;
        return node;
    }
}
