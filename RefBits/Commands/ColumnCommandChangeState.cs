using System.Collections.Generic;

namespace Units
{
    ///<summary>
    ///Changingstateofexistingobjects:
    ///move,resize,changeproperties.
    ///</summary>
    internal class ColumnCommandChangeState : ColumnCommand
    {
        //Selectedobject(s)beforeoperation
        private List<DrawObject> listBefore;

        //Selectedobject(s)afteroperation
        private List<DrawObject> listAfter;

        //CreatethiscommandBEFOREoperation.
        public ColumnCommandChangeState(GraphicsList graphicsList)
        {
            //Keepobjectsstatebeforeoperation.
            FillList(graphicsList, ref listBefore);
        }

        //CallthisfunctionAFTERoperation.
        public void State(GraphicsList graphicsList)
        {
            //Keepobjectsstateafteroperation.
            FillList(graphicsList, ref listAfter);
        }

        public override void Undo(GraphicsList list)
        {
            //Replaceallobjectsint helistwithobjectsfromlistBefore
            ReplaceObjects(list, listBefore);
        }

        public override void Redo(GraphicsList list)
        {
            //Replaceallobjectsint helistwithobjectsfromlistAfter
            ReplaceObjects(list, listAfter);
        }

        //ReplaceobjectsingraphicsListwithobjectsfromlist
        private void ReplaceObjects(GraphicsList graphicsList, List<DrawObject> list)
        {
            for (int i = 0; i < graphicsList.Count; i++)
            {
                DrawObject replacement = null;

                foreach (DrawObject o in list)
                {
                    if (o.Guid == graphicsList[i].Guid)
                    {
                        replacement = o;
                        break;
                    }
                }

                if (replacement != null)
                {
                    graphicsList.Replace(i, replacement);
                }
            }
        }

        //Filllistfromselection
        private void FillList(GraphicsList graphicsList, ref List<DrawObject> listToFill)
        {
            listToFill = new List<DrawObject>();

            foreach (DrawObject o in graphicsList.Selection)
            {
                listToFill.Add((DrawObject)o.Clone());
            }
        }
    }
}