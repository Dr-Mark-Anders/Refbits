namespace Units
{
    ///<summary>
    ///AddnewObjectcommand
    ///</summary>
    internal class CommandAdd : Command
    {
        private DrawObject drawObject;

        //CreatethiscommandwithDrawObjectinstanceaddedtothelist
        public CommandAdd(DrawObject drawObject) : base()
        {
            //Keepcopyofaddedobject
            this.drawObject = drawObject.Clone();
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