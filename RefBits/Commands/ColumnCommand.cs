///Undo-Redocodeiswrittenusing thearticle:
///http://www.codeproject.com/cs/design/commandpatterndemo.asp
//TheCommandPatternandMVCArchitecture
//ByDavidVeeneman.

namespace Units
{
    ///<summary>
    ///Baseclass forcommandsusedforUndo-Redo
    ///</summary>
    internal abstract class ColumnCommand
    {
        //ThisfunctionisusedtomakeUndooperation.
        //Itmakesactionoppositetotheoriginalcommand.
        public abstract void Undo(GraphicsList list);

        //ThiscommandisusedtomakeRedooperation.
        //Itmakesoriginalcommandagain.
        public abstract void Redo(GraphicsList list);

        //Derivedclass eshavememberswhichcontainenoughinformation
        //tomakeUndoandRedooperationsforeveryspecificcommand.
    }
}