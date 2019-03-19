using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LumoTrack.DTO;

namespace LumoTrack.App.Android.Adapters
{
    public class NotificationAdapter : BaseAdapter<DTO.NotificationDTO>
    {

        Activity _context;
        List<DTO.NotificationDTO> _entities;

        private TextView Title, Date,Message;
        private LinearLayout Importart;

        public NotificationAdapter(Activity context,List<DTO.NotificationDTO> entities)
        {
            _context = context;
            _entities = entities;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return _entities[position].Id;
        }

        public override long GetItemId(int position)
        {
            return _entities[position].Id;
        }


        public override int Count => _entities.Count;
        public override DTO.NotificationDTO this[int position] => _entities[position];
        public void Clear() => _entities.Clear();

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var entity = _entities[position];

            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.NotificationLayout, null);

            Title = view.FindViewById<TextView>(Resource.Id.notificationTitle);
            Date = view.FindViewById<TextView>(Resource.Id.notificationDate);
            Message = view.FindViewById<TextView>(Resource.Id.notificationMessage);
            Importart = view.FindViewById<LinearLayout>(Resource.Id.importantMessage);

            LinearLayout.LayoutParams ll = new LinearLayout.LayoutParams(Message.Width, Message.Height);

            if (entity.Important)
            {
                Importart.Visibility = ViewStates.Visible;
                
                ll.SetMargins(0, 30, 0, 0);
                Message.LayoutParameters = ll;
            }
            else
            {
                Importart.Visibility = ViewStates.Gone;
                ll.SetMargins(0, 0, 0, 0);
                Message.LayoutParameters = ll;
            }
               


            Title.Text = entity.Title;
            Date.Text = $"{entity.NotificationDate.ToString("dd MMMM yyyy") } - {entity.ExpiryDate.ToString("dd MMMM yyyy")}"; 
            Message.Text = entity.Message;

            return view;
        }
    }
}