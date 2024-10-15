using System.Collections.Generic;

/// Undo-Redo code is written using   the article:
/// http://www.codeproject.com/cs/design/commandpatterndemo.asp
//  The Command Pattern and MVC Architecture
//  By David Veeneman.

namespace Units
{
    /// <summary>
    /// class  is responsible for executing Undo - Redo operations
    /// </summary>
    internal class UndoManager
    {
        #region class  Members

        private GraphicsList graphicsList;

        private List<Command> historyList;
        private int nextUndo;

        #endregion class  Members

        #region Constructor

        public UndoManager(GraphicsList graphicsList)
        {
            this.graphicsList = graphicsList;

            ClearHistory();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// return   true if Undo operation is available
        /// </summary>
        public bool CanUndo
        {
            get
            {
                // If the NextUndo point er is -1, no commands to undo
                if (nextUndo < 0 ||
                    nextUndo > historyList.Count - 1)   // precaution
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// return   true if Redo operation is available
        /// </summary>
        public bool CanRedo
        {
            get
            {
                // If the NextUndo point er point s to the last item, no commands to redo
                if (nextUndo == historyList.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion Properties

        #region public  Functions

        /// <summary>
        /// Clear History
        /// </summary>
        public void ClearHistory()
        {
            historyList = new List<Command>();
            nextUndo = -1;
        }

        /// <summary>
        /// Add new  command to history.
        /// Called by client after executing some action.
        /// </summary>
        /// <param name="command"></param>
        public void AddCommandToHistory(Command command)
        {
            // Purge history list
            this.TrimHistoryList();

            // Add command and increment undo counter
            historyList.Add(command);

            nextUndo++;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            // Get the Command object to be undone
            Command command = historyList[nextUndo];

            // Execute the Command object's undo method
            command.Undo(graphicsList);

            // Move the point er up one item
            nextUndo--;
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            // Get the Command object to redo
            int itemToRedo = nextUndo + 1;
            Command command = historyList[itemToRedo];

            // Execute the Command object
            command.Redo(graphicsList);

            // Move the undo point er down one item
            nextUndo++;
        }

        #endregion public  Functions

        #region private  Functions

        private void TrimHistoryList()
        {
            // We can redo any undone command until we execute a new
            // command. The new  command takes us off in a new  direction,
            // which means we can no longer redo previously undone actions.
            // So, we purge all undone commands from the history list.*/

            // Exit if no items in History list
            if (historyList.Count == 0)
            {
                return;
            }

            // Exit if NextUndo point s to last item on the list
            if (nextUndo == historyList.Count - 1)
            {
                return;
            }

            // Purge all items below the NextUndo point er
            for (int i = historyList.Count - 1; i > nextUndo; i--)
            {
                historyList.RemoveAt(i);
            }
        }

        #endregion private  Functions
    }
}