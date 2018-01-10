using System.Diagnostics;
using Gcc.FlashPoint.Infrastructure;
using Prism.Commands;

namespace Gcc.FlashPoint
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            //Initialize Commands
            Operation = new DelegateCommand(ExecuteOperation, CanExecuteOperation);
        }
        private string _queryStringJsonArray;
        private string _urlSegmentJsonArray;
        private string _jsonBodiesForPost;
        private string _baseUrl;
        private int _selectedTabIndex;
        
        //Props
        public string QueryStringJsonArray
        {
            get => _queryStringJsonArray;
            set
            {
                if (_queryStringJsonArray != value)
                {
                    _queryStringJsonArray = value;
                    OnPropertyChanged(nameof(QueryStringJsonArray));
                    Operation.RaiseCanExecuteChanged();
                }
            }
        }
        public string UrlSegmentJsonArray
        {
            get => _urlSegmentJsonArray;
            set
            {
                if (_urlSegmentJsonArray != value)
                {
                    _urlSegmentJsonArray = value;
                    OnPropertyChanged(nameof(UrlSegmentJsonArray));
                    Operation.RaiseCanExecuteChanged();
                }
            }
        }
        public OperationMode OperationMode => _selectedTabIndex == 0 ? OperationMode.GetMode : OperationMode.PostMode;
        public string BaseUrl
        {
            get => _baseUrl;
            set
            {
                if (_baseUrl != value)
                {
                    _baseUrl = value;
                    OnPropertyChanged(nameof(BaseUrl));
                    Operation.RaiseCanExecuteChanged();
                }
            }
        }
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged(nameof(SelectedTabIndex));
                }
            }
        }
        public string JsonBodiesForPost
        {
            get => _jsonBodiesForPost;
            set
            {
                if (_jsonBodiesForPost != value)
                {
                    _jsonBodiesForPost = value;
                    OnPropertyChanged(nameof(JsonBodiesForPost));
                    Operation.RaiseCanExecuteChanged();
                }
            }
        }

        //Commands
        public DelegateCommand Operation { get; set; }
        private bool CanExecuteOperation()
        {
            //TODO write some validation logic so see whether the entered JSON is a valid JSON Array or not!
            //Validate QueryStringJsonArray and UrlSegmentJsonArray for GET ||||| JsonBodiesForPost for Post OperationMode
            Debug.WriteLine("I am going to validate whether the added imput is a valid JSON array or not??");
            return true;
        }
        private void ExecuteOperation()
        {
            //TODO Execute Business Logic
            SelectedTabIndex = 1;
            Debug.WriteLine("I am Executing What Operation Mode says!");
        }
    }

    public enum OperationMode
    {
        GetMode,
        PostMode
    }
}
