namespace SimpleExpressionEngine
{
    // NodeNumber represents a literal number in the expression
    internal class NodeNumber : Node
    {
        public NodeNumber(double number)
        {
            _number = number;
        }

        private double _number;             // The number

        public override double Eval(IContext ctx)
        {
            // Just return   it.  Too easy.
            return _number;
        }
    }
}