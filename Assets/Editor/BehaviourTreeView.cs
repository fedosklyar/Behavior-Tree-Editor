using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
    BehaviourTree tree; //current tree for editing

    private WindowSearchProvider searchProvider;

    public BehaviourTreeEditor window;
    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet); //vorsicht! Can be missed

        searchProvider = ScriptableObject.CreateInstance<WindowSearchProvider>();
        searchProvider.graphView = this;

        this.RegisterCallback<MouseDownEvent>(evt =>
        {
            if (evt.button == 1)
            {
                var screenPos = evt.mousePosition + new Vector2(window.position.x, window.position.y);
                ShowSearchWindow(new NodeCreationContext() 
                { 
                    screenMousePosition = screenPos
                });
                evt.StopPropagation();
            }
        });


        //To prevent the contextual menu from appearing
        this.RegisterCallback<ContextualMenuPopulateEvent>(evt =>
        {
            evt.StopPropagation();
            evt.menu.MenuItems().Clear();
        });
        // this.nodeCreationRequest = ShowSearchWindow;

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    //For the reloading of the window after calling Undo operation
    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    private void ShowSearchWindow(NodeCreationContext obj)
    {
        searchProvider.target = (VisualElement)focusController.focusedElement;
        SearchWindow.Open(new SearchWindowContext(obj.screenMousePosition), searchProvider);
    }

    internal void PopulateView(BehaviourTree tree)
    {
        Debug.Log("Within Populate view()");
        this.tree = tree;

        if (tree.rootNode == null)
        {
            Debug.Log("Root node happened to be null");
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;


        //Creates node view
        foreach (Node node in tree.nodes)
        {
            Debug.Log($"The node type: {node.GetType()} and the name: {node.name}");
            CreateNodeView(node);
        }

        //Creates edges
        foreach (Node node in tree.nodes)
        {
            var children = tree.GetChildren(node);
            foreach (Node child in children)
            {
                NodeView parentView = FindNodeView(node);
                NodeView childView = FindNodeView(child);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            }
        }
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //Simplify the LINQ later
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (GraphElement element in graphViewChange.elementsToRemove)
            {
                NodeView nodeView = element as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = element as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            }

        }

        if (graphViewChange.edgesToCreate != null)
        {
            foreach (Edge edge in graphViewChange.edgesToCreate)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);

            }
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }
        return graphViewChange;
    }

    // public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    // {
    //     //base.BuildContextualMenu(evt);
    //     {
    //         var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
    //         foreach (var type in types)
    //         {
    //             evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
    //         }
    //     }


    //     {
    //         var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
    //         foreach (var type in types)
    //         {
    //             evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
    //         }
    //     }

    //     {
    //         var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
    //         foreach (var type in types)
    //         {
    //             evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
    //         }
    //     }

    //     {
    //         var types = TypeCache.GetTypesDerivedFrom<ConditionNode>();
    //         foreach (var type in types)
    //         {
    //             evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
    //         }
    //     }
    // }

    public void CreateNode(System.Type type, Vector2 mousePosition)
    {
        Node node = tree.CreateNode(type);
        node.position = mousePosition;
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        //Node node = tree.CreateNode(type);
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }


    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView nodeView = n as NodeView;
            nodeView.UpdateState();
        });
    }
    
}
