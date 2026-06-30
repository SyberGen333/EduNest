using System;
using System.Collections.Generic;

namespace EduNest.Helpers.Nest.UndoRedo
{
    // Simple configuration holder for undo/redo limits.
    internal static class UndoRedoConfig
    {
        // Default maximum history size. Can be adjusted before creating/using manager.
        public static int MaxHistory { get; set; } = 10;
    }

    // Represents a simple undoable action storing file or folder locations.
    public enum UndoActionKind
    {
        Move,
        Copy,
        Rename
    }

    public sealed class UndoAction
    {
        public string? SourcePath { get; }
        public string? DestinationPath { get; }
        public string? Description { get; }
        public UndoActionKind Kind { get; }

        public UndoAction(string? sourcePath, string? destinationPath, UndoActionKind kind, string? description = null)
        {
            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            Kind = kind;
            Description = description;
        }
    }

    // Manager that stores bounded undo and redo stacks for frmNest.
    public sealed class UndoRedoManager
    {
        // Raised whenever undo/redo stacks change (actions added, undone, redone, or cleared)
        public event EventHandler? Changed;

        private readonly LinkedList<UndoAction> _undoList = new LinkedList<UndoAction>();
        private readonly LinkedList<UndoAction> _redoList = new LinkedList<UndoAction>();

        private int MaxHistory => Math.Max(1, UndoRedoConfig.MaxHistory);

        public bool CanUndo => _undoList.Count > 0;
        public bool CanRedo => _redoList.Count > 0;

        public void AddAction(UndoAction action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            // Add to undo list and clear redo stack
            _undoList.AddFirst(action);
            TrimList(_undoList);

            _redoList.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public UndoAction? Undo()
        {
            if (!CanUndo) return null;

            var action = _undoList.First!.Value;
            _undoList.RemoveFirst();

            _redoList.AddFirst(action);
            TrimList(_redoList);

            Changed?.Invoke(this, EventArgs.Empty);
            return action;
        }

        public UndoAction? Redo()
        {
            if (!CanRedo) return null;

            var action = _redoList.First!.Value;
            _redoList.RemoveFirst();

            _undoList.AddFirst(action);
            TrimList(_undoList);

            Changed?.Invoke(this, EventArgs.Empty);
            return action;
        }

        public void Clear()
        {
            _undoList.Clear();
            _redoList.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void TrimList(LinkedList<UndoAction> list)
        {
            while (list.Count > MaxHistory)
            {
                list.RemoveLast();
            }
        }
    }
}
