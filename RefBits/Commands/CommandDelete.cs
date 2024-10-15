using System.Collections.Generic;

namespace Units
{
    ///<summary>
    ///Deletecommand
    ///</summary>
    public class CommandDelete : Command
    {
        private List<DrawObject> cloneList;//containsselecteditemswhicharedeleted

        //CreatethiscommandBEFOREapplyingDeleteAllfunction.
        public CommandDelete(GraphicsList graphicsList)
        {
            cloneList = new List<DrawObject>();

            //Makecloneofthelistselection.

            foreach (DrawObject o in graphicsList.Selection)
            {
                cloneList.Add(o.Clone());
            }
        }

        public override void Undo(GraphicsList list)
        {
            list.UnselectAll();

            //AddallobjectsfromcloneListtolist.
            foreach (DrawObject o in cloneList)
            {
                list.Add(o);
            }
        }

        public override void Redo(GraphicsList list)
        {
            //DeletefromlistallobjectskeptincloneList

            int n = list.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                bool toDelete = false;
                IDrawObject objectToDelete = list[i];

                foreach (DrawObject o in cloneList)
                {
                    if (objectToDelete.Guid == o.Guid)
                    {
                        toDelete = true;
                        break;
                    }
                }

                if (toDelete)
                {
                    list.RemoveAt(i);
                }
            }
        }
    }
}