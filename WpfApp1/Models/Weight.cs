using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSAPP.Models
{
   public class Weight
    {
        public bool IsInterval { get; set; }
        public float? Expression { get; set; }
        public float? ExpressionSup { get; set; }
        public float? ExpressionInf { get; set; }
        public Operation? Operation { get; set; }
        public OperationSup? OperationSup { get; set; }
        public OperationInf? OperationInf { get; set; }
    }
    public enum Operation
    {
        None = 0,
        Equal = 1,
        Sup = 2,
        SupOrEqual = 3,
        Inf = 4,
        InfOrEqaul = 5
    }
    public enum OperationSup
    {
        None = 0,
        Sup = 2,
        SupOrEqual = 3,
    }
    public enum OperationInf
    {
        None = 0,
        Inf = 4,
        InfOrEqaul = 5
    }
}
