using System.Collections.Generic;

namespace Units
{
    ///<summary>
    ///DeleteAllcommand
    ///</summary>
    public class CommandDeleteAll : Command
    {
        private List<DrawObject> cloneList;

        //CreatethiscommandBEFOREapplyingDeleteAllfunction.
        public CommandDeleteAll(GraphicsList graphicsList)
        {
            cloneList = new List<DrawObject>();

            //Makecloneofthewholelist.
            //AddobjectsinreverseorderbecauseGraphicsList.Add
            //inserteveryobjecttothebeginning.
            int n = graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                cloneList.Add(graphicsList[i].Clone());
            }
        }

        public override void Undo(GraphicsList list)
        {
            //Addallobjectsfromclonelisttolist-
            //oppositetoDeleteAll
            foreach (DrawObject o in cloneList)
            {
                list.Add(o);
            }
        }

        public override void Redo(GraphicsList list)
        {
            //Clearlist-makeDeleteAllagain
            list.Clear();
        }
    }
}