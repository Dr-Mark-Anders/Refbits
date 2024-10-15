namespace SimpleExpressionEngine
{
    // NodeBinary for binary operations such as Add, Subtract etc...
    internal class NodeBinary : Node
    {
        // Constructor accepts the two nodes to be operated on and function
        // that performs the actual operation
        public NodeBinary(Node lhs, Node rhs, Func<double, double, double> op)
        {
            _lhs = lhs;
            _rhs = rhs;
            _op = op;
        }

        private Node _lhs;                              // Left hand side of the operation
        private Node _rhs;                              // Right hand side of the operation
        private Func<double, double, double> _op;       // The callback operator

        public override double Eval(IContext ctx)
        {
            // if (ctx is null)
            //     return   double.NaN;
            // Evaluate both sides
            var lhsVal = _lhs.Eval(ctx);
            var rhsVal = _rhs.Eval(ctx);

            // Evaluate and return
            var result = _op(lhsVal, rhsVal);
            return result;
        }
    }
}