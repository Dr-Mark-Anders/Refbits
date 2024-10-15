using System.Collections.Generic;

///Undo-Redocodeiswrittenusing thearticle:
///http://www.codeproject.com/cs/design/commandpatterndemo.asp
//TheCommandPatternandMVCArchitecture
//ByDavidVeeneman.

namespace Units
{
    ///<summary>
    ///class isresponsibleforexecutingUndo-Redooperations
    ///</summary>
    internal class ColumnUndoManager
    {
        #region class Members

        private GraphicsList graphicsList;

        private List<ColumnCommand> historyList;
        private int nextUndo;

        #endregion class Members

        #region Constructor

        public ColumnUndoManager(GraphicsList graphicsList)
        {
            this.graphicsList = graphicsList;

            ClearHistory();
        }

        #endregion Constructor

        #region Properties

        ///<summary>
        ///return  trueifUndooperationisavailable
        ///</summary>
        public bool CanUndo
        {
            get
            {
                //IftheNextUndopoint eris-1,nocommandstoundo
                if (nextUndo < 0 ||
                nextUndo > historyList.Count - 1)//precaution
                {
                    return false;
                }

                return true;
            }
        }

        ///<summary>
        ///return  trueifRedooperationisavailable
        ///</summary>
        public bool CanRedo
        {
            get
            {
                //IftheNextUndopoint erpoint stothelastitem,nocommandstoredo
                if (nextUndo == historyList.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion Properties

        #region public Functions

        ///<summary>
        ///ClearHistory
        ///</summary>
        public void ClearHistory()
        {
            historyList = new List<ColumnCommand>();
            nextUndo = -1;
        }

        ///<summary>
        ///Addnew commandtohistory.
        ///Calledbyclientafterexecutingsomeaction.
        ///</summary>
        ///<paramname="command"></param>
        public void AddCommandToHistory(ColumnCommand command)
        {
            //Purgehistorylist
            this.TrimHistoryList();

            //Addcommandandincrementundocounter
            historyList.Add(command);

            nextUndo++;
        }

        ///<summary>
        ///Undo
        ///</summary>
        public void Undo()
        {
            if (!CanUndo)
            {
                return;
            }

            //GettheCommandobjecttobeundone
            ColumnCommand command = historyList[nextUndo];

            //ExecutetheCommandobject'sundomethod
            command.Undo(graphicsList);

            //Movethepoint eruponeitem
            nextUndo--;
        }

        ///<summary>
        ///Redo
        ///</summary>
        public void Redo()
        {
            if (!CanRedo)
            {
                return;
            }

            //GettheCommandobjecttoredo
            int itemToRedo = nextUndo + 1;
            ColumnCommand command = historyList[itemToRedo];

            //ExecutetheCommandobject
            command.Redo(graphicsList);

            //Movetheundopoint erdownoneitem
            nextUndo++;
        }

        #endregion public Functions

        #region private Functions

        private void TrimHistoryList()
        {
            //Wecanredoanyundonecommanduntilweexecuteanew
            //command.Thenew commandtakesusoffinanew direction,
            //whichmeanswecannolongerredopreviouslyundoneactions.
            //So,wepurgeallundonecommandsfromthehistorylist.*/

            //ExitifnoitemsinHistorylist
            if (historyList.Count == 0)
            {
                return;
            }

            //ExitifNextUndopoint stolastitemonthelist
            if (nextUndo == historyList.Count - 1)
            {
                return;
            }

            //PurgeallitemsbelowtheNextUndopoint er
            for (int i = historyList.Count - 1; i > nextUndo; i--)
            {
                historyList.RemoveAt(i);
            }
        }

        #endregion private Functions
    }
}