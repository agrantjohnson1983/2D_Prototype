// SO_DialogueGraphEditor.cs
// Place this file inside any folder named "Editor" in your project.
// e.g. Assets/Editor/SO_DialogueGraphEditor.cs
//
// Opens via: Window > Dialogue Graph Editor
// Or double-click any SO_Dialogue asset in the Project window.
//
// Matches the data model in sDialogueManager:
//   SO_Dialogue
//     └── DialogueBits[]
//           ├── character         (SO_Character)
//           ├── textDialogue
//           ├── typingLettersPerSec
//           ├── choices[]
//           │     ├── textButtonChoice
//           │     └── nextDialogueBit   (DialogueBits)
//           └── nextDialogueBit[] (DialogueBits) — for linear chaining

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SO_DialogueGraphEditor : EditorWindow
{
    // ── Constants ─────────────────────────────────────────────────────────

    const float NODE_W = 230f;
    const float NODE_H = 110f;
    const float HEADER_H = 26f;
    const float TOOLBAR_H = 22f;
    const float GRID_SMALL = 20f;
    const float GRID_LARGE = 100f;
    const float PORT_R = 6f;     // port circle radius

    static readonly Color COL_BG = new Color(0.13f, 0.13f, 0.14f);
    static readonly Color COL_GRID_MINOR = new Color(0.19f, 0.19f, 0.20f);
    static readonly Color COL_GRID_MAJOR = new Color(0.23f, 0.23f, 0.25f);
    static readonly Color COL_NODE = new Color(0.22f, 0.22f, 0.26f);
    static readonly Color COL_HEADER = new Color(0.16f, 0.38f, 0.58f);
    static readonly Color COL_HEADER_SEL = new Color(0.22f, 0.55f, 0.85f);
    static readonly Color COL_HEADER_END = new Color(0.38f, 0.22f, 0.22f);  // terminal nodes
    static readonly Color COL_BORDER = new Color(0.35f, 0.35f, 0.40f);
    static readonly Color COL_BORDER_SEL = new Color(0.30f, 0.65f, 1.00f);
    static readonly Color COL_WIRE_CHOICE = new Color(0.40f, 0.85f, 0.55f, 0.90f);  // choice → next
    static readonly Color COL_WIRE_LINEAR = new Color(0.70f, 0.70f, 0.85f, 0.70f);  // nextDialogueBit chain
    static readonly Color COL_WIRE_DRAG = new Color(1.00f, 0.85f, 0.20f, 1.00f);  // bright yellow — easy to see
    static readonly Color COL_PORT_OUT = new Color(0.30f, 0.65f, 1.00f);
    static readonly Color COL_PORT_IN = new Color(0.50f, 0.50f, 0.60f);
    static readonly Color COL_TEXT_MAIN = new Color(0.90f, 0.90f, 0.92f);
    static readonly Color COL_TEXT_DIM = new Color(0.55f, 0.55f, 0.60f);
    static readonly Color COL_TEXT_CHOICE = new Color(0.45f, 0.85f, 0.55f);
    static readonly Color COL_TEXT_END = new Color(0.90f, 0.55f, 0.40f);

    // ── State ─────────────────────────────────────────────────────────────

    SO_Dialogue loadedDialogue;     // the SO_Dialogue asset being viewed
    List<BitNode> bitNodes = new List<BitNode>();  // one per DialogueBits entry

    Vector2 pan;
    bool isPanning;
    Vector2 panMouseStart;

    BitNode dragNode;
    Vector2 dragNodeOffset;

    // Wiring state — dragging from an output port
    BitNode wireFrom;
    int wireChoiceIdx;   // -1 = linear nextDialogueBit port, >=0 = choices[i]
    Vector2 wireDragPos;

    BitNode selectedNode;

    // ── Inner class: wraps one DialogueBits with editor position ──────────

    class BitNode
    {
        public SO_Dialogue.DialogueBits bit;
        public Vector2 pos;         // stored in bit.editorPosition
        public int index;       // index in SO_Dialogue.dialogueBits[]
    }

    // ── Menu / Open ───────────────────────────────────────────────────────

    [MenuItem("Window/Dialogue Graph Editor")]
    public static SO_DialogueGraphEditor Open()
    {
        var win = GetWindow<SO_DialogueGraphEditor>("Dialogue Graph");
        win.minSize = new Vector2(700, 450);
        return win;
    }

    [UnityEditor.Callbacks.OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is SO_Dialogue dialogue)
        {
            var win = Open();
            win.LoadDialogue(dialogue);
            return true;
        }
        return false;
    }

    // ── Load ──────────────────────────────────────────────────────────────

    public void LoadDialogue(SO_Dialogue dialogue)
    {
        loadedDialogue = dialogue;
        RebuildNodes();
        Repaint();
    }

    void RebuildNodes()
    {
        bitNodes.Clear();
        if (loadedDialogue == null || loadedDialogue.dialogueBits == null) return;

        for (int i = 0; i < loadedDialogue.dialogueBits.Length; i++)
        {
            var bit = loadedDialogue.dialogueBits[i];
            if (bit == null) continue;

            // Auto-layout new nodes that haven't been positioned yet
            if (bit.editorPosition == Vector2.zero)
                bit.editorPosition = new Vector2(80 + i * (NODE_W + 40), 80);

            bitNodes.Add(new BitNode { bit = bit, pos = bit.editorPosition, index = i });
        }
    }

    // ── OnGUI ─────────────────────────────────────────────────────────────

    void OnGUI()
    {
        // Keep positions synced
        foreach (var bn in bitNodes) bn.pos = bn.bit.editorPosition;

        DrawBackground();
        DrawGrid();
        DrawToolbar();

        DrawConnections();
        DrawNodes();
        DrawDragWire();

        HandleEvents();

        if (GUI.changed) Repaint();
    }

    // ── Toolbar ───────────────────────────────────────────────────────────

    void DrawToolbar()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        // Dialogue picker
        EditorGUI.BeginChangeCheck();
        var picked = (SO_Dialogue)EditorGUILayout.ObjectField(
            loadedDialogue, typeof(SO_Dialogue), false,
            GUILayout.Width(220));
        if (EditorGUI.EndChangeCheck()) LoadDialogue(picked);

        if (GUILayout.Button("Reload", EditorStyles.toolbarButton, GUILayout.Width(60)))
            RebuildNodes();

        GUILayout.Space(8);

        GUI.enabled = loadedDialogue != null;
        if (GUILayout.Button("+ Add Bit", EditorStyles.toolbarButton, GUILayout.Width(70)))
            AddBit();
        GUI.enabled = true;

        GUILayout.FlexibleSpace();

        if (loadedDialogue != null)
            GUILayout.Label($"{loadedDialogue.name}  •  {bitNodes.Count} bits", EditorStyles.miniLabel);
        else
            GUILayout.Label("No dialogue loaded — drag an SO_Dialogue here or open one from the Project window", EditorStyles.miniLabel);

        GUILayout.EndHorizontal();
    }

    // ── Drawing ───────────────────────────────────────────────────────────

    void DrawBackground()
    {
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), COL_BG);
    }

    void DrawGrid()
    {
        Handles.BeginGUI();

        DrawGridLines(GRID_SMALL, COL_GRID_MINOR);
        DrawGridLines(GRID_LARGE, COL_GRID_MAJOR);

        Handles.EndGUI();
    }

    void DrawGridLines(float spacing, Color color)
    {
        Handles.color = color;
        float ox = (pan.x % spacing + spacing) % spacing;
        float oy = (pan.y % spacing + spacing) % spacing + TOOLBAR_H;

        int cols = Mathf.CeilToInt(position.width / spacing) + 1;
        int rows = Mathf.CeilToInt(position.height / spacing) + 1;

        for (int i = 0; i < cols; i++)
            Handles.DrawLine(new Vector3(ox + i * spacing, TOOLBAR_H), new Vector3(ox + i * spacing, position.height));
        for (int i = 0; i < rows; i++)
            Handles.DrawLine(new Vector3(0, oy + i * spacing), new Vector3(position.width, oy + i * spacing));
    }

    void DrawNodes()
    {
        foreach (var bn in bitNodes)
            DrawNode(bn);
    }

    void DrawNode(BitNode bn)
    {
        var rect = NodeRect(bn);
        bool isSel = bn == selectedNode;
        var bit = bn.bit;

        bool isTerminal = IsTerminal(bit);

        // Shadow
        EditorGUI.DrawRect(new Rect(rect.x + 3, rect.y + 3, rect.width, rect.height),
                           new Color(0, 0, 0, 0.45f));

        // Body
        EditorGUI.DrawRect(rect, COL_NODE);

        // Header colour: selected > terminal > default
        Color headerCol = isSel ? COL_HEADER_SEL : isTerminal ? COL_HEADER_END : COL_HEADER;
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, HEADER_H), headerCol);

        // Outline
        Handles.BeginGUI();
        Handles.color = isSel ? COL_BORDER_SEL : COL_BORDER;
        Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Handles.color);
        Handles.EndGUI();

        // ── Header text: character name + bit index ──────────────────────
        var headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white },
            padding = new RectOffset(8, 6, 0, 0)
        };
        string charName = (bit.character != null) ? bit.character.characterName : "(no character)";
        GUI.Label(new Rect(rect.x, rect.y, rect.width - 30, HEADER_H),
                  charName, headerStyle);

        // Bit index badge (top-right)
        var idxStyle = new GUIStyle(EditorStyles.miniLabel)
        {
            alignment = TextAnchor.MiddleRight,
            normal = { textColor = new Color(1, 1, 1, 0.45f) },
            padding = new RectOffset(0, 7, 0, 0)
        };
        GUI.Label(new Rect(rect.x, rect.y, rect.width, HEADER_H),
                  $"#{bn.index}", idxStyle);

        // ── Dialogue text preview ────────────────────────────────────────
        var previewStyle = new GUIStyle(EditorStyles.wordWrappedMiniLabel)
        {
            normal = { textColor = COL_TEXT_MAIN },
            padding = new RectOffset(7, 7, 4, 2)
        };
        string preview = string.IsNullOrEmpty(bit.textDialogue) ? "(no text)" : bit.textDialogue;
        if (preview.Length > 90) preview = preview.Substring(0, 88) + "…";

        float previewH = NODE_H - HEADER_H - 22f;
        GUI.Label(new Rect(rect.x, rect.y + HEADER_H, rect.width, previewH), preview, previewStyle);

        // ── Footer: choice labels / terminal / chain badge ───────────────
        float footerY = rect.yMax - 20f;
        DrawNodeFooter(bn, rect, footerY);

        // ── Port: IN (left center) ───────────────────────────────────────
        DrawPort(new Vector2(rect.x, rect.center.y), COL_PORT_IN, false);

        // ── Port: linear OUT (right center) — only if no choices ────────
        if (bit.choices == null || bit.choices.Length == 0)
            DrawPort(LinearOutPort(bn), COL_PORT_OUT, true);

        // ── Ports: choice OUT (right side, stacked) ──────────────────────
        if (bit.choices != null)
        {
            for (int i = 0; i < bit.choices.Length; i++)
                DrawPort(ChoiceOutPort(bn, i), COL_WIRE_CHOICE, true);
        }
    }

    void DrawNodeFooter(BitNode bn, Rect rect, float footerY)
    {
        var bit = bn.bit;
        var style = new GUIStyle(EditorStyles.miniLabel) { padding = new RectOffset(7, 7, 2, 0) };

        if (IsTerminal(bit))
        {
            style.normal.textColor = COL_TEXT_END;
            GUI.Label(new Rect(rect.x, footerY, rect.width, 18), "▣  end of dialogue", style);
            return;
        }

        if (bit.choices != null && bit.choices.Length > 0)
        {
            style.normal.textColor = COL_TEXT_CHOICE;
            string labels = "";
            for (int i = 0; i < bit.choices.Length; i++)
            {
                string ct = bit.choices[i] != null ? bit.choices[i].textButtonChoice : "?";
                if (ct != null && ct.Length > 14) ct = ct.Substring(0, 12) + "…";
                labels += (i > 0 ? "  |  " : "") + ct;
            }
            GUI.Label(new Rect(rect.x, footerY, rect.width, 18), "▸  " + labels, style);
            return;
        }

        // Linear chain
        style.normal.textColor = COL_TEXT_DIM;
        GUI.Label(new Rect(rect.x, footerY, rect.width, 18), "→  continues", style);
    }

    void DrawPort(Vector2 center, Color color, bool isOut)
    {
        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawSolidDisc(center, Vector3.forward, PORT_R);
        Handles.color = new Color(0.1f, 0.1f, 0.1f, 0.6f);
        Handles.DrawWireDisc(center, Vector3.forward, PORT_R);
        Handles.EndGUI();
    }

    // ── Connection curves ─────────────────────────────────────────────────

    void DrawConnections()
    {
        foreach (var bn in bitNodes)
        {
            var bit = bn.bit;

            // Linear nextDialogueBit[0] chain
            if ((bit.choices == null || bit.choices.Length == 0)
                && bit.nextDialogueBit != null && bit.nextDialogueBit.Length > 0
                && bit.nextDialogueBit[0] != null)
            {
                var target = FindNodeForBit(bit.nextDialogueBit[0]);
                if (target != null)
                    DrawCurve(LinearOutPort(bn), InPort(target), COL_WIRE_LINEAR, null);
            }

            // Choice wires
            if (bit.choices != null)
            {
                for (int i = 0; i < bit.choices.Length; i++)
                {
                    var choice = bit.choices[i];
                    if (choice == null || choice.nextDialogueBit == null) continue;
                    var target = FindNodeForBit(choice.nextDialogueBit);
                    if (target == null) continue;

                    string label = choice.textButtonChoice;
                    if (label != null && label.Length > 18) label = label.Substring(0, 16) + "…";
                    DrawCurve(ChoiceOutPort(bn, i), InPort(target), COL_WIRE_CHOICE, label);
                }
            }
        }
    }

    void DrawCurve(Vector2 start, Vector2 end, Color color, string label)
    {
        Vector2 tan = Vector2.right * Mathf.Max(50f, Mathf.Abs(end.x - start.x) * 0.5f);

        Handles.BeginGUI();
        Handles.DrawBezier(start, end, start + tan, end - tan, color, null, 2f);

        // Arrowhead
        Vector2 dir = ((end - tan * 0.05f) - end).normalized;
        Handles.color = color;
        Handles.DrawSolidArc(end, Vector3.forward, dir, 25f, 4.5f);
        Handles.EndGUI();

        // Wire label
        if (!string.IsNullOrEmpty(label))
        {
            Vector2 mid = BezierPoint(start, end, start + tan, end - tan, 0.5f);
            var labelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = color },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 9
            };
            GUI.Label(new Rect(mid.x - 55, mid.y - 9, 110, 16), label, labelStyle);
        }
    }

    void DrawDragWire()
    {
        if (wireFrom == null) return;
        Vector2 start = wireChoiceIdx < 0 ? LinearOutPort(wireFrom) : ChoiceOutPort(wireFrom, wireChoiceIdx);
        Vector2 end = wireDragPos;
        Vector2 tan = Vector2.right * Mathf.Max(50f, Mathf.Abs(end.x - start.x) * 0.5f);

        Handles.BeginGUI();
        Handles.DrawBezier(start, end, start + tan, end - tan, COL_WIRE_DRAG, null, 1.5f);
        Handles.EndGUI();
    }

    // ── Event Handling ────────────────────────────────────────────────────

    void HandleEvents()
    {
        var e = Event.current;
        Vector2 gMouse = e.mousePosition;  // same space as NodeRect now

        switch (e.type)
        {
            case EventType.MouseDown:
                HandleMouseDown(e, gMouse);
                break;

            case EventType.MouseDrag:
                HandleMouseDrag(e, gMouse);
                break;

            case EventType.MouseUp:
                HandleMouseUp(e, gMouse);
                break;

            case EventType.ContextClick:
                HandleContextClick(gMouse);
                break;

            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete || e.keyCode == KeyCode.Backspace)
                    if (selectedNode != null) DeleteBit(selectedNode);
                break;
        }
    }

    void HandleMouseDown(Event e, Vector2 gMouse)
    {
        // Middle mouse or Alt+left = pan
        if (e.button == 2 || (e.button == 0 && e.alt))
        {
            isPanning = true;
            panMouseStart = e.mousePosition;
            e.Use();
            return;
        }

        if (e.button == 0)
        {
            // Check output ports first (wire dragging)
            foreach (var bn in bitNodes)
            {
                // Linear out port
                if (bn.bit.choices == null || bn.bit.choices.Length == 0)
                {
                    if (Vector2.Distance(gMouse, LinearOutPort(bn)) <= PORT_R + 3f)
                    {
                        wireFrom = bn; wireChoiceIdx = -1;
                        wireDragPos = gMouse;
                        e.Use(); return;
                    }
                }
                // Choice out ports
                if (bn.bit.choices != null)
                {
                    for (int i = 0; i < bn.bit.choices.Length; i++)
                    {
                        if (Vector2.Distance(gMouse, ChoiceOutPort(bn, i)) <= PORT_R + 3f)
                        {
                            wireFrom = bn; wireChoiceIdx = i;
                            wireDragPos = gMouse;
                            e.Use(); return;
                        }
                    }
                }
            }

            // Node body click — select + start drag
            foreach (var bn in bitNodes)
            {
                if (NodeRect(bn).Contains(gMouse))
                {
                    selectedNode = bn;
                    Selection.activeObject = loadedDialogue;
                    dragNode = bn;
                    dragNodeOffset = gMouse - new Vector2(bn.pos.x + pan.x, bn.pos.y + pan.y + TOOLBAR_H);
                    e.Use(); return;
                }
            }

            // Click on empty space — deselect
            selectedNode = null;
        }
    }

    void HandleMouseDrag(Event e, Vector2 gMouse)
    {
        if (isPanning)
        {
            pan += e.delta;
            GUI.changed = true;
            e.Use();
            return;
        }
        if (dragNode != null)
        {
            Undo.RecordObject(loadedDialogue, "Move Dialogue Bit");
            Vector2 worldPos = gMouse - dragNodeOffset - new Vector2(pan.x, pan.y + TOOLBAR_H);
            dragNode.bit.editorPosition = worldPos;
            dragNode.pos = worldPos;
            EditorUtility.SetDirty(loadedDialogue);
            GUI.changed = true;
            e.Use();
            return;
        }
        if (wireFrom != null)
        {
            wireDragPos = gMouse;
            GUI.changed = true;
            e.Use();
        }
    }

    void HandleMouseUp(Event e, Vector2 gMouse)
    {
        isPanning = false;

        if (dragNode != null) { dragNode = null; }

        if (wireFrom != null)
        {
            // Find node under mouse for wire drop
            BitNode dropTarget = null;
            foreach (var bn in bitNodes)
            {
                if (bn != wireFrom && NodeRect(bn).Contains(gMouse))
                { dropTarget = bn; break; }
            }

            if (dropTarget != null)
                ConnectNodes(wireFrom, wireChoiceIdx, dropTarget);

            wireFrom = null;
            GUI.changed = true;
            e.Use();
        }
    }

    void HandleContextClick(Vector2 gMouse)
    {
        // Right-click on a node: delete option + disconnect
        foreach (var bn in bitNodes)
        {
            if (!NodeRect(bn).Contains(gMouse)) continue;

            var menu = new GenericMenu();
            var captured = bn;

            menu.AddItem(new GUIContent("Delete Bit"), false, () => DeleteBit(captured));
            menu.AddSeparator("");

            // Disconnect linear wire
            if (bn.bit.nextDialogueBit != null && bn.bit.nextDialogueBit.Length > 0)
                menu.AddItem(new GUIContent("Disconnect Linear Wire"), false, () =>
                {
                    Undo.RecordObject(loadedDialogue, "Disconnect Wire");
                    bn.bit.nextDialogueBit = new SO_Dialogue.DialogueBits[0];
                    EditorUtility.SetDirty(loadedDialogue);
                    GUI.changed = true;
                });

            // Disconnect choice wires
            if (bn.bit.choices != null)
            {
                for (int i = 0; i < bn.bit.choices.Length; i++)
                {
                    int idx = i;
                    string choiceLabel = bn.bit.choices[i]?.textButtonChoice ?? $"Choice {i}";
                    menu.AddItem(new GUIContent($"Disconnect \"{choiceLabel}\""), false, () =>
                    {
                        Undo.RecordObject(loadedDialogue, "Disconnect Choice Wire");
                        if (captured.bit.choices[idx] != null)
                            captured.bit.choices[idx].nextDialogueBit = null;
                        EditorUtility.SetDirty(loadedDialogue);
                        GUI.changed = true;
                    });
                }
            }

            menu.ShowAsContext();
            return;
        }

        // Right-click on empty canvas: quick-add
        var addMenu = new GenericMenu();
        addMenu.AddItem(new GUIContent("Add Dialogue Bit here"), false, () => AddBitAt(gMouse));
        addMenu.ShowAsContext();
    }

    // ── Connections ───────────────────────────────────────────────────────

    void ConnectNodes(BitNode from, int choiceIdx, BitNode to)
    {
        Undo.RecordObject(loadedDialogue, "Connect Dialogue Bits");

        if (choiceIdx < 0)
        {
            // Linear next bit connection
            from.bit.nextDialogueBit = new SO_Dialogue.DialogueBits[] { to.bit };
        }
        else
        {
            // Choice connection
            if (from.bit.choices != null && choiceIdx < from.bit.choices.Length
                && from.bit.choices[choiceIdx] != null)
            {
                from.bit.choices[choiceIdx].nextDialogueBit = to.bit;
            }
        }

        EditorUtility.SetDirty(loadedDialogue);
        GUI.changed = true;
    }

    // ── Add / Delete ──────────────────────────────────────────────────────

    void AddBit() => AddBitAt(new Vector2(
        position.width * 0.5f - pan.x - NODE_W * 0.5f,
        (position.height - TOOLBAR_H) * 0.5f - pan.y - NODE_H * 0.5f));

    void AddBitAt(Vector2 worldPos)
    {
        Undo.RecordObject(loadedDialogue, "Add Dialogue Bit");

        var newBit = new SO_Dialogue.DialogueBits
        {
            textDialogue = "New dialogue...",
            typingLettersPerSec = 0.04f,
            editorPosition = worldPos
        };

        int oldLen = loadedDialogue.dialogueBits != null ? loadedDialogue.dialogueBits.Length : 0;
        var newArr = new SO_Dialogue.DialogueBits[oldLen + 1];
        if (loadedDialogue.dialogueBits != null)
            loadedDialogue.dialogueBits.CopyTo(newArr, 0);
        newArr[oldLen] = newBit;
        loadedDialogue.dialogueBits = newArr;

        EditorUtility.SetDirty(loadedDialogue);
        RebuildNodes();
    }

    void DeleteBit(BitNode bn)
    {
        Undo.RecordObject(loadedDialogue, "Delete Dialogue Bit");

        // Remove from array
        var list = new List<SO_Dialogue.DialogueBits>(loadedDialogue.dialogueBits);
        list.Remove(bn.bit);
        loadedDialogue.dialogueBits = list.ToArray();

        // Clean up any wires pointing to it
        foreach (var other in bitNodes)
        {
            if (other == bn) continue;
            if (other.bit.nextDialogueBit != null)
            {
                var nl = new List<SO_Dialogue.DialogueBits>(other.bit.nextDialogueBit);
                nl.Remove(bn.bit);
                other.bit.nextDialogueBit = nl.ToArray();
            }
            if (other.bit.choices != null)
                foreach (var c in other.bit.choices)
                    if (c != null && c.nextDialogueBit == bn.bit)
                        c.nextDialogueBit = null;
        }

        if (selectedNode == bn) selectedNode = null;
        EditorUtility.SetDirty(loadedDialogue);
        RebuildNodes();
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    // Node rect is in graph-clip space (pan applied)
    Rect NodeRect(BitNode bn)
        => new Rect(bn.pos.x + pan.x, bn.pos.y + pan.y + TOOLBAR_H, NODE_W, NODE_H);

    // Right-center port (linear out)
    Vector2 LinearOutPort(BitNode bn)
    {
        var r = NodeRect(bn);
        return new Vector2(r.xMax, r.center.y);
    }

    // Left-center port (in)
    Vector2 InPort(BitNode bn)
    {
        var r = NodeRect(bn);
        return new Vector2(r.xMin, r.center.y);
    }

    // Choice output ports — stacked on the right edge
    Vector2 ChoiceOutPort(BitNode bn, int choiceIdx)
    {
        var r = NodeRect(bn);
        int total = bn.bit.choices.Length;
        float spacing = Mathf.Min(18f, (NODE_H - HEADER_H - 8f) / Mathf.Max(1, total));
        float startY = r.y + HEADER_H + 8f + spacing * 0.5f;
        return new Vector2(r.xMax, startY + choiceIdx * spacing);
    }

    bool IsTerminal(SO_Dialogue.DialogueBits bit)
    {
        bool noChoices = bit.choices == null || bit.choices.Length == 0;
        bool noNext = bit.nextDialogueBit == null || bit.nextDialogueBit.Length == 0;
        return noChoices && noNext;
    }

    BitNode FindNodeForBit(SO_Dialogue.DialogueBits bit)
    {
        foreach (var bn in bitNodes)
            if (bn.bit == bit) return bn;
        return null;
    }

    // Cubic bezier point at t
    static Vector2 BezierPoint(Vector2 p0, Vector2 p3, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }
}

// ── Custom Inspector for SO_Dialogue — adds "Open in Graph Editor" button ────

[CustomEditor(typeof(SO_Dialogue))]
public class SO_DialogueInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open in Dialogue Graph Editor", GUILayout.Height(30)))
        {
            var win = SO_DialogueGraphEditor.Open();
            win.LoadDialogue((SO_Dialogue)target);  // LoadDialogue is public — see above
        }

        EditorGUILayout.Space(6);
        DrawDefaultInspector();
    }
}