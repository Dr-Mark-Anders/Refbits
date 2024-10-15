namespace Units
{
    ///<summary>
    ///AddnewObjectcommand
    ///</summary>
    internal class ColumnCommandAdd : ColumnCommand
    {
        private DrawObject drawObject;

        //CreatethiscommandwithDrawObjectinstanceaddedtothelist
        public ColumnCommandAdd(DrawObject drawObject)
        : base()
        {
            //Keepcopyofaddedobject
            this.drawObject = (DrawObject)drawObject.Clone();
        }

        public override void Undo(GraphicsList list)
        {
            list.DeleteLastAddedObject();
        }

        public override void Redo(GraphicsList list)
        {
            list.UnselectAll();
            list.Add(drawObject);
        }
    }
}