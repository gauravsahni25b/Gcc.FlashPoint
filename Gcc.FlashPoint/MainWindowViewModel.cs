using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Gcc.FlashPoint.Core.BusinessLogic;
using Gcc.FlashPoint.Infrastructure;
using Newtonsoft.Json.Linq;
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
        private string _queryStringJsonArray = String.Empty;
        private string _urlSegmentJsonArray = String.Empty;
        private string _jsonBodiesForPost = String.Empty;
        private string _resultSentence = String.Empty;
        private string _baseUrl = "http://";
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
                    if (!ValidateJsonArray(value))
                    {
                        throw new ArgumentException("Invalid JSON Array!");
                    }
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
                    if (!ValidateJsonArray(value))
                    {
                        throw new ArgumentException("Invalid JSON Array!");
                    }
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
                    Operation.RaiseCanExecuteChanged();
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
                    if (!ValidateJsonArray(value))
                    {
                        throw new ArgumentException("Invalid JSON Array!");
                    }
                }
            }
        }

        public string ResultSentence
        {
            get => _resultSentence;
            set
            {
                if (_resultSentence != value)
                {
                    _resultSentence = value;
                    OnPropertyChanged(nameof(ResultSentence));
                }
            }
        }

        public ObservableCollection<ResultGridItem> Results { get; set; } = new ObservableCollection<ResultGridItem>();

        //Commands
        public DelegateCommand Operation { get; set; }
        private bool CanExecuteOperation()
        {
            return OperationMode == OperationMode.GetMode
                ? ValidateBaseUrl() && ValidateJsonArray(QueryStringJsonArray) && ValidateJsonArray(UrlSegmentJsonArray)
                : ValidateBaseUrl() && ValidateJsonArray(JsonBodiesForPost);
        }

        private bool ValidateJsonArray(string jsonArray)
        {
            try
            {
                var jsonArrayObject = JArray.Parse(jsonArray);
                return jsonArrayObject != null;
            }
            catch
            {
                return false;
            }
        }
        
        private bool ValidateBaseUrl()
        {
            return Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private async void ExecuteOperation()
        {
            Results.Clear();
            if (OperationMode == OperationMode.GetMode)
            {
                SelectedTabIndex = 2;
                UrlGeneratorForMassHttpGets urlGenerator = new UrlGeneratorForMassHttpGets(BaseUrl, QueryStringJsonArray, UrlSegmentJsonArray);
                HttpCallTimeSpanCalculator timeSpanCalculator = new HttpCallTimeSpanCalculator();
                foreach (string url in urlGenerator.Urls)
                {
                    Results.Add(new ResultGridItem(url, await timeSpanCalculator.MeasureGetCall(url)));
                }
                PopulateResultSentence(Results.Select(a => a.TimeTaken));
            }
            else
            {
                SelectedTabIndex = 2;
                HttpCallTimeSpanCalculator timeSpanCalculator = new HttpCallTimeSpanCalculator();
                var timeSpans = await timeSpanCalculator.MeasureNPostCalls(BaseUrl, JsonBodiesForPost);
                foreach (var timeSpan in timeSpans)
                {
                    Results.Add(new ResultGridItem(BaseUrl, timeSpan));
                }
                PopulateResultSentence(Results.Select(a => a.TimeTaken));
            }
        }
        private void PopulateResultSentence(IEnumerable<TimeSpan> timeSpans)
        {
            TimeSpan total = new TimeSpan(0);
            timeSpans.ToList().ForEach(a => total += a);
            ResultSentence = $"Number of Queries Ran = {timeSpans.Count()}. \nTotal Time Taken = {total}.";
        }
    }

    public enum OperationMode
    {
        GetMode,
        PostMode
    }

    public class ResultGridItem
    {
        public ResultGridItem(string url, TimeSpan timeTaken)
        {
            Url = url;
            TimeTaken = timeTaken;
        }
        public string Url { get; protected set; }
        public TimeSpan TimeTaken { get; protected set; }
    }
}
