using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LumoTrack.App.Android.Adapters;
using LumoTrack.App.Android.Helpers;
using LumoTrack.Common;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using LumoTrack.ProxyContract;
using v4 = Android.Support.V4.App;

namespace LumoTrack.App.Android.Fragments
{
    public class CommentFragment : v4.Fragment
    {
        private View _view;

        public string UserID, FireBaseToken;

        public BasicProxiesFactory BasicProxiesFactory;

        Spinner _typeReportSpinner, _truckNameSpinner;

        List<DTO.VehicleDTO> _vehiclesList;

        EditText _messageText;

        Button _sendButton;

        public string truckID;

        LinearLayout _lottieAnimation;

        ScrollView _commentView;

        public CommentFragment(string truckid)
        {
            truckID = truckid;
        }

        public CommentFragment()
        {

        }


        private async Task Initializer()
        {
            var deviceManager = new DeviceManager();

            if (!deviceManager.IsDeviceRegister())
            {
                await deviceManager.UpsertDeviceAsync(0.0, 0.0);
            }
            UserID = deviceManager.GetDeviceID();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BasicProxiesFactory = new BasicProxiesFactory();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.CommentLayout, container, false);

            _lottieAnimation = _view.FindViewById<LinearLayout>(Resource.Id.animation_view);
            _commentView = _view.FindViewById<ScrollView>(Resource.Id.commentLayout);

            _lottieAnimation.Visibility = ViewStates.Visible;
            _commentView.Visibility = ViewStates.Gone;

            return _view;
        }

        public async override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

 
        }
        
       

        public async override void OnResume()
        {
            base.OnResume();


            var internetConnectionHelper = new InternetConnectionHelper();

            bool internetConnection = internetConnectionHelper.IsInternetAvailable();

            if (internetConnection)
            {
                try
                {
                    await Initializer();

                    Init();
                }
                catch (Exception e)
                {
                    OnDestroyView();
                }
            }
            else
            {
                Activity.RunOnUiThread(() =>
        Toast.MakeText(Activity, "Es necesario tener acceso a internet para usar la aplicación", ToastLength.Long).Show()
    );
            }

        }

        private void Init()
        {
            Task.WhenAll(LoadData()).ContinueWith((task) =>
            {
                if (_view != null)
                    Activity.RunOnUiThread(() => RenderView());
            });
        }

        private void LoadElements()
        {
            _typeReportSpinner = _view.FindViewById<Spinner>(Resource.Id.reportSpinner);

            _truckNameSpinner = _view.FindViewById<Spinner>(Resource.Id.truckSpinner);

            _messageText = _view.FindViewById<EditText>(Resource.Id.messageText);
            _messageText.SetOnTouchListener(new EditTextTouchListener());

            _sendButton = _view.FindViewById<Button>(Resource.Id.SendButton);
            _sendButton.Click += SendButtonOnClick;
        }

        private async Task LoadData()
        {
            try
            {
                IVehicleProxy vehicleProxy = BasicProxiesFactory.GetVehicleProxy();

                var response = await vehicleProxy.GetVehicle();

                _vehiclesList = response.Data;
            }
            catch (Exception e)
            {
                Activity.RunOnUiThread(() =>
                        Toast.MakeText(Activity, "¡Lo sentimos! El envío de comentarios no se encuentra disponible.", ToastLength.Long).Show()
                    );
            }
        }

        private void RenderView()
        {
            LoadElements();
            FillData();
        }

        private void FillData()
        {
            _lottieAnimation.Visibility = ViewStates.Gone;
            _commentView.Visibility = ViewStates.Visible;

            var enumValues = Enum.GetValues(typeof(Common.ReportTypesEnum));
            var objectList = ((IEnumerable)enumValues).Cast<Common.ReportTypesEnum>().ToList();
            var reportAdapters = objectList.Select(e => EnumHelper.GetDescription(e)).ToArray();
            _typeReportSpinner.Adapter = new ArrayAdapter<string>(Activity, Resource.Drawable.spinner_item, reportAdapters);
            _typeReportSpinner.SetMinimumHeight(30);

            var truckAdapters = _vehiclesList.Select(x => x.TruckName).ToArray();
            var trucklist = truckAdapters.ToList();
            trucklist.Insert(0, "No aplica / No conozco el dato");
            truckAdapters = trucklist.ToArray();
            _truckNameSpinner.Adapter = new TruckSpinner(Activity, Resource.Drawable.spinner_item, truckAdapters, _vehiclesList);
            _truckNameSpinner.SetMinimumHeight(30);

            if (truckID != "")
            {
                var vehicle = _vehiclesList.FirstOrDefault(x => x.Id.ToString() == truckID);
                var index = _vehiclesList.IndexOf(vehicle);
                _truckNameSpinner.SetSelection(index);
            }
            else
            {
                _truckNameSpinner.SetSelection(0);

                _truckNameSpinner.SetSelection(trucklist.Capacity - 1);

                _truckNameSpinner.SetSelection(0);
            }
            _lottieAnimation.Dispose();
            GarbageCollector();
        }

        private async void SendButtonOnClick(object sender, EventArgs e)
        {
            if (_messageText.Text == "")
            {
                await AlertDialogHelper.ShowAsync(Activity, "Atención", "Para enviar el mensaje con éxito, es necesario llenar todos los campos.", "OK", "");
                return;
            }

            _sendButton.Activated = false;
            IInboxProxy inboxProxy = BasicProxiesFactory.GetInboxProxy();

            Tuple<int, string> truckInfo = GetTruckId();

            Common.ReportTypesEnum reportType = GetReportType();

            DTO.InboxDTO inboxDTO = GenerateInboxMessage(truckInfo, reportType);

            try
            {
                _lottieAnimation.Visibility = ViewStates.Visible;
                _commentView.Visibility = ViewStates.Gone;

                var response = await inboxProxy.Create(inboxDTO);
            }
            catch (Exception ex)
            {
                _sendButton.Activated = true;
                Toast.MakeText(Activity, "¡Lo sentimos! El envío de comentarios no se encuentra disponible.", ToastLength.Long).Show();
            }

            FragmentManager.BeginTransaction().Replace(Resource.Id.fragments_container, new Fragments.MessageSend()).Commit();
        }

        private InboxDTO GenerateInboxMessage(Tuple<int, string> truckInfo, ReportTypesEnum reportType)
        {
            DTO.InboxDTO inboxDTO = new DTO.InboxDTO();

            inboxDTO.CreationDate = DateTime.Now;
            inboxDTO.Message = _messageText.Text;
            inboxDTO.ReportType = reportType;
            inboxDTO.TruckId = truckInfo.Item1.ToString();
            inboxDTO.TruckName = truckInfo.Item2;
            inboxDTO.UserId = UserID;

            return inboxDTO;
        }

        private ReportTypesEnum GetReportType()
        {
            var reportTypeString = _typeReportSpinner.SelectedItem.ToString();

            Common.ReportTypesEnum reportType = EnumHelper.GetValueFromDescription(reportTypeString);

            return reportType;
        }

        private Tuple<int, string> GetTruckId()
        {
            var truckname = _truckNameSpinner.SelectedItem.ToString();
            TruckSpinner adapter = (TruckSpinner)_truckNameSpinner.Adapter;
            int id = adapter.GetIdByValue(truckname);

            return Tuple.Create(id, truckname);
        }
        
        public override void OnDestroyView()
        {
            base.OnDestroyView();
            GarbageCollector();
        }

        private void GarbageCollector()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }


    }
}