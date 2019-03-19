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

namespace LumoTrack.App.Android.Adapters
{
    class InboxAdapter : BaseAdapter<DTO.InboxDTO>
    {

        Activity _context;
        List<DTO.InboxDTO> _entities;

        private TextView Title, Date, Message,ResponseDate,Response;

        private LinearLayout ResponseLayout;

        public InboxAdapter(Activity context, List<DTO.InboxDTO> entities)
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
        public override DTO.InboxDTO this[int position] => _entities[position];
        public void Clear() => _entities.Clear();

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var entity = _entities[position];

            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.InboxResponseLayout, null);

            Title = view.FindViewById<TextView>(Resource.Id.inboxTitle);
            Date = view.FindViewById<TextView>(Resource.Id.inboxDate);
            Message = view.FindViewById<TextView>(Resource.Id.inboxMessage);

            Title.Text = "Mensaje";
            Date.Text = entity.CreationDate.ToString("dd MMMM yyyy");
            Message.Text = entity.Message;


            if (entity.Response!=null)
            {
                ResponseDate = view.FindViewById<TextView>(Resource.Id.inboxresponseDateId);
                Response = view.FindViewById<TextView>(Resource.Id.inboxResponseId);

                ResponseLayout = view.FindViewById<LinearLayout>(Resource.Id.ResponseLayout);
                ResponseLayout.Visibility = ViewStates.Visible;

                ResponseDate.Text = entity.ResponseDate?.ToString("dd MMMM");
                Response.Text = entity.Response;
            }

            return view;
        }
    }
}