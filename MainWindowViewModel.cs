using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static double? _firstNumber;
        private static double? _secondNumber;
        private static string _firstNumberString;
        private static string _secondNumberString;
        private static bool _isOperationChosen;
        private static string _operationSign;
        private static string _input;

        private static void InitDefaults()
        {
            //_input = "";
            _firstNumber = 0;
            _secondNumber = null;
            _firstNumberString = "";
            _secondNumberString = "";
            _isOperationChosen = false;
            _operationSign = "";
        }

        public MainWindowViewModel()
        {
            InitDefaults();
        }


        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged();
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ClickCommandNumber => new RelayCommand<string>(OnClickCommandNumber, true);
        private void OnClickCommandNumber(string number)
        {
            if (!_isOperationChosen)
            {
                if (number == "sign")
                {
                    _firstNumberString = "-" + _firstNumberString;
                }
                else
                {
                    _firstNumberString += number;
                }
                RefreshInput();
                OnPropertyChanged(nameof(Input));
            }
            else
            {
                if (number == "sign")
                {
                    _secondNumberString = "-" + _secondNumberString;
                }
                else
                {
                    _secondNumberString += number;
                }
                RefreshInput();
                OnPropertyChanged(nameof(Input));
            }
        }

        delegate string Operation();
        Operation _operation;

        public ICommand ClickCommandOperation => new RelayCommand<string>(OnClickCommandOperation, true);
        private void OnClickCommandOperation(string oper)
        {
            switch (oper)
            {
                case "/":
                    _operation = Divide;
                    break;
                case "*":
                    _operation = () => (_firstNumber * _secondNumber).ToString();
                    break;
                case "-":
                    _operation = () => (_firstNumber - _secondNumber).ToString();
                    break;
                case "+":
                    _operation = () => (_firstNumber + _secondNumber).ToString();
                    break;
                case "=":
                    Count();
                    break;
                case "C":
                    InitDefaults();
                    RefreshInput();
                    break;
                case "CE":
                    if (_secondNumberString != "")
                        _secondNumberString = "";
                    else if (_isOperationChosen)
                        _secondNumber = 0;
                    else
                    {
                        _firstNumber = 0;
                        _firstNumberString = "";
                    }
                    RefreshInput();
                    OnPropertyChanged(nameof(Input));
                    break;
                case "delete":
                    if (_secondNumberString == "" && !_isOperationChosen)
                        _firstNumberString = _firstNumberString.Remove(_firstNumberString.Length - 1);
                    else if (_secondNumberString != "")
                        _secondNumberString = _secondNumberString.Remove(_secondNumberString.Length - 1);

                    RefreshInput();
                    break;
            }
            if (oper != "=" && oper != "C" && oper != "CE" && oper != "delete")
            {
                RefreshInput();
                _isOperationChosen = true;
                _operationSign = oper;
            }
            OnPropertyChanged(nameof(Input));
        }

        private string Divide()
        {
            if (_secondNumber == null)
            {
                if (_firstNumber != 0)
                {
                    return (_firstNumber / _firstNumber).ToString();
                }
                else
                    return "Divide by zero exception";
            }
            else
            {
                if (_secondNumber == 0)
                {
                    return "Divide by zero exception";
                }
                else
                    return (_firstNumber / _secondNumber).ToString();
            }
        }
        private void Count()
        {
            _firstNumber = Double.Parse(_firstNumberString);
            if (_secondNumberString != "")
                _secondNumber = Double.Parse(_secondNumberString);
            else
            {
                _secondNumber = _firstNumber;
                _secondNumberString = _firstNumberString;
            }
            RefreshInput();
            _input += " =";
            OnPropertyChanged(nameof(Input));
            _result = _operation();
            OnPropertyChanged(nameof(Result));

            InitDefaults();
        }

        private void RefreshInput()
        {
            string _toInput = "";
            if (_firstNumberString != "")
                _toInput += _firstNumberString;
            if (_operationSign != "")
                _toInput += " " + _operationSign;
            if (_secondNumberString != "")
                _toInput += " " + _secondNumberString;

            _input = _toInput;
        }


        //observable collection
    }
}
