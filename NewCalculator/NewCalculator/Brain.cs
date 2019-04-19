using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewCalculator
{
    enum CalcState
    {
        Zero,
        AccumulateDigits,
        Operation,
        OneStep,
        Result
    }

    public delegate void ChangeTextDelegate(string text);

    class Brain
    {
        ChangeTextDelegate changeTextDelegate;
        CalcState calcState = CalcState.Zero;
        string tempNumber = "";
        string resultNumber = "";
        double result;
        string operation = "";

        public Brain(ChangeTextDelegate changeTextDelegate)
        {
            this.changeTextDelegate = changeTextDelegate;
        }

        public void Process(string msg)
        {
            switch (calcState)
            {
                case CalcState.Zero:
                    Zero(msg, false);
                    break;
                case CalcState.AccumulateDigits:
                    AccumulateDigits(msg, false);
                    break;
                case CalcState.Operation:
                    Operation(msg, false);
                    break;
                case CalcState.OneStep:
                    Onestep(msg,false);
                    break;
                case CalcState.Result:
                    Result(msg, false);
                    break;
                default:
                    break;
            }
        }

        void Zero(string msg, bool isInput)
        {
            if (isInput)
            {
                calcState = CalcState.Zero;
            }
            else
            {
                if (Rules.IsNonZeroDigit(msg))
                {
                    AccumulateDigits(msg, true);
                }
            }
        }

        void AccumulateDigits(string msg, bool isInput)
        {
            if (isInput)
            {
                calcState = CalcState.AccumulateDigits;
                tempNumber += msg;
                changeTextDelegate.Invoke(tempNumber);
            }
            else
            {
                if (Rules.IsDigit(msg))
                {
                    AccumulateDigits(msg, true);
                }
                else if (Rules.IsOperation(msg))
                {
                    Operation(msg, true);
                }
                else if (Rules.IsResult(msg))
                {
                    Result(msg, true);
                }
                else if (Rules.IsOneStep(msg))
                {
                    Onestep(msg, true);
                }
            }
        }

        void Operation(string msg, bool isInput)
        {
            if (isInput)
            {
                calcState = CalcState.Operation;

                if (operation.Length != 0)
                {
                    PerformCalculation();
                    changeTextDelegate.Invoke(resultNumber);
                    
                }

                if (resultNumber == "")
                {
                    resultNumber = tempNumber;
                }

                operation = msg;
                tempNumber = "";
            }
            else
            {
                if (Rules.IsDigit(msg))
                {
                    AccumulateDigits(msg, true);
                }
            }
        }
        void Onestep(string msg, bool IsInput)
        {
            if (IsInput)
            {
                calcState = CalcState.OneStep;
                operation = msg;
                MessageBox.Show("do u miss  me?");
                if(operation.Length > 0)
                {
                    MessageBox.Show("and here");
                    Result(msg, true);
                }
            }
            else
            {
                if (Rules.IsResult(msg))
                {
                    Result(msg, true);
                }
            }
        }

        void Result(string msg, bool isInput)
        {
            if (isInput)
            {
                MessageBox.Show("i am here");
                calcState = CalcState.Result;
                if (operation.Length != 0 )
                {
                  PerformCalculation();
                  changeTextDelegate.Invoke(resultNumber);
                }
                operation = "";
                
            }
            else
            {
                if (Rules.IsOperation(msg))
                {
                    Operation(msg, true);
                }
            }
        }
       
        public void MathFunc()
        {
            switch (operation)
            {
                case "x²":
                    result = int.Parse(tempNumber) * int.Parse(tempNumber);
                    break;
                case "√":
                    result = Math.Sqrt(int.Parse(tempNumber));
                    break;
                case "1/x":
                    result = 1 / int.Parse(tempNumber);
                    break;
                case "cos":
                    double RAD = int.Parse(tempNumber) * Math.PI / 180;
                    result = Math.Cos(RAD);
                    break;
            }
        }

        void PerformCalculation()
        {
            if (operation == "+")
            {
                resultNumber = (int.Parse(tempNumber) + int.Parse(resultNumber)).ToString();
            }
            else if (operation == "-")
            {
                resultNumber = (int.Parse(resultNumber) - int.Parse(tempNumber)).ToString();
            }
            else if (operation == "*")
            {
                resultNumber = (int.Parse(tempNumber) * int.Parse(resultNumber)).ToString();
            }
            else if (operation == "/")
            {
                resultNumber = (int.Parse(resultNumber) / int.Parse(tempNumber)).ToString();
                    
            }
            else if (operation == "√")
            {
                double res = int.Parse(tempNumber);
                tempNumber = (Math.Sqrt(res)).ToString();
            }
           
        }
    }
}